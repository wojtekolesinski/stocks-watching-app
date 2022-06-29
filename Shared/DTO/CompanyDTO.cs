using System.Text.Json.Serialization;

namespace Stocks.Shared.DTO;

public class CompanyDTO
{
    [JsonPropertyName("name")] 
    public string Name { get; set; }
    
    [JsonPropertyName("ticker")] 
    public string Ticker { get; set; }
    
    [JsonPropertyName("sic_description")]
    public string Industry { get; set; }
    
    [JsonPropertyName("locale")]
    public string Country { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("branding")] 
    public BrandingDTO? Branding
    {
        get { return _branding ?? new BrandingDTO(); }
        set { _branding = value; }
    }

    [JsonIgnore]
    private BrandingDTO? _branding;
}