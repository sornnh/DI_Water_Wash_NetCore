using APIchecklistProcess.Model.ProcessValidate;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public sealed class ProcessCheckService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ProcessCheckService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ProcessCheckResponse> CheckProcessAsync(
        string assyPN,
        string serialNumber,
        string workOrder,
        string processCheck,
        int unitIndex = 0)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new ProcessCheckRequest
                {
                    AssyPN = assyPN,
                    SerialNumber = serialNumber,
                    WorkOrder = workOrder,
                    ProcessCheck = processCheck
                };

                var response = await _httpClient.PostAsJsonAsync("api/process/check", request);
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<ProcessCheckResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var res = new ProcessCheckResponse
                    {
                        AssyPN = assyPN,
                        SerialNumber = serialNumber,
                        WorkOrder = workOrder,
                        ProcessCheck = processCheck
                    };

                    try
                    {
                        var unit = (ClsUnitManagercs.cls_Units != null && unitIndex >= 0 && unitIndex < ClsUnitManagercs.cls_Units.Length)
                            ? ClsUnitManagercs.cls_Units[unitIndex]
                            : null;

                        // 1. Format check
                        if (unit != null && !IsSNValidFormatLocal(serialNumber, unit))
                        {
                            res.Success = false;
                            res.Code = 8;
                            res.Message = "SN format is invalid";
                            return res;
                        }

                        // 2. WO Check
                        if (unit != null && unit.bVerify_SN_in_WO)
                        {
                            if (serialNumber.Length >= unit.iSNin_WO_Ctl_String_Offset + unit.iSNin_WO_Ctl_String_Length)
                            {
                                if (int.TryParse(serialNumber.Substring(unit.iSNin_WO_Ctl_String_Offset, unit.iSNin_WO_Ctl_String_Length), out int iSNindexWO))
                                {
                                    if (iSNindexWO < unit.iFirst_SN_in_WO || iSNindexWO > unit.iLast_SN_in_WO)
                                    {
                                        res.Success = false;
                                        res.Code = 7;
                                        res.Message = $"SN is not in WO {unit.WO}, SN should be between {unit.iFirst_SN_in_WO} and {unit.iLast_SN_in_WO}";
                                        return res;
                                    }
                                }
                            }
                        }

                        // 3. DB Fallback checks
                        string[] proceslist = unit?.verifyFlags;
                        if (proceslist == null || proceslist.Length == 0)
                        {
                            res.Success = false;
                            res.Code = 1;
                            res.Message = "Process list is null, call technical to support!!!";
                            return res;
                        }

                        int procIndex = System.Array.IndexOf(proceslist, processCheck);
                        if (procIndex == -1)
                        {
                            res.Success = false;
                            res.Code = 2;
                            res.Message = "Process don't include this station!!!";
                            return res;
                        }

                        var dbProd = new Cls_DBMsSQL();
                        dbProd.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
                        dbProd.Open();

                        // Get previous processes
                        string[] processesBefore = new string[procIndex];
                        System.Array.Copy(proceslist, 0, processesBefore, 0, procIndex);

                        bool allBeforePass = true;
                        foreach (string p in processesBefore)
                        {
                            string sqlP = $"SELECT TOP 1 FailCode FROM [Production_SZ].[dbo].[Production_Flow_All_Months_Log] WHERE Serial = '{serialNumber}' AND Process_Step = '{p}' ORDER BY Date_Time DESC";
                            var dtP = dbProd.ExecuteQuery(sqlP);
                            if (dtP == null || dtP.Rows.Count == 0 || (dtP.Rows[0]["FailCode"]?.ToString().Trim().ToUpper() != "OK" && dtP.Rows[0]["FailCode"]?.ToString().Trim().ToUpper() != "PASS"))
                            {
                                allBeforePass = false;
                                break;
                            }
                        }

                        if (!allBeforePass)
                        {
                            dbProd.Close();
                            res.Success = false;
                            res.Code = 3;
                            res.Message = "This product did not pass in process before, please check process log!!!";
                            return res;
                        }

                        // Check current process
                        string sqlCur = $"SELECT TOP 1 FailCode FROM [Production_SZ].[dbo].[Production_Flow_All_Months_Log] WHERE Serial = '{serialNumber}' AND Process_Step = '{processCheck}' ORDER BY Date_Time DESC";
                        var dtCur = dbProd.ExecuteQuery(sqlCur);
                        if (dtCur != null && dtCur.Rows.Count > 0)
                        {
                            string curFail = dtCur.Rows[0]["FailCode"]?.ToString().Trim().ToUpper();
                            if (curFail == "OK" || curFail == "PASS")
                            {
                                dbProd.Close();
                                res.Success = false;
                                res.Code = 4;
                                res.Message = "This product already passed in this staion!!!";
                                return res;
                            }
                        }

                        // Check MRB
                        string sqlMrb = $"SELECT TOP 1 FailCode FROM [Production_SZ].[dbo].[Aavid_MRB_Rework_Log_Station] WHERE Serial = '{serialNumber}' ORDER BY Date_Time DESC";
                        var dtMrb = dbProd.ExecuteQuery(sqlMrb);
                        if (dtMrb != null && dtMrb.Rows.Count > 0)
                        {
                            string mrbFail = dtMrb.Rows[0]["FailCode"]?.ToString().Trim().ToUpper();
                            if (mrbFail != "OK" && mrbFail != "PASS")
                            {
                                dbProd.Close();
                                res.Success = false;
                                res.Code = 5;
                                res.Message = "This product is in MRB, please check MRB log!!!";
                                return res;
                            }
                        }

                        // Check Linked Part
                        if (unit != null && unit.Verify_Linked_Part)
                        {
                            string sqlLink = $"SELECT TOP 1 1 FROM [Production_SZ].[dbo].[LCS_LCP_Serial_Numbers_Link] WHERE Serial = '{serialNumber}' AND Assy_PN = '{assyPN}'";
                            var dtLink = dbProd.ExecuteQuery(sqlLink);
                            if (dtLink == null || dtLink.Rows.Count == 0)
                            {
                                dbProd.Close();
                                res.Success = false;
                                res.Code = 6;
                                res.Message = "This product is not linked yet!!!";
                                return res;
                            }
                        }

                        dbProd.Close();
                        res.Success = true;
                        res.Code = 0;
                        res.Message = "OK (DB Fallback)";
                        return res;
                    }
                    catch (System.Exception ex)
                    {
                        res.Success = false;
                        res.Code = 500;
                        res.Message = $"DB Fallback error: {ex.Message}";
                        return res;
                    }
                });
            },
            actionName: $"CheckProcessAsync ({serialNumber}-{processCheck})",
            isResultSuccessful: res => res != null && (res.Success || res.Code != 0)
        );
    }

    private static bool IsSNValidFormatLocal(string sn, ClsUnit unit)
    {
        if (string.IsNullOrWhiteSpace(sn)) return false;
        sn = sn.ToUpper().Trim();
        if (unit.SN_Length > 0 && sn.Length != unit.SN_Length) return false;
        if (unit.Ctl_Str_Length > 0 && unit.Ctl_Str_Offset + unit.Ctl_Str_Length <= sn.Length)
        {
            string text = sn.Substring(unit.Ctl_Str_Offset, unit.Ctl_Str_Length);
            return text.Equals(unit.PN_Ctl_String, System.StringComparison.OrdinalIgnoreCase);
        }
        return true;
    }
    public async Task<ProcessListResponse> GetProcessListAsync(string assyPN)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new ProcessListRequest
                {
                    AssyPN = assyPN
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "api/process/list",
                    request);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<ProcessListResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var result = new ProcessListResponse();
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $"SELECT TOP 1 Process_List FROM [Parameters_SZ].[dbo].[Process_Flow_for_Part_Numbers] WHERE Assy_PN = '{assyPN}' order by Date_Time DESC";
                        System.Data.DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string processListStr = dt.Rows[0]["Process_List"]?.ToString() ?? string.Empty;
                            var array = processListStr.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            
                            var list = new System.Collections.Generic.List<string>();
                            foreach (var item in array)
                            {
                                list.Add(item.Trim());
                            }
                            
                            result.Success = true;
                            result.Code = 200;
                            result.Message = "OK (DB Fallback)";
                            result.ProcessList = list;
                        }
                        else
                        {
                            result.Success = false;
                            result.Code = 404;
                            result.Message = "Process list not found in DB Fallback";
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Code = 500;
                        result.Message = $"DB Fallback error: {ex.Message}";
                    }
                    return result;
                });
            },
            actionName: $"GetProcessListAsync ({assyPN})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    public async Task<APIchecklistProcess.Model.RecordProcessResultResponse> RecordProcessResultAsync(
        APIchecklistProcess.Model.RecordProcessResultRequest request)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var response = await _httpClient.PostAsJsonAsync("api/process/record-result", request);
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<APIchecklistProcess.Model.RecordProcessResultResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var res = new APIchecklistProcess.Model.RecordProcessResultResponse
                    {
                        SerialNumber = request.SerialNumber,
                        ProcessStep = request.ProcessStep
                    };
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
                        db.Open();

                        bool bMain = db.SaveToLogMonths_LogTable(
                            request.SerialNumber,
                            request.AssyPN,
                            System.DateTime.Now,
                            request.Station ?? "",
                            request.FailCode ?? "PASS",
                            request.ProcessStep,
                            request.StepNo.ToString(),
                            request.Remark ?? "",
                            request.WorkOrder ?? "",
                            request.Operator ?? "");

                        if (bMain) res.SavedTables.Add("Production_Flow_All_Months_Log");

                        if (request.ExtraData != null && (request.ExtraData.ContainsKey("Pressure_Fill") || request.ExtraData.ContainsKey("PressureFill")))
                        {
                            double fill = 0, drop = 0;
                            if (request.ExtraData.TryGetValue("Pressure_Fill", out var fVal) || request.ExtraData.TryGetValue("PressureFill", out fVal))
                                double.TryParse(fVal?.ToString(), out fill);
                            if (request.ExtraData.TryGetValue("Pressure_Drop", out var dVal) || request.ExtraData.TryGetValue("PressureDrop", out dVal))
                                double.TryParse(dVal?.ToString(), out drop);

                            string fixId = request.ExtraData.TryGetValue("FixID", out var fx) ? fx?.ToString() ?? "" : "";
                            string dpUnit = request.ExtraData.TryGetValue("Dp_Unit", out var dp) ? dp?.ToString() ?? "" : "";
                            string fillUnit = request.ExtraData.TryGetValue("Fill_Unit", out var fu) ? fu?.ToString() ?? "" : "";
                            string testType = request.ExtraData.TryGetValue("Test_Type", out var tt) ? tt?.ToString() ?? "" : "";

                            bool bPressure = db.SaveToPressureDecayLog(
                                request.SerialNumber,
                                request.AssyPN,
                                request.Station ?? "",
                                request.FailCode ?? "PASS",
                                testType,
                                fill,
                                request.Operator ?? "",
                                request.Remark ?? "",
                                request.WorkOrder ?? "",
                                drop,
                                fixId,
                                dpUnit,
                                fillUnit,
                                request.StepNo.ToString());

                            if (bPressure) res.SavedTables.Add("Aavid_Leak_Test_Data");
                        }
                        else if (request.ExtraData != null && request.ExtraData.ContainsKey("Flowrate"))
                        {
                            float flow = 0, waterP = 0, airP = 0;
                            if (request.ExtraData.TryGetValue("Water_Flow_Rate", out var fl)) float.TryParse(fl?.ToString(), out flow);
                            if (request.ExtraData.TryGetValue("Water_Pressure", out var wp)) float.TryParse(wp?.ToString(), out waterP);
                            if (request.ExtraData.TryGetValue("Air_Pressure", out var ap)) float.TryParse(ap?.ToString(), out airP);
                            int fixId =int.Parse(request.ExtraData.TryGetValue("Fixture_No", out var fx) ? fx?.ToString() ?? "0" : "0");
                            bool bWater = db.SaveToDIWaterWashLog(
                                request.SerialNumber,
                                request.AssyPN,
                                request.Station ?? "",
                                request.FailCode ?? "PASS",
                                request.Operator ?? "",
                                request.Remark ?? "",
                                request.WorkOrder ?? "",
                                request.StepNo,
                                fixId,
                                flow,
                                waterP,
                                airP);

                            if (bWater) res.SavedTables.Add("DI_Water_Wash_Log");
                        }

                        db.Close();

                        res.Success = bMain;
                        res.Code = bMain ? 200 : 500;
                        res.Message = bMain ? "Recorded (DB Fallback)" : "DB Fallback insert failed";
                    }
                    catch (System.Exception ex)
                    {
                        res.Success = false;
                        res.Code = 500;
                        res.Message = $"DB Fallback error: {ex.Message}";
                    }
                    return res;
                });
            },
            actionName: $"RecordProcessResult ({request.SerialNumber}-{request.ProcessStep})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    public async Task<MoveToMRBResponse> MoveToMRBAsync(MoveToMRBRequest request)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var response = await _httpClient.PostAsJsonAsync("api/process/mrb", request);
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<MoveToMRBResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var res = new MoveToMRBResponse
                    {
                        SerialNumber = request.SerialNumber,
                        AssyPN = request.AssyPN
                    };
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        bool success = db.MoveToMRBLog(
                            request.SerialNumber,
                            request.AssyPN,
                            request.DateTime ?? System.DateTime.Now,
                            request.Station,
                            request.FailCode,
                            string.IsNullOrWhiteSpace(request.MrbCodes) ? request.FailCode : request.MrbCodes,
                            request.Process);
                        db.Close();

                        res.Success = success;
                        res.Code = success ? 200 : 500;
                        res.Message = success ? "OK (DB Fallback)" : "DB Fallback insert failed";
                    }
                    catch (System.Exception ex)
                    {
                        res.Success = false;
                        res.Code = 500;
                        res.Message = $"DB Fallback error: {ex.Message}";
                    }
                    return res;
                });
            },
            actionName: $"MoveToMRBAsync ({request.SerialNumber})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    public async Task<CheckMaxTestAttemptsResponse> CheckMaxTestAttemptsAsync(CheckMaxTestAttemptsRequest request, int unitIndex = 0)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var response = await _httpClient.PostAsJsonAsync("api/process/check-max-attempts", request);
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<CheckMaxTestAttemptsResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var res = new CheckMaxTestAttemptsResponse
                    {
                        SerialNumber = request.SerialNumber,
                        AssyPN = request.AssyPN,
                        ProcessStep = request.ProcessStep,
                        StepNo = request.StepNo
                    };
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
                        db.Open();

                        string sn = request.SerialNumber;
                        string proc = request.ProcessStep;

                        string queryMRB = $"SELECT TOP 1 [FailCode], [Date_Time] FROM [Production_SZ].[dbo].[Aavid_MRB_Rework_Log_Station] WHERE Serial = '{sn}' ORDER BY Date_Time DESC";
                        System.Data.DataTable dtMRB = db.ExecuteQuery(queryMRB);

                        bool isReworkProduct = false;
                        System.DateTime DT_Rework = System.DateTime.MinValue;
                        if (dtMRB != null && dtMRB.Rows.Count > 0)
                        {
                            System.Data.DataRow row = dtMRB.Rows[0];
                            string failCode = row["FailCode"]?.ToString().Trim().ToUpper();
                            if (failCode == "OK" || failCode == "PASS")
                            {
                                DT_Rework = System.Convert.ToDateTime(row["Date_Time"]);
                                isReworkProduct = true;
                            }
                        }

                        int iStep = request.StepNo ?? 0;
                        string cleanProcess = proc;
                        for (int index = 0; index < 10; index++)
                        {
                            if (proc.EndsWith($"-{index}"))
                            {
                                iStep = index;
                                cleanProcess = proc.Replace($"-{index}", "");
                                break;
                            }
                        }

                        string stepCondition = (iStep != 0) ? $"AND [Step_No] = '{iStep}'" : "";
                        StateCommon.GetTableName(cleanProcess, out string TableName, out _);

                        string query2;
                        if (isReworkProduct)
                        {
                            if (TableName.Contains("Production_Flow_"))
                            {
                                query2 = $@"SELECT [Failcode] FROM [Production_SZ].[dbo].[{TableName}] WHERE Serial ='{sn}' 
                                           AND [Process_Step] = '{cleanProcess}' {stepCondition}
                                           AND Date_Time > '{DT_Rework.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY Date_Time ASC";
                            }
                            else
                            {
                                query2 = $@"SELECT [Failcode] FROM [Production_SZ].[dbo].[{TableName}] WHERE Serial ='{sn}' 
                                           AND Date_Time > '{DT_Rework.ToString("yyyy-MM-dd HH:mm:ss")}' ORDER BY Date_Time ASC";
                            }
                        }
                        else
                        {
                            if (TableName.Contains("Production_Flow_"))
                            {
                                query2 = $@"SELECT [Failcode] FROM [Production_SZ].[dbo].[{TableName}] WHERE Serial ='{sn}' 
                                           AND [Process_Step] = '{cleanProcess}' {stepCondition} ORDER BY Date_Time ASC";
                            }
                            else
                            {
                                query2 = $@"SELECT [Failcode] FROM [Production_SZ].[dbo].[{TableName}] WHERE Serial ='{sn}' ORDER BY Date_Time ASC";
                            }
                        }

                        System.Data.DataTable dt2 = db.ExecuteQuery(query2);
                        db.Close();

                        int maxAttempts = request.MaxTestAttempts > 0 ? request.MaxTestAttempts : 3;
                        if (ClsUnitManagercs.cls_Units != null && unitIndex >= 0 && unitIndex < ClsUnitManagercs.cls_Units.Length)
                        {
                            if (ClsUnitManagercs.cls_Units[unitIndex].iMax_Test_Attempts > 0)
                            {
                                maxAttempts = ClsUnitManagercs.cls_Units[unitIndex].iMax_Test_Attempts;
                            }
                        }

                        res.MaxTestAttempts = maxAttempts;

                        if (dt2 != null && dt2.Rows.Count > 0)
                        {
                            res.TestAttemptsCount = dt2.Rows.Count;
                            var failCode = dt2.Rows[dt2.Rows.Count - 1]["Failcode"]?.ToString().Trim();
                            if (!string.Equals(failCode, "OK", System.StringComparison.OrdinalIgnoreCase) &&
                                !string.Equals(failCode, "PASS", System.StringComparison.OrdinalIgnoreCase) &&
                                !failCode.StartsWith("Abort", System.StringComparison.OrdinalIgnoreCase))
                            {
                                res.IsMaxTestAttempts = true;
                            }
                            else
                            {
                                res.IsMaxTestAttempts = (dt2.Rows.Count + 1) >= maxAttempts;
                            }
                        }
                        else
                        {
                            res.TestAttemptsCount = 0;
                            res.IsMaxTestAttempts = false;
                        }

                        res.Success = true;
                        res.Code = 200;
                        res.Message = "OK (DB Fallback)";
                    }
                    catch (System.Exception ex)
                    {
                        res.Success = false;
                        res.Code = 500;
                        res.Message = $"DB Fallback error: {ex.Message}";
                    }
                    return res;
                });
            },
            actionName: $"CheckMaxTestAttempts ({request.SerialNumber})",
            isResultSuccessful: res => res != null && res.Success
        );
    }
}