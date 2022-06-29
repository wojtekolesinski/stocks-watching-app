using System.Net;
using Microsoft.EntityFrameworkCore;
using Stocks.Server.Data;
using Stocks.Server.Exceptions;
using Stocks.Server.Extensions;
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
        var url = $"https://api.polygon.io/v3/reference/tickers/{ticker}?apiKey={_apiKey}";

        var companyFromDb = await _context.Companies.SingleOrDefaultAsync(c => c.Ticker == ticker);
        
        if (companyFromDb != null & companyFromDb.HasDetails)
        {
            return companyFromDb.ToDto(_apiKey);
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
}