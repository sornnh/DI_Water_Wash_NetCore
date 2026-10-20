using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public sealed class GoldenSampleService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public GoldenSampleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<GoldenSettingTestResponse> GetGoldenTestSetting(
        string assyPN,
        string processCheck)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new GoldenSettingTestGetRequest
                {
                    AssyPN = assyPN,
                    ProcessCheck = processCheck
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "api/GoldenSample/SettingGoldenTest",
                    request);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<GoldenSettingTestResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $"SELECT TOP 1 IsEnable, TestIntervalHour, FailedContinues FROM [Parameters_SZ].[dbo].[Setting_GoldenSampleTest] WHERE Assy_PN = '{assyPN}' AND Process = '{processCheck}'";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            return new GoldenSettingTestResponse
                            {
                                Success = true,
                                Code = 200,
                                Message = "OK (DB Fallback)",
                                AssyPN = assyPN,
                                ProcessCheck = processCheck,
                                IsEnable = row["IsEnable"] != DBNull.Value && Convert.ToBoolean(row["IsEnable"]),
                                TestIntervalHour = row["TestIntervalHour"] != DBNull.Value ? Convert.ToInt32(row["TestIntervalHour"]) : 4,
                                FailedContinues = row["FailedContinues"] != DBNull.Value ? Convert.ToInt32(row["FailedContinues"]) : 3
                            };
                        }
                    }
                    catch { }

                    return new GoldenSettingTestResponse
                    {
                        Success = false,
                        Code = 500,
                        Message = "Setting Golden Sample not found in DB Fallback",
                        AssyPN = assyPN,
                        ProcessCheck = processCheck,
                        IsEnable = false,
                        TestIntervalHour = 4,
                        FailedContinues = 3
                    };
                });
            },
            actionName: $"GetGoldenTestSetting ({assyPN}-{processCheck})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    public async Task<GoldenSampleLimitResponse> GetGoldenTestLimit(
       string assyPN,
       string SN,
       string processCheck)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new GoldenSampleLimitGetRequest
                {
                    AssyPN = assyPN,
                    SerialNumber = SN,
                    ProcessCheck = processCheck
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "api/GoldenSample/GoldenSampleLimit",
                    request);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<GoldenSampleLimitResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Parameters_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $"SELECT TOP 1 * FROM [Parameters_SZ].[dbo].[BOYD_Golden_Units] WHERE Serial = '{SN}' AND Assy_PN = '{assyPN}' AND Process_to_Monitor = '{processCheck}' ORDER BY Date_Time DESC";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        bool isGoldenOK = false;
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            string comment = dt.Rows[0]["Comment"]?.ToString() ?? "";
                            isGoldenOK = comment.IndexOf("OK", StringComparison.OrdinalIgnoreCase) >= 0;
                        }

                        PressureDecayGoldenParameters parameters = null;
                        try
                        {
                            var dbLimit = new Cls_DBMsSQL();
                            dbLimit.Initialize("10.102.4.20", "Golden_Parameters", "readuser", "Boyd@2025");
                            dbLimit.Open();
                            string limitSql = $"SELECT TOP 1 [Min_Pressure_Drop], [Max_Pressure_Drop], [Pressure_Drop_Units] FROM [Golden_Parameters].[dbo].[Pressure_Decay_Test_Limits] WHERE Serial = '{SN}' AND Assy_PN = '{assyPN}' ORDER BY Date_Time DESC";
                            DataTable dtLimit = dbLimit.ExecuteQuery(limitSql);
                            dbLimit.Close();

                            if (dtLimit != null && dtLimit.Rows.Count > 0)
                            {
                                var row = dtLimit.Rows[0];
                                parameters = new PressureDecayGoldenParameters
                                {
                                    PressureDropUSL = row["Max_Pressure_Drop"] != DBNull.Value ? Convert.ToDouble(row["Max_Pressure_Drop"]) : 0,
                                    PressureDropLSL = row["Min_Pressure_Drop"] != DBNull.Value ? Convert.ToDouble(row["Min_Pressure_Drop"]) : 0,
                                    PressureDropUnit = row["Pressure_Drop_Units"] != DBNull.Value ? Convert.ToInt32(row["Pressure_Drop_Units"]) : 0
                                };
                            }
                        }
                        catch { }

                        return new GoldenSampleLimitResponse
                        {
                            Success = isGoldenOK || parameters != null,
                            Code = 200,
                            Message = isGoldenOK ? "Golden Sample Validated (DB Fallback)" : "Golden Sample Not Found (DB Fallback)",
                            AssyPN = assyPN,
                            SerialNumber = SN,
                            ProcessCheck = processCheck,
                            IsGoldenOK = isGoldenOK,
                            LimitParameters = parameters
                        };
                    }
                    catch (Exception ex)
                    {
                        return new GoldenSampleLimitResponse
                        {
                            Success = false,
                            Code = 500,
                            Message = $"DB Fallback Error: {ex.Message}",
                            AssyPN = assyPN,
                            SerialNumber = SN,
                            ProcessCheck = processCheck,
                            IsGoldenOK = false
                        };
                    }
                });
            },
            actionName: $"GetGoldenTestLimit ({SN})",
            isResultSuccessful: res => res != null && res.Success
        );
    }
}
