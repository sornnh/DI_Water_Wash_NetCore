using System.Text.Json.Serialization;


public sealed class CheckVersionResponse
{
    [JsonPropertyName("hasConfiguration")]
    public bool HasConfiguration { get; init; }

    [JsonPropertyName("updateRequired")]
    public bool UpdateRequired { get; init; }

    [JsonPropertyName("reason")]
    public string Reason { get; init; } = string.Empty;

    [JsonPropertyName("configId")]
    public int? ConfigId { get; init; }

    [JsonPropertyName("appName")]
    public string AppName { get; init; } = string.Empty;

    [JsonPropertyName("currentVersion")]
    public string CurrentVersion { get; init; } = string.Empty;

    [JsonPropertyName("targetVersion")]
    public string TargetVersion { get; init; } = string.Empty;

    [JsonPropertyName("isMandatory")]
    public bool IsMandatory { get; init; }

    [JsonPropertyName("message")]
    public string Message { get; init; } = string.Empty;
}
