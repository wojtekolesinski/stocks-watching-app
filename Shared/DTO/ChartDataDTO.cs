using System.Text.Json.Serialization;

namespace Stocks.Shared.DTO;

public class ChartDataDTO
{
    [JsonPropertyName("c")]
    public double Close { get; set; }
    
    [JsonPropertyName("o")]
    public double Open { get; set; }
    
    [JsonPropertyName("h")]
    public double High { get; set; }
    
    [JsonPropertyName("l")]
    public double Low { get; set; }
    
    [JsonPropertyName("v")]
    public double Volume { get; set; }
    
    [JsonPropertyName("t")]
    public int Timestamp { get; set; }
    
    public DateTime Date { get; set; }
}