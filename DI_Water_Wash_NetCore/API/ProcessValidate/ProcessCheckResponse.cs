
using System;
using System.Collections.Generic;

public sealed class ProcessCheckResponse
{
    public bool Success { get; set; }
    public int Code { get; set; }
    public string Message { get; set; } = string.Empty;

    public string AssyPN { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string WorkOrder { get; set; } = string.Empty;
    public string ProcessCheck { get; set; } = string.Empty;

    public string[] ProcessList { get; set; } = Array.Empty<string>();
    public List<ProcessResultItem> ProcessResults { get; set; } = new List<ProcessResultItem>();
}
