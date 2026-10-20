namespace APIchecklistProcess.Model.ToolLifeTime
{
    public sealed class ToolLifeUpdateDatabaseRequest
    {
        public string Serial { get; set; } = string.Empty;
        public string Process { get; set; } = string.Empty; //QD,Oring,.....
        public string StationID { get; set; } = string.Empty;
        public string FixtureType {  get; set; } = string.Empty;
        public string AssyPN { get; set; } = string.Empty;
    }
}
