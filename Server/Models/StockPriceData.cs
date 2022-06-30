namespace Stocks.Server.Models;

public class StockPriceData
{
    public int Id { get; set; }
    public double Close { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Volume { get; set; }
    public double? PreMarket { get; set; }
    public double? AfterHours { get; set; }
    public DateTime Date { get; set; }
    public bool HasDailyData { get; set; }

    public virtual Company Company { get; set; }
}