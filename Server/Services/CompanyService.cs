using System.Globalization;
using System.Net;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Stocks.Server.Data;
using Stocks.Server.Exceptions;
using Stocks.Server.Models;
using Stocks.Shared.DTO;

namespace Stocks.Server.Services;

public class CompanyService : ICompanyService
{
    private readonly ApplicationDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public CompanyService(ApplicationDbContext context, HttpClient httpClient, IConfiguration configuration)
    {
        _context = context;
        _httpClient = httpClient;
        _apiKey = configuration["PolygonToken"];
    }

    public async Task<IEnumerable<Company>> GetCompaniesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<CompanyDTO> GetCompanyAsync(string ticker)
    {
        var url = $"https://api.polygon.io/v3/reference/tickers/{ticker}?apiKey={_apiKey}";

        var companyFromDb = await _context.Companies.SingleOrDefaultAsync(c => c.Ticker == ticker);
        
        if (companyFromDb != null)
        {
            return new CompanyDTO
            {
                Name = companyFromDb.Name,
                Ticker = companyFromDb.Ticker,
                Industry = companyFromDb.Industry,
                Country = companyFromDb.Country,
                Description = companyFromDb.Description,
                Branding = new BrandingDTO
                {
                    IconUrl = companyFromDb.IconUrl,
                    LogoUrl = companyFromDb.LogoUrl
                }
            };
        }

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw response.StatusCode switch
            {
                HttpStatusCode.TooManyRequests => new TooManyRequestsException(),
                HttpStatusCode.NotFound => new NotFoundException($"Ticker {ticker}"),
                _ => new Exception("Something went wrong")
            };
        }

        var companyDto =
            (await JsonSerializer.DeserializeAsync<WrapperDTO<CompanyDTO>>(await response.Content.ReadAsStreamAsync()))
            .Results;
        companyDto.Country = new RegionInfo(companyDto.Country).EnglishName;

        Console.WriteLine("from api");
        await _context.Companies.AddAsync(new Company
        {
            Name = companyDto.Name,
            Ticker = companyDto.Ticker,
            Industry = companyDto.Industry,
            Country = companyDto.Country,
            Description = companyDto.Description,
            IconUrl = companyDto.Branding.IconUrl,
            LogoUrl = companyDto.Branding.LogoUrl
        });

        await _context.SaveChangesAsync();
        
        return companyDto;
    }
}