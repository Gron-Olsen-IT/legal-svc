using Microsoft.AspNetCore.Mvc;
using LegalAPI.Services;
using Microsoft.AspNetCore.Authorization;

namespace LegalAPI.Controllers;

[Authorize]
[ApiController]
[Route("legal")]
public class LegalController : ControllerBase
{
    private readonly ILogger<LegalController> _logger;
    private readonly ILegalService _legalService;

    public LegalController(ILogger<LegalController> logger, ILegalService legalService)
    {
        _logger = logger;
        _legalService = legalService;
    }

    /// <summary>
    /// Get all auctions
    /// </summary>
    /// <response code="200">Returns a list containing all auctions</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <returns></returns>
    [HttpGet("auctions")]
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

    /// <summary>
    /// Get a specific auction by id
    /// </summary>
    /// <response code="200">Returns an auction</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <returns></returns>
    [HttpGet("auctions/{auctionId}")]
    public async Task<IActionResult> GetAuctionById(string auctionId)
    {
        try
        {
            var auction = await _legalService.GetAuctionById(auctionId, Request.Headers["Authorization"]!);
            return Ok(auction);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest("GetAuctionById failed");
        }
    }

    /// <summary>
    /// Get a specific user by id
    /// </summary>
    /// <response code="200">Returns a user</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <returns></returns>
    [HttpGet("users/{userId}")]
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

    /// <summary>
    /// Log in using a username and password
    /// </summary>
    /// <response code="200">Returns a token if user is authenticated</response>
    /// <response code="401">If the user is not authenticated</response>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("login")]
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

