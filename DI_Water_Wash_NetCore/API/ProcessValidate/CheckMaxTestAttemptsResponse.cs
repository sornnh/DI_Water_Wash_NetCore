namespace APIchecklistProcess.Model.ProcessValidate
{
    public sealed class CheckMaxTestAttemptsResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string AssyPN { get; set; } = string.Empty;
        public string ProcessStep { get; set; } = string.Empty;
        public int? StepNo { get; set; }
        public int TestAttemptsCount { get; set; }
        public int MaxTestAttempts { get; set; }
        public bool IsMaxTestAttempts { get; set; }
    }
}
