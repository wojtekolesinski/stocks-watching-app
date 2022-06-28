using System.Text.Json.Serialization;

namespace Stocks.Shared.DTO;

public class WrapperDTO<T>
{
    [JsonPropertyName("results")]
    public T Results { get; set; }
}