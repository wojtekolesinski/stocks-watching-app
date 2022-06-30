using Microsoft.EntityFrameworkCore;
using Stocks.Server.Data;
using Stocks.Server.Exceptions;
using Stocks.Server.Extensions;
using Stocks.Shared.DTO;

namespace Stocks.Server.Services;

public class SubscriptionsService : ISubscriptionsService
{
    private readonly ApplicationDbContext _context;
    private readonly string _apiKey;
    
    public SubscriptionsService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _apiKey = configuration["PolygonToken"];
    }
    
    public async Task AddSubscribtion(string userId, CompanyDTO company)
    {
        var userFromDb = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId);
        var companyFromDb = await _context.Companies.SingleOrDefaultAsync(c => c.Ticker == company.Ticker);

        if (userFromDb == null | companyFromDb == null)
            throw new BadRequestException();
        
        if (userFromDb.Companies.Contains(companyFromDb))
            return;
        
        userFromDb.Companies.Add(companyFromDb);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<CompanyDTO>> GetSubscriptions(string userId)
    {
        var userFromDb = await _context.Users
            .Include(u => u.Companies)
            .SingleOrDefaultAsync(u => u.Id == userId);
        
        if (userFromDb == null)
            throw new BadRequestException();

        return userFromDb.Companies.Select(c => c.ToDto(_apiKey));
    }
}