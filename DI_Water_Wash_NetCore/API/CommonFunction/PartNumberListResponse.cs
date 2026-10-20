using System.Collections.Generic;

namespace APIchecklistProcess.Model.CommonFunction
{
    public sealed class PartNumberListResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<PartNumberItem> Data { get; set; } = new();
    }
}
