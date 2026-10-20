namespace APIchecklistProcess.Model.ToolLifeTime
{
    public sealed class ToolUsedTimeResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Serial { get; set; } = string.Empty;
        public int iUsedTime { get; set; } = 3000;
    }
}
