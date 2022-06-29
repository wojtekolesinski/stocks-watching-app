using Microsoft.AspNetCore.Components;
using Stocks.Shared.DTO;

namespace Stocks.Client.Shared;

public partial class StockChart
{
    [Parameter] public List<ChartDataDTO> Data { get; set; }
}