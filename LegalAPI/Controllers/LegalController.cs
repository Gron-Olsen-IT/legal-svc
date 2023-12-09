using Microsoft.AspNetCore.Mvc;
using LegalAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace LegalAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class LegalController : ControllerBase
{
    private readonly ILogger<LegalController> _logger;
    private readonly ILegalService _legalService;

    public LegalController(ILogger<LegalController> logger, ILegalService legalService)
    {
        _logger = logger;
        _legalService = legalService;
    }

    [Authorize]
    [HttpGet("auctions")]
    //summary: Get a list of auctions
    public async Task<IActionResult> GetAllAuctions()
    {
        try
        {
            var auctions = await _legalService.GetAllAuctions(Request.Headers["Authorization"]!);
            return Ok(auctions);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("GetAllAuctions failed");
        }
    }

    [Authorize]
    [HttpGet("auctions/{auctionId}")]
    //summary: Get a specific auction
    public async Task<IActionResult> GetAuctionById(string auctionId)
    {
        try
        {
            var auction = await _legalService.GetAuctionById(auctionId,Request.Headers["Authorization"]!);
            return Ok(auction);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("GetAuctionById failed");
        }
    }

    [Authorize]
    [HttpGet("users/{userId}")]
    //summary: Get a specific user
    public async Task<IActionResult> GetUserById(string userId)
    {
        try
        {
            _logger.LogInformation("GetUserById: " + userId);
            var user = await _legalService.GetUserById(userId, Request.Headers["Authorization"]!);
            return Ok(user);
        }
        catch (Exception)
        {
            return BadRequest("GetUserById failed");
        }
    }


    [HttpPost("login")]
    //summary: Sign in as a legal user
    public async Task<IActionResult> Login([FromBody] LoginDTO login)
    {
        try
        {
            _logger.LogInformation("Login username: " + login.Email);
            _logger.LogInformation("Login password: " + login.Password);
            string token = await _legalService.Login(login);
            return Ok(token);
        }
        catch (Exception)
        {
            return BadRequest("Login failed");
        }
    }
    
}

public record LoginDTO(string Email, string Password);

