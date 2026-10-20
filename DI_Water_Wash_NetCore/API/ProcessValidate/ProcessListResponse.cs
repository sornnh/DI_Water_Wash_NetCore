namespace APIchecklistProcess.Model.ProcessValidate
{
    public sealed class ProcessListResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public List<string> ProcessList { get; set; } = new List<string>();
    }
}