namespace APIchecklistProcess.Model.CommonFunction
{
    public sealed class GetSNVerifyResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;

        public int SN_Length { get; set; }
        public string PN_Ctl_String { get; set; } = string.Empty;
        public int Ctl_Str_Length { get; set; }
        public int Ctl_Str_Offset { get; set; }
        public string Customer_Name { get; set; } = string.Empty;
        public int Serial_Length { get; set; }
        public int SN_Offset { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool Verify_SN_in_WO { get; set; }
        public bool Verify_Linked_Part { get; set; }
    }
}
