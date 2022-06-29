using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stocks.Server.Services;

namespace Stocks.Server.Controllers;

// [Authorize]
[Route("api/[controller]")]
[ApiController]
public class TickersController : ControllerBase
{
    private ICompanyService _service;

    public TickersController(ICompanyService service)
    {
        _service = service;
    }

    [HttpGet("{ticker}")]
    public async Task<IActionResult> GetCompany(string ticker)
    {
        return Ok(await _service.GetCompanyAsync(ticker));
    }

    [HttpGet]
    public async Task<IActionResult> GetCompanies([FromQuery] string? search)
    {
        if (search == null)
        {
            search = "a";
        }

        return Ok(await _service.GetCompaniesAsync(search));
    }
}