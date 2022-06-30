using Stocks.Shared.DTO;

namespace Stocks.Server.Services;

public interface ISubscriptionsService
{
    Task<object?> AddSubscribtion(string userId, CompanyDTO company);
}