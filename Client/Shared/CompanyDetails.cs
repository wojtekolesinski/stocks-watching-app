using Microsoft.AspNetCore.Components;
using Stocks.Shared.DTO;

namespace Stocks.Client.Shared;

public partial class CompanyDetails
{
    [Parameter] public CompanyDTO Company { get; set; }
}