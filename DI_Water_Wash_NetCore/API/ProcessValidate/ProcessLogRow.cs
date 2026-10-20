using System;

public sealed class ProcessLogRow
{
    public string Serial { get; set; } = string.Empty;
    public string AssyPN { get; set; } = string.Empty;
    public string ProcessStep { get; set; } = string.Empty;
    public int StepNo { get; set; }
    public string FailCode { get; set; } = string.Empty;
    public DateTime DateTime { get; set; }
}

