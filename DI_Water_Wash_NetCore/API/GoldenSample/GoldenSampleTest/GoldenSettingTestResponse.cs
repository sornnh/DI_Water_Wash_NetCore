
public sealed class GoldenSettingTestResponse
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;

    public string AssyPN { get; set; } = string.Empty;
    public string ProcessCheck { get; set; } = string.Empty;
    public bool IsEnable { get; set; } = false;
    public int TestIntervalHour { get; set; } = 4;
    public int FailedContinues { get; set; } = 3;
}
