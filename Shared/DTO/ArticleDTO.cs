using System.Text.Json.Serialization;

namespace Stocks.Shared.DTO;

public class ArticleDTO
{
    [JsonPropertyName("title")]
    public string Title { get; set; }
    
    [JsonPropertyName("article_url")]
    public string Url { get; set; }
    
    [JsonPropertyName("author")]
    public string Author { get; set; }
    
    [JsonPropertyName("description")]
    public string Description { get; set; }
    
    [JsonPropertyName("published_utc")]
    public DateTime PublishedUtc { get; set; }
}