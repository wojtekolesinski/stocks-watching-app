using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Stocks.Shared.DTO;

namespace Stocks.Client.Shared;

public partial class NewsList
{
    [Inject] private HttpClient _httpClient { get; set; }
    
    [Parameter] public CompanyDTO Company { get; set; }

    public List<ArticleDTO> Articles { get; set; }
    public bool DataReady { get; set; }

    protected override async Task OnInitializedAsync()
    {
        Articles = await _httpClient.GetFromJsonAsync<List<ArticleDTO>>($"api/Tickers/{Company.Ticker}/news");
        DataReady = true;
        await base.OnInitializedAsync();
    }
}