
public class GoldenSampleLimitResponse
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;
    public string AssyPN { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string ProcessCheck { get; set; } = string.Empty;
    public bool IsGoldenOK { get; set; } = false;
    public object LimitParameters { get; set; }
}

