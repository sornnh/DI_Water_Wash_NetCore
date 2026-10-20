using APIchecklistProcess.Model.ProductionLimit;
using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public sealed class ProductionLimitService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ProductionLimitService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// API-first: GET /api/ProductionLimit/PressureDecay?AssyPN=xxx
    /// Fallback: Truy vấn trực tiếp Parameters_SZ.dbo.Leak_Test_Limits
    /// </summary>
    public async Task<PressureDecayLimitResponse> GetPressureDecayLimitAsync(string assyPN)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var response = await _httpClient.GetAsync($"api/ProductionLimit/PressureDecay?AssyPN={Uri.EscapeDataString(assyPN)}");
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<PressureDecayLimitResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var result = new PressureDecayLimitResponse
                    {
                        AssyPN = assyPN
                    };
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $"SELECT TOP 1 * FROM [Parameters_SZ].[dbo].[Leak_Test_Limits] WHERE [Assy_PN] = '{assyPN}' ORDER BY [Date_Time] DESC";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            result.Success = true;
                            result.Code = 200;
                            result.Message = "OK (DB Fallback)";
                            
                            result.DecayPressureFill = GetDoubleOrNull(row, "Pressure");
                            result.DecayPressureFillTolerance = GetDoubleOrNull(row, "P_Tolerance");
                            result.DecayDropPressureMax = GetDoubleOrNull(row, "Max_dP");
                            result.DecayStabilize_Time = GetDoubleOrNull(row, "Stabilize_Time");
                            result.DecayTest_Time = GetDoubleOrNull(row, "Decay_Time");
                            result.DecayVent_Time = GetDoubleOrNull(row, "Vent_Time");
                            
                            result.MaxHumidity = GetDoubleOrNull(row, "PDc_Max_Humidity");
                            result.PreDrying = GetBoolOrNull(row, "Pre_Drying");
                            result.MaxTestAttempts = GetIntOrNull(row, "Max_Test_Attempts");
                            result.TestToPassIfFail = GetIntOrNull(row, "Test_to_Pass_if_Faily");
                        }
                        else
                        {
                            result.Success = false;
                            result.Code = 404;
                            result.Message = "Not found in DB Fallback";
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Code = 500;
                        result.Message = $"DB Fallback Error: {ex.Message}";
                    }
                    return result;
                });
            },
            actionName: $"GetPressureDecayLimitAsync ({assyPN})",
            isResultSuccessful: res => res != null && res.Success
        );
    }
    public async Task<DIWaterWashLimitResponse> GetDIWaterWashLimitAsync(string assyPN)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var response = await _httpClient.GetAsync($"api/ProductionLimit/DIWaterWash?AssyPN={Uri.EscapeDataString(assyPN)}");
                if (!response.IsSuccessStatusCode) return null;
                return await response.Content.ReadFromJsonAsync<DIWaterWashLimitResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    var result = new DIWaterWashLimitResponse   
                    {
                        AssyPN = assyPN
                    };
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = @$"SELECT TOP 1 
                                            limits.*, 
                                            part_numbers.[Max_Test_Attempts] AS [Aavid_Max_Test_Attempts],
                                            part_numbers.[Test_to_Pass_if_Fail] AS [Aavid_Test_to_Pass_if_Fail] FROM [Parameters_SZ].[dbo].[Leak_Test_Limits] AS limits
                                        LEFT JOIN [Parameters_SZ].[dbo].[Aavid_Part_Numbers] AS part_numbers
                                            ON part_numbers.[ASSY_PN] = limits.[Assy_PN]
                                        WHERE limits.[Assy_PN] = '{assyPN}'
                                        ORDER BY limits.[Date_Time] DESC, part_numbers.[Date_Time] DESC";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            result.Success = true;
                            result.Code = 200;
                            result.Message = "OK (DB Fallback)";
                            result.MaxTestAttempts = GetIntOrNull(row, "Max_Test_Attempts");
                            result.TestToPassIfFail = GetIntOrNull(row, "Test_to_Pass_if_Faily");
                            result.MaxTestAttempts = GetIntOrNull(row, "Aavid_Max_Test_Attempts");
                            result.TestToPassIfFail = GetIntOrNull(row, "Aavid_Test_to_Pass_if_Fail");
                            result.WashCycles = GetIntOrNull(row, "Number_of_Wash_cycles");
                            result.PreWashingTime = GetIntOrNull(row, "DI_Pre_Wash_Time");
                            result.WashingTime = GetIntOrNull(row, "DI_Washing_Time");
                            result.ReverseWashingFlow = GetBoolOrNull(row, "Reverse_Washing_Flow");
                            result.DIReverseWashingTime = GetIntOrNull(row, "DI_Reverse_Washing_Time");
                            result.ReverseDIFlushingFlow = GetBoolOrNull(row, "Reverse_DI_Flushing_Flow");
                            result.DIDryingTime = GetIntOrNull(row, "DI_Drying_Time");
                            result.ReverseDIDryingTime = GetIntOrNull(row, "Reversed_DI_Drying_Time");
                            result.DIFlowRate = GetDoubleOrNull(row, "DI_Flow_Rate");
                            result.DIFlowTolerance = GetDoubleOrNull(row, "DI_Flow_Tol");
                            result.DIMaxAirPressure = GetDoubleOrNull(row, "Flush_Air_Pressure_Max");
                            result.DIMinAirPressure = GetDoubleOrNull(row, "Flush_Air_Pressure_Min");
                            result.DIMaxWaterPressure = GetDoubleOrNull(row, "Water_Pressure_Max");
                            result.DIMinWaterPressure = GetDoubleOrNull(row, "Water_Pressure_Min");
                            result.CheckDIHumidity = GetBoolOrNull(row, "Check_DI_Humidity");
                            result.DIMaxHumidity = GetIntOrNull(row, "DI_Max_Humidity");
                        }
                        else
                        {
                            result.Success = false;
                            result.Code = 404;
                            result.Message = "Not found in DB Fallback";
                        }
                    }
                    catch (Exception ex)
                    {
                        result.Success = false;
                        result.Code = 500;
                        result.Message = $"DB Fallback Error: {ex.Message}";
                    }
                    return result;
                });
            },
            actionName: $"GetPressureDecayLimitAsync ({assyPN})",
            isResultSuccessful: res => res != null && res.Success
        );
    }
    private double? GetDoubleOrNull(DataRow row, string colName)
    {
        if (row.Table.Columns.Contains(colName) && row[colName] != DBNull.Value)
            return Convert.ToDouble(row[colName]);
        return null;
    }
    
    private int? GetIntOrNull(DataRow row, string colName)
    {
        if (row.Table.Columns.Contains(colName) && row[colName] != DBNull.Value)
            return Convert.ToInt32(row[colName]);
        return null;
    }
    
    private bool? GetBoolOrNull(DataRow row, string colName)
    {
        if (row.Table.Columns.Contains(colName) && row[colName] != DBNull.Value)
            return Convert.ToBoolean(row[colName]);
        return null;
    }
}
