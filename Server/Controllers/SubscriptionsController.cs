using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stocks.Server.Models;
using Stocks.Server.Services;
using Stocks.Shared.DTO;

namespace Stocks.Server.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class SubscriptionsController : ControllerBase
{
    private readonly ISubscriptionsService _service;
    private readonly UserManager<ApplicationUser> _userManager;

    public SubscriptionsController(ISubscriptionsService service, UserManager<ApplicationUser> userManager)
    {
        _service = service;
        _userManager = userManager;
    }

    [HttpPost]
    public async Task<IActionResult> AddCompanyToSubscriptions(CompanyDTO company)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _service.AddSubscribtion(userId, company);

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetSubscriptions()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Ok(await _service.GetSubscriptions(userId));
    }

    [HttpDelete("{ticker}")]
    public async Task<IActionResult> DeleteSubscription(string ticker)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        await _service.DeleteSubscriptionAsync(userId, ticker);

        return NoContent();
    }
}
