using System.Collections.Generic;

namespace APIchecklistProcess.Model
{
    public sealed class RecordProcessResultRequest
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string AssyPN { get; set; } = string.Empty;
        public string WorkOrder { get; set; } = string.Empty;
        public string ProcessStep { get; set; } = string.Empty;
        public int StepNo { get; set; }
        public string FailCode { get; set; } = "PASS";
        public string? Operator { get; set; }
        public string? Station { get; set; }
        public string? Remark { get; set; }
        public Dictionary<string, object>? ExtraData { get; set; }
    }
}
