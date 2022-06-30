using System.Collections.ObjectModel;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Stocks.Shared.DTO;
using Syncfusion.Blazor.Charts;

namespace Stocks.Client.Shared;

public partial class StockChart
{
    [Inject] public HttpClient _httpClient { get; set; }
    
    [Parameter] public CompanyDTO Company { get; set; }

    public ObservableCollection<ChartDataDTO> Data { get; set; }
    public ChartDataDTO TodaysData { get; set; }
    protected SfStockChart _stockChart { get; set; }
    public bool DoneLoading { get; set; }

    public async Task LoadData()
    {
        Data = await _httpClient.GetFromJsonAsync<ObservableCollection<ChartDataDTO>>($"api/Tickers/{Company.Ticker}/data");
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
        TodaysData = Data.Single(e => e.Date == Data.Max(d => d.Date));
        DoneLoading = true;
        StateHasChanged();
        _stockChart.Refresh();
        await base.OnInitializedAsync();
    }
}