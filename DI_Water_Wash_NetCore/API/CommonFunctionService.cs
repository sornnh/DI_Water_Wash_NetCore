using APIchecklistProcess.Model.CommonFunction;
using System;
using System.Collections.Generic;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public sealed class CommonFunctionService
{
    private readonly HttpClient _httpClient;

    // ASP.NET Core API trả JSON camelCase, cần bật PropertyNameCaseInsensitive
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public CommonFunctionService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GetPasswordResponse> GetPasswordAsync(string function)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new GetPasswordRequest
                {
                    Function = function
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "api/HelperCommonFunction/password",
                    request);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<GetPasswordResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "ApprovalDB", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $"SELECT TOP 1 Password FROM [ApprovalDB].[dbo].[Account] WHERE [User] = '{function}'";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            return new GetPasswordResponse
                            {
                                Success = true,
                                Code = 200,
                                Message = "OK (DB Fallback)",
                                Function = function,
                                Password = dt.Rows[0]["Password"]?.ToString() ?? string.Empty
                            };
                        }
                    }
                    catch { }

                    return new GetPasswordResponse
                    {
                        Success = false,
                        Code = 404,
                        Message = "Password function not found in DB Fallback",
                        Function = function,
                        Password = string.Empty
                    };
                });
            },
            actionName: $"GetPasswordAsync ({function})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    /// <summary>
    /// Lấy toàn bộ danh sách Part Number (AssyPN + Description).
    /// API-first: GET /api/HelperCommonFunction/part-numbers
    /// Fallback: Truy vấn trực tiếp DB Parameters_SZ.dbo.Aavid_Part_Numbers
    /// </summary>
    public async Task<PartNumberListResponse> GetAllPartNumbersAsync()
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            // === API call ===
            async () =>
            {
                var response = await _httpClient.GetAsync("api/HelperCommonFunction/part-numbers");
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<PartNumberListResponse>(_jsonOptions);
            },
            // === DB Fallback ===
            async () =>
            {
                return await Task.Run(() =>
                {
                    var result = new PartNumberListResponse();
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = "SELECT ASSY_PN, Description FROM [Parameters_SZ].[dbo].[Aavid_Part_Numbers]";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            var items = new List<PartNumberItem>();
                            foreach (DataRow row in dt.Rows)
                            {
                                items.Add(new PartNumberItem
                                {
                                    AssyPN = row["ASSY_PN"]?.ToString() ?? string.Empty,
                                    Description = row["Description"]?.ToString() ?? string.Empty
                                });
                            }
                            result.Success = true;
                            result.Code = 200;
                            result.Message = "OK (DB Fallback)";
                            result.Data = items;
                        }
                        else
                        {
                            result.Success = false;
                            result.Code = 404;
                            result.Message = "No part numbers found in DB";
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
            actionName: "GetAllPartNumbersAsync",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    /// <summary>
    /// API-first: POST /api/HelperCommonFunction/enable-item
    /// Fallback: Truy vấn trực tiếp ApprovalDB.dbo.ControlItem
    /// </summary>
    public async Task<GetEnableItemResponse> GetEnableItemAsync(string enableItem)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new GetEnableItemRequest { EnableItem = enableItem };
                var response = await _httpClient.PostAsJsonAsync("api/HelperCommonFunction/enable-item", request);
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<GetEnableItemResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var result = new GetEnableItemResponse();
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "ApprovalDB", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $"SELECT TOP 1 [Enable] FROM [ApprovalDB].[dbo].[ControlItem] WHERE [EnableItem] = '{enableItem}'";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        result.Success = true;
                        result.Code = 200;
                        result.Message = "OK (DB Fallback)";
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["Enable"] != DBNull.Value)
                        {
                            result.Enable = Convert.ToBoolean(dt.Rows[0]["Enable"]);
                        }
                        else
                        {
                            result.Enable = false;
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
            actionName: $"GetEnableItemAsync ({enableItem})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    /// <summary>
    /// API-first: POST /api/HelperCommonFunction/sn-verify
    /// Fallback: Truy vấn trực tiếp Parameters_SZ.dbo.Aavid_Part_Numbers và Production_SZ
    /// </summary>
    public async Task<GetSNVerifyResponse> GetSNVerifyAsync(string assyPN, string workOrder)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new GetSNVerifyRequest { AssyPN = assyPN, WorkOrder = workOrder };
                var response = await _httpClient.PostAsJsonAsync("api/HelperCommonFunction/sn-verify", request);
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<GetSNVerifyResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var result = new GetSNVerifyResponse();
                    try
                    {
                        var dbParam = new Cls_DBMsSQL();
                        dbParam.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
                        dbParam.Open();
                        string query = $@"SELECT 
                                            SN_Length,
                                            PN_Ctl_String,
                                            Ctl_Str_Length,
                                            Ctl_Str_Offset,
                                            [No_Linked_Units],
                                            [No_Linked_PN_2],
                                            [No_Linked_PN_3],
                                            [No_Linked_PN_4],
                                            [No_Linked_PN_5],
                                            [No_Linked_PN_6],
                                            [No_Linked_PN_7],
                                            [No_Linked_PN_8],
                                            [Customer_Name],
                                            [Serial_Length],
                                            [SN_Offset],
                                            [Description]
                                            FROM [Parameters_SZ].[dbo].[Aavid_Part_Numbers] WHERE [ASSY_PN] = '{assyPN}' ORDER BY Date_Time DESC";
                        DataTable dt = dbParam.ExecuteQuery(query);
                        dbParam.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            result.Success = true;
                            result.Code = 200;
                            result.Message = "OK (DB Fallback)";

                            result.SN_Length = GetSafeInt(row, "SN_Length");
                            result.PN_Ctl_String = row["PN_Ctl_String"]?.ToString() ?? string.Empty;
                            result.Ctl_Str_Length = GetSafeInt(row, "Ctl_Str_Length");
                            result.Ctl_Str_Offset = GetSafeInt(row, "Ctl_Str_Offset");
                            result.Customer_Name = row["Customer_Name"]?.ToString() ?? string.Empty;
                            result.Serial_Length = GetSafeInt(row, "Serial_Length");
                            result.SN_Offset = GetSafeInt(row, "SN_Offset");
                            result.Description = row["Description"]?.ToString() ?? string.Empty;

                            // Linked part check
                            string[] linkedColumns = { "No_Linked_Units", "No_Linked_PN_2", "No_Linked_PN_3", "No_Linked_PN_4",
                                                       "No_Linked_PN_5", "No_Linked_PN_6", "No_Linked_PN_7", "No_Linked_PN_8" };
                            foreach (string col in linkedColumns)
                            {
                                if (dt.Columns.Contains(col) && row[col] != DBNull.Value && Convert.ToInt32(row[col]) > 0)
                                {
                                    result.Verify_Linked_Part = true;
                                    break;
                                }
                            }

                            // WO check
                            if (!string.IsNullOrEmpty(result.Customer_Name)
                                && result.Customer_Name.IndexOf("GTS", StringComparison.OrdinalIgnoreCase) >= 0
                                && result.Serial_Length > 0
                                && result.SN_Offset > 0
                                && !string.IsNullOrEmpty(workOrder))
                            {
                                var dbProd = new Cls_DBMsSQL();
                                dbProd.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
                                dbProd.Open();
                                string queryWO = $@"SELECT [First_Serial], [Last_Serial]
                                                  FROM [Production_SZ].[dbo].[BOYD_Production_Work_Orders]
                                                  WHERE [ASSY_PN] = '{assyPN}' AND [Work_Order] = '{workOrder}' ORDER BY Date_Time DESC";
                                DataTable dtWO = dbProd.ExecuteQuery(queryWO);
                                dbProd.Close();

                                if (dtWO != null && dtWO.Rows.Count > 0)
                                {
                                    int firstSN = GetSafeInt(dtWO.Rows[0], "First_Serial");
                                    int lastSN = GetSafeInt(dtWO.Rows[0], "Last_Serial");

                                    // Check enable item
                                    bool checkWOEnable = false;
                                    try
                                    {
                                        var dbApp = new Cls_DBMsSQL();
                                        dbApp.Initialize("10.102.4.20", "ApprovalDB", "readuser", "Boyd@2025");
                                        dbApp.Open();
                                        DataTable dtApp = dbApp.ExecuteQuery("SELECT [Enable] FROM [ApprovalDB].[dbo].[ControlItem] WHERE [EnableItem] = 'CheckSNbyWO'");
                                        dbApp.Close();
                                        if (dtApp != null && dtApp.Rows.Count > 0 && dtApp.Rows[0]["Enable"] != DBNull.Value)
                                        {
                                            checkWOEnable = Convert.ToBoolean(dtApp.Rows[0]["Enable"]);
                                        }
                                    }
                                    catch { }

                                    if (firstSN > 0 && lastSN > 0 && checkWOEnable)
                                    {
                                        result.Verify_SN_in_WO = true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Code = 404;
                            result.Message = "Part number not found in DB Fallback";
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
            actionName: $"GetSNVerifyAsync ({assyPN}, {workOrder})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    private static int GetSafeInt(DataRow row, string columnName)
    {
        if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
        {
            return Convert.ToInt32(row[columnName]);
        }
        return 0;
    }
}

