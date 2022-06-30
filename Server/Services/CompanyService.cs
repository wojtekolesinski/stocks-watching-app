using System.Net;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Protocol;
using Stocks.Server.Data;
using Stocks.Server.Exceptions;
using Stocks.Server.Extensions;
using Stocks.Server.Models;
using Stocks.Shared.DTO;
using JsonSerializer = System.Text.Json.JsonSerializer;

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

    public async Task<IEnumerable<CompanyDTO>> GetCompaniesAsync(string? search)
    {

        var companiesFromDb = await _context.Companies
                                            .Where(c => c.Ticker.ToLower().StartsWith(search.ToLower()))
                                            .ToListAsync();
        if (companiesFromDb.Count != 0)
        {
            return companiesFromDb.Select(c => c.ToDto(""));
        }
        
        var url = $"https://api.polygon.io/v3/reference/tickers?search={search}&active=true&sort=ticker&order=asc&limit=100&apiKey={_apiKey}";
        
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            throw response.StatusCode switch
            {
                HttpStatusCode.TooManyRequests => new TooManyRequestsException(),
                HttpStatusCode.NotFound => new NotFoundException(""),
                _ => new Exception("Something went wrong")
            };
        }
        
        var companies = (
            await JsonSerializer
                .DeserializeAsync<WrapperDTO<ICollection<CompanyDTO>>>(await response.Content.ReadAsStreamAsync())
                ).Results
                .Where(c => c.Ticker.ToLower().StartsWith(search.ToLower()))
                .ToList();
        
        foreach (var companyDto in companies)
        {
            await _context.Companies.AddAsync(companyDto.toEntity());
        }

        await _context.SaveChangesAsync();

        return companies;
    }

    public async Task<CompanyDTO> GetCompanyAsync(string ticker)
    {
        var companyFromDb = await _context.Companies.FirstOrDefaultAsync(c => c.Ticker == ticker);
        
        if (companyFromDb != null & companyFromDb.HasDetails)
        {
            return companyFromDb.ToDto(_apiKey);
        }

        var url = $"https://api.polygon.io/v3/reference/tickers/{ticker}?apiKey={_apiKey}";
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

        if (companyFromDb != null)
        {
            companyFromDb.Country = companyDto.Country;
            companyFromDb.Industry = companyDto.Industry;
            companyFromDb.Description = companyDto.Description;
            companyFromDb.IconUrl = companyDto.Branding.IconUrl;
            companyFromDb.LogoUrl = companyDto.Branding.LogoUrl;
            companyFromDb.HasDetails = true;
            _context.Companies.Update(companyFromDb);
        }
        else
        {
            await _context.Companies.AddAsync(companyDto.toEntity());
        }
        
        await _context.SaveChangesAsync();
        
        return companyFromDb.ToDto(_apiKey);
    }

    public async Task<IEnumerable<ChartDataDTO>> GetCompanyPriceDataAsync(string ticker)
    {
        var stockPricesFromDb = await _context.StockPrices.Where(s => s.Company.Ticker == ticker).ToListAsync();
        await GetCompanyAsync(ticker);
        var companyFromDb = await _context.Companies.FirstAsync(c => c.Ticker == ticker);
        DateTime? maxSavedDate = null;
        if (stockPricesFromDb.Count > 0)
        {
            maxSavedDate = stockPricesFromDb.Max(sp => sp.Date).Date;
            if (maxSavedDate == DateTime.Today.AddDays(-1))
            {
                await GetCompanyPriceDailyAsync(ticker);
                return _context.StockPrices.Where(s => s.Company.Ticker == ticker).Select(sp => sp.toDto());
            }
            maxSavedDate = maxSavedDate.Value.AddDays(1);
        }

        maxSavedDate ??= DateTime.UtcNow.AddYears(-1);

        var url = $"https://api.polygon.io/v2/aggs/ticker/{ticker}/range/1/day/{maxSavedDate.Value.ToString("yyyy-MM-dd")}/{DateTime.UtcNow.ToString("yyyy-MM-dd")}?adjusted=true&sort=asc&limit=365&apiKey={_apiKey}";
        
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
        
        var priceData = 
            (await JsonSerializer.DeserializeAsync<WrapperDTO<ICollection<ChartDataDTO>>>(await response.Content.ReadAsStreamAsync()))
            .Results;
        
        foreach (var chartDataDto in priceData)
        {
            await _context.StockPrices.AddAsync(chartDataDto.toEntity(companyFromDb));
        }

        
        await _context.SaveChangesAsync();
        if (stockPricesFromDb.Count > 0)
        {
            priceData = priceData.Union(stockPricesFromDb.Select(sp => sp.toDto())).ToList();
        }

        var todaysData = priceData.First(e => e.Date == priceData.Max(d => d.Date));
        var todaysDataUpdated = await GetCompanyPriceDailyAsync(ticker);
        todaysData.PreMarket = todaysDataUpdated.PreMarket; 
        todaysData.AfterHours = todaysDataUpdated.AfterHours; 

        return priceData;
    }

    public async Task<ChartDataDTO> GetCompanyPriceDailyAsync(string ticker)
    {
        var stockPriceFromDb = await _context.StockPrices.FirstAsync(sp => sp.Company.Ticker == ticker & sp.Date.Date == DateTime.Today.AddDays(-1).Date);

        if (stockPriceFromDb.HasDailyData)
        {
            return stockPriceFromDb.toDto();
        }
        
        var url = $"https://api.polygon.io/v1/open-close/{ticker}/{DateTime.UtcNow.AddDays(-1).ToString("yyyy-MM-dd")}?adjusted=true&apiKey={_apiKey}";
        
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
        
        var priceData = await JsonSerializer.DeserializeAsync<ChartDataDTO>(await response.Content.ReadAsStreamAsync());
        
        stockPriceFromDb.PreMarket = priceData.PreMarket;
        stockPriceFromDb.AfterHours = priceData.AfterHours;
        await _context.SaveChangesAsync();

        return stockPriceFromDb.toDto();
    }

    public async Task<IEnumerable<ArticleDTO>> GetCompanyNewsAsync(string ticker)
    {
        var url = $"https://api.polygon.io/v2/reference/news?ticker={ticker}&limit=5&apiKey={_apiKey}";
        
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
        
        var articles =
            (await JsonSerializer.DeserializeAsync<WrapperDTO<ICollection<ArticleDTO>>>(
                await response.Content.ReadAsStreamAsync())).Results;

        return articles;
    }
}