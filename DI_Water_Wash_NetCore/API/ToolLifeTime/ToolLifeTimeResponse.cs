namespace APIchecklistProcess.Model.ToolLifeTime
{
    public sealed class ToolLifeTimeResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;

        public string AssyPN { get; set; } = string.Empty;
        public string ToolName { get; set; } = string.Empty; //QD,Oring,.....
        public string ProcessCheck { get; set; } = string.Empty;
        public bool isControl { get; set; } = false;
        public int LifeTimeLimit { get; set; } = 0;
    }
}
