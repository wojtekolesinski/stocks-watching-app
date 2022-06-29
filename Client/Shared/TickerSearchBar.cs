using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Stocks.Shared.DTO;
using Syncfusion.Blazor.DropDowns;

namespace Stocks.Client.Shared;

public partial class TickerSearchBar
{
    [Inject] HttpClient _httpClient { get; set; }
    [Parameter] public EventCallback<string> OnSearch { get; set; }

    public string Ticker;

    List<CompanyDTO> LocalData;

    protected override async Task OnInitializedAsync()
    {
        LocalData = await _httpClient.GetFromJsonAsync<List<CompanyDTO>>("api/Tickers");
        await base.OnInitializedAsync();
    }

    private async Task OnFilter(FilteringEventArgs obj)
    {
        Ticker = obj.Text;
        LocalData = await _httpClient.GetFromJsonAsync<List<CompanyDTO>>($"api/Tickers?search={Ticker}");
    }
}