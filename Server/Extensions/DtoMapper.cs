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
}