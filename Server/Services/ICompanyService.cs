using Stocks.Server.Models;
using Stocks.Shared.DTO;

namespace Stocks.Server.Services;

public interface ICompanyService
{
    Task<IEnumerable<Company>> GetCompaniesAsync();
    Task<CompanyDTO> GetCompanyAsync(string ticker);
    

}