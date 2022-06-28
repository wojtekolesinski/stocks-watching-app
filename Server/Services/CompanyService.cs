using Stocks.Server.Data;
using Stocks.Server.Models;

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

    public async Task<Company> GetCompanyAsync(string ticker)
    {
        Console.WriteLine(_apiKey);
        throw new NotImplementedException();
    }
}