using Stocks.Server.Models;
using Stocks.Shared.DTO;

namespace Stocks.Server.Services;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDTO>> GetCompaniesAsync(string? search);
    Task<CompanyDTO> GetCompanyAsync(string ticker);
    

}