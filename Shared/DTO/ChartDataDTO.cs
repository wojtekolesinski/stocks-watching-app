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
    
    [JsonPropertyName("preMarket")]
    public double PreMarket { get; set; }
    
    [JsonPropertyName("afterHours")]
    public double AfterHours { get; set; }

    [JsonPropertyName("t")]
    public long Timestamp
    {
        set
        {
            Date = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                .AddMilliseconds(value);
        }
    }

    public DateTime Date { get; set; }
}