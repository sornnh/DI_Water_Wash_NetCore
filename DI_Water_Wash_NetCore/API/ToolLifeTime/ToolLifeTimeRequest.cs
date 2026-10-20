namespace APIchecklistProcess.Model.ToolLifeTime
{
    public sealed class ToolLifeTimeRequest
    {
        public string AssyPN { get; set; } = string.Empty;
        public string ToolName { get; set; } = string.Empty; //QD,Oring,.....
        public string ProcessCheck { get; set; } = string.Empty;
    }
}
