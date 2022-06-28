using Stocks.Server.Models;

namespace Stocks.Server.Services;

public interface ICompanyService
{
    Task<IEnumerable<Company>> GetCompaniesAsync();
    Task<Company> GetCompanyAsync(string ticker);
    

}