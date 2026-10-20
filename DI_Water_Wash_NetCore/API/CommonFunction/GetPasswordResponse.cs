namespace APIchecklistProcess.Model.CommonFunction
{
    public sealed class GetPasswordResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Function { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
