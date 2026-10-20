using System.Text.Json.Serialization;


public sealed class CheckVersionRequest
{
    [JsonPropertyName("appName")]
    public string AppName { get; init; } = string.Empty;

    [JsonPropertyName("currentVersion")]
    public string CurrentVersion { get; init; } = string.Empty;

    [JsonPropertyName("process")]
    public string Process { get; init; } = "ALL";

    [JsonPropertyName("assyPN")]
    public string AssyPN { get; init; } = "ALL";

    [JsonPropertyName("machineName")]
    public string MachineName { get; init; } = "ALL";

    [JsonPropertyName("stationId")]
    public string StationId { get; init; } = "ALL";

    [JsonPropertyName("lineName")]
    public string LineName { get; init; } = "ALL";

    [JsonPropertyName("installedExecutablePath")]
    public string InstalledExecutablePath { get; init; } = string.Empty;
}
