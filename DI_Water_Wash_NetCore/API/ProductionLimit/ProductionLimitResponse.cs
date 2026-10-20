namespace APIchecklistProcess.Model.ProductionLimit
{
    public abstract class ProductionLimitResponse
    {
        public bool Success { get; set; }
        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public string AssyPN { get; set; } = string.Empty;
        public int? StepNo { get; set; }
    }

    public sealed class PressureDecayLimitResponse : ProductionLimitResponse
    {
        public double? DecayPressureFill { get; set; }
        public double? DecayPressureFillTolerance { get; set; }
        public double? DecayDropPressureMax { get; set; }
        public double? DecayStabilize_Time { get; set; }
        public double? DecayTest_Time { get; set; }
        public double? DecayVent_Time { get; set; }

        // Bổ sung thêm các trường dùng cho Pressure Decay 
        public double? MaxHumidity { get; set; }
        public bool? PreDrying { get; set; }
        public int? MaxTestAttempts { get; set; }
        public int? TestToPassIfFail { get; set; }
    }

    public sealed class DIWaterWashLimitResponse : ProductionLimitResponse
    {
        public int? MaxTestAttempts { get; set; }
        public int? TestToPassIfFail { get; set; }
        public int? WashCycles { get; set; }
        public int? PreWashingTime { get; set; }
        public int? WashingTime { get; set; }
        public bool? ReverseWashingFlow { get; set; }
        public int? DIReverseWashingTime { get; set; }
        public bool? ReverseDIFlushingFlow { get; set; }
        public int? DIDryingTime { get; set; }
        public int? ReverseDIDryingTime { get; set; }
        public double? DIFlowRate { get; set; }
        public double? DIFlowTolerance { get; set; }
        public double? DIMaxAirPressure { get; set; }
        public double? DIMinAirPressure { get; set; }
        public double? DIMaxWaterPressure { get; set; }
        public double? DIMinWaterPressure { get; set; }
        public bool? CheckDIHumidity { get; set; }
        public int? DIMaxHumidity { get; set; }
    }
}
