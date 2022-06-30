using Newtonsoft.Json;
using NuGet.Protocol;
using Stocks.Server.Models;
using Stocks.Shared.DTO;

namespace Stocks.Server.Extensions;

public static class DtoMapper
{

    public static CompanyDTO ToDto(this Company company, string apiKey)
    {
        return new CompanyDTO
        {
            Name = company.Name,
            Ticker = company.Ticker,
            Industry = company.Industry,
            Country = company.Country,
            Description = company.Description,
            Branding = new BrandingDTO
            {
                IconUrl = company.IconUrl + "?apiKey=" + apiKey,
                LogoUrl = company.LogoUrl + "?apiKey=" + apiKey
            }
        };
    }

    public static Company toEntity(this CompanyDTO companyDto)
    {
        Console.WriteLine(companyDto.ToJson(Formatting.Indented));
        return new Company
        {
            Name = companyDto.Name,
            Ticker = companyDto.Ticker,
            Industry = companyDto.Industry,
            Country = companyDto.Country,
            Description = companyDto.Description,
            IconUrl = companyDto.Branding.IconUrl,
            LogoUrl = companyDto.Branding.LogoUrl
        };
    }

    public static ChartDataDTO toDto(this StockPriceData sp)
    {
        return new ChartDataDTO
        {
            Close = sp.Close,
            Open = sp.Open,
            High = sp.High,
            Low = sp.Low,
            Volume = sp.Volume,
            PreMarket = sp.PreMarket ?? 0,
            AfterHours = sp.AfterHours ?? 0,
            Date = sp.Date,
        };
    }

    public static StockPriceData toEntity(this ChartDataDTO chartDataDto, Company company)
    {
        return new StockPriceData
        {
            Close = chartDataDto.Close,
            Open = chartDataDto.Open,
            High = chartDataDto.High,
            Low = chartDataDto.Low,
            Volume = chartDataDto.Volume,
            PreMarket = chartDataDto.PreMarket,
            AfterHours = chartDataDto.AfterHours,
            Date = chartDataDto.Date,
            Company = company
        };
    }
}