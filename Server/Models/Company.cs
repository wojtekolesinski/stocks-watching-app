using System.ComponentModel.DataAnnotations;

namespace Stocks.Server.Models;

public class Company
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Ticker { get; set; }
    public string? Industry { get; set; }
    public string Country { get; set; }
    public string? Description { get; set; }
    public string? LogoUrl { get; set; }
    public string? IconUrl { get; set; }
    public bool HasDetails { get; set; }
    
    public virtual ICollection<ApplicationUser> Users { get; set; }
    public virtual ICollection<Article> Articles { get; set; }
    public virtual ICollection<StockPriceData> StockPrices { get; set; }
}