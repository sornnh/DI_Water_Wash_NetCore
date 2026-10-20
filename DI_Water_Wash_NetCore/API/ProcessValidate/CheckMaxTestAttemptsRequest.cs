namespace APIchecklistProcess.Model.ProcessValidate
{
    public sealed class CheckMaxTestAttemptsRequest
    {
        public string SerialNumber { get; set; } = string.Empty;
        public string AssyPN { get; set; } = string.Empty;
        public string ProcessStep { get; set; } = string.Empty;
        public int? StepNo { get; set; }
        public int MaxTestAttempts { get; set; } = 3;
    }
}
