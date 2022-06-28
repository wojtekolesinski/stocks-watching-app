using System.Text.Json.Serialization;

namespace Stocks.Shared.DTO;

public class BrandingDTO
{
    [JsonPropertyName("logo_url")]
    public string LogoUrl { get; set; }
    
    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; }
}