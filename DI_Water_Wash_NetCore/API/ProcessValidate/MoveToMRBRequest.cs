using System;

namespace APIchecklistProcess.Model.ProcessValidate
{
    public sealed class MoveToMRBRequest
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string AssyPN { get; set; } = string.Empty;
        public string Station { get; set; } = string.Empty;
        public string FailCode { get; set; } = string.Empty;
        public string MrbCodes { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty;
        public DateTime? DateTime { get; set; }
    }
}
