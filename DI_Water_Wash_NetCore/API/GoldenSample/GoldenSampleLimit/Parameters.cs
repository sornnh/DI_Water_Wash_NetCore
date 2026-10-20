
using System.Collections.Generic;
using System.Text.Json.Serialization;

public class PressureDecayGoldenParameters
{
    [JsonPropertyName("pressureDropUSL")]
    public double PressureDropUSL { get; set; } // Giới hạn sụt áp max
    [JsonPropertyName("pressureDropLSL")]
    public double PressureDropLSL { get; set; } // Áp suất nạp tối thiểu
    [JsonPropertyName("pressureDropUnit")]
    public int PressureDropUnit { get; set; } = 0; //0:Pa,1:psi
}
public class HeliumLeakGoldenParameters
{
    [JsonPropertyName("leakRateUSL")]
    public double LeakRateUSL { get; set; }  // Giới hạn tỷ lệ rò rỉ (e.g. 1.0e-5)
    [JsonPropertyName("leakRateLSL")]
    public double LeakRateLSL { get; set; }  // Giới hạn tỷ lệ rò rỉ (e.g. 1.0e-5)
}
public class FlatnessGoldenParameters
{
    public List<FlatnessLimit> FlatnessLimits { get; set; } = new List<FlatnessLimit>();
}
public class FlatnessLimit
{
    [JsonPropertyName("usl")]
    public double USL { get; set; } = 0;
    [JsonPropertyName("lsl")]
    public double LSL { get; set; } = 0;
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; } = false;
}
public class FlowTestGoldenParameters
{
    public List<FlowTestLimit> FlowTestLimits { get; set; } = new List<FlowTestLimit>();
}
public class FlowTestLimit {
    public double PressureDropUSL { get; set; }
    public double PressureDropLSL { get; set; }
    public bool IsActive { get; set; }
}

