namespace LegalAPI.Services;

using System.Text;
using System.Text.Json;
using LegalAPI.Controllers;

public class LegalService : ILegalService
{

    private readonly ILogger<LegalService> _logger;
    private readonly IInfraRepo _infraRepo;

    public LegalService(IInfraRepo infraRepo, ILogger<LegalService> logger)
    {
        _infraRepo = infraRepo;
        _logger = logger;
    }

    public async Task<string> GetAllAuctions(string token){
        try{
            var auctions = await _infraRepo.GetFromEndpoint("auctions", null, token);
            return auctions;
        }
        catch (Exception e){
            _logger.LogError("GetAllAuctions: " + e);
            throw;
        }
    }
    public async Task<string> GetAuctionById(string auctionId, string token){
        var auction = await _infraRepo.GetFromEndpoint($"auctions/{auctionId}", null, token);
        return auction;
    }
    public async Task<string> GetUserById(string userId, string token){
        try
        {
            var user = await _infraRepo.GetFromEndpoint($"users/{userId}", null, token);
            _logger.LogInformation("GetUserById: " + user);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine("Something went wrong in getting user by id" + e);
            throw;
        }
    }
    public async Task<string> Login(LoginDTO login){
        try {
            var body = new StringContent(JsonSerializer.Serialize(login), Encoding.UTF8, "application/json");
            var JWT_token = await _infraRepo.GetFromEndpoint($"auth/login", body);
            _logger.LogInformation("JWT_token: " + JWT_token);
            return JWT_token;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
}