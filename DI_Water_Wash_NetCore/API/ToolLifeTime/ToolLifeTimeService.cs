using APIchecklistProcess.Model.ToolLifeTime;
using System;
using System.Data;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

public sealed class ToolLifeTimeService
{
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public ToolLifeTimeService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ToolLifeTimeResponse> GetToolLifeTimeSetting(
        string assyPN,
        string processCheck,
        string toolName)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new ToolLifeTimeRequest
                {
                    AssyPN = assyPN,
                    ProcessCheck = processCheck,
                    ToolName = toolName
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "api/ToolLifeTime/SettingToolLifeTime",
                    request);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<ToolLifeTimeResponse>(_jsonOptions);
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
                        string sql = $"SELECT TOP 1 isControl, LifeTimeLimit FROM [Parameters_SZ].[dbo].[Setting_ToolLifeTime] WHERE Assy_PN = '{assyPN}' AND Process = '{processCheck}' AND ToolName = '{toolName}'";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow row = dt.Rows[0];
                            return new ToolLifeTimeResponse
                            {
                                Success = true,
                                Code = 200,
                                Message = "OK (DB Fallback)",
                                AssyPN = assyPN,
                                ProcessCheck = processCheck,
                                ToolName = toolName,
                                isControl = row["isControl"] != DBNull.Value && Convert.ToBoolean(row["isControl"]),
                                LifeTimeLimit = row["LifeTimeLimit"] != DBNull.Value ? Convert.ToInt32(row["LifeTimeLimit"]) : 0
                            };
                        }
                    }
                    catch { }

                    return new ToolLifeTimeResponse
                    {
                        Success = false,
                        Code = 500,
                        Message = "Setting ToolLifeTime not found in DB Fallback",
                        AssyPN = assyPN,
                        ProcessCheck = processCheck,
                        ToolName = toolName,
                        isControl = false,
                        LifeTimeLimit = 0
                    };
                });
            },
            actionName: $"GetToolLifeTimeSetting ({toolName})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    public async Task<ToolUsedTimeResponse> GetToolUsedTime(string serial)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new ToolUsedTimeRequest
                {
                    Serial = serial
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "api/ToolLifeTime/GetToolUsedTime",
                    request);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<ToolUsedTimeResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $"SELECT COUNT(1) AS UsedTime FROM [Production_SZ].[dbo].[ToolLifeTime_Log] WHERE ToolSerial = '{serial}'";
                        DataTable dt = db.ExecuteQuery(sql);
                        db.Close();

                        int count = 0;
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0]["UsedTime"] != DBNull.Value)
                        {
                            count = Convert.ToInt32(dt.Rows[0]["UsedTime"]);
                        }

                        return new ToolUsedTimeResponse
                        {
                            Success = true,
                            Code = 200,
                            Message = "OK (DB Fallback)",
                            Serial = serial,
                            iUsedTime = count
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ToolUsedTimeResponse
                        {
                            Success = false,
                            Code = 500,
                            Message = $"DB Fallback Error: {ex.Message}",
                            Serial = serial,
                            iUsedTime = 0
                        };
                    }
                });
            },
            actionName: $"GetToolUsedTime ({serial})",
            isResultSuccessful: res => res != null && res.Success
        );
    }

    public async Task<ToolLifeUpdateDatabaseResponse> UpdateToolUsedTime(
        string serial,
        string process,
        string stationID,
        string fixtureType,
        string assyPN)
    {
        return await ApiFallbackManager.ExecuteWithFallbackAsync(
            async () =>
            {
                var request = new ToolLifeUpdateDatabaseRequest
                {
                    Serial = serial,
                    Process = process,
                    StationID = stationID,
                    FixtureType = fixtureType,
                    AssyPN = assyPN
                };

                var response = await _httpClient.PostAsJsonAsync(
                    "api/ToolLifeTime/UpdateToolUsedTime",
                    request);

                if (!response.IsSuccessStatusCode) return null;

                return await response.Content.ReadFromJsonAsync<ToolLifeUpdateDatabaseResponse>(_jsonOptions);
            },
            async () =>
            {
                return await Task.Run(() =>
                {
                    try
                    {
                        var db = new Cls_DBMsSQL();
                        db.Initialize("10.102.4.20", "Production_SZ", "readuser", "Boyd@2025");
                        db.Open();
                        string sql = $@"INSERT INTO [Production_SZ].[dbo].[ToolLifeTime_Log] 
                            ([ToolSerial], [Process], [StationID], [FixtureType], [Assy_PN], [Date_Time]) 
                            VALUES ('{serial}', '{process}', '{stationID}', '{fixtureType}', '{assyPN}', GETDATE())";
                        bool success = db.ExecuteNonQuery(sql);
                        db.Close();

                        return new ToolLifeUpdateDatabaseResponse
                        {
                            Success = success,
                            Code = 200,
                            Message = success ? "Updated DB Fallback OK" : "DB Fallback insert failed"
                        };
                    }
                    catch (Exception ex)
                    {
                        return new ToolLifeUpdateDatabaseResponse
                        {
                            Success = false,
                            Code = 500,
                            Message = $"DB Fallback Error: {ex.Message}"
                        };
                    }
                });
            },
            actionName: $"UpdateToolUsedTime ({serial})",
            isResultSuccessful: res => res != null && res.Success
        );
    }
}
