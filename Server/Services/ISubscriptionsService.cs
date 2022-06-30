using Stocks.Shared.DTO;

namespace Stocks.Server.Services;

public interface ISubscriptionsService
{
    Task AddSubscribtion(string userId, CompanyDTO company);
    Task<IEnumerable<CompanyDTO>> GetSubscriptions(string userId);
    Task DeleteSubscriptionAsync(string userId, string ticker);
}