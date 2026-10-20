namespace APIchecklistProcess.Model.CommonFunction
{
    public sealed class GetEnableItemResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        
        public bool Enable { get; set; }
    }
}
