using System.Collections.Generic;

namespace APIchecklistProcess.Model
{
    public sealed class RecordProcessResultResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> SavedTables { get; set; } = new List<string>();
        public string SerialNumber { get; set; } = string.Empty;
        public string ProcessStep { get; set; } = string.Empty;
    }
}
