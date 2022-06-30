using Microsoft.EntityFrameworkCore;
using Stocks.Server.Data;
using Stocks.Shared.DTO;

namespace Stocks.Server.Services;

public class SubscriptionsService : ISubscriptionsService
{
    private readonly ApplicationDbContext _context;
    
    public SubscriptionsService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<object?> AddSubscribtion(string userId, CompanyDTO company)
    {
        var userFromDb = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        var companyFromDb = await _context.Companies.SingleOrDefaultAsync(c => c.Ticker == company.Ticker);

        // if (userFromDb == null | companyFromDb == null)
            // throw 
        
        if (userFromDb.Companies.Contains(companyFromDb))
            return null;
        
        userFromDb.Companies.Add(companyFromDb);
        await _context.SaveChangesAsync();
        return null;
    }
}