namespace APIchecklistProcess.Model.ProcessValidate
{
    public sealed class MoveToMRBResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string AssyPN { get; set; } = string.Empty;
    }
}
