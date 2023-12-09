using LegalAPI.Controllers;

namespace LegalAPI.Services;
public interface ILegalService
{
    public Task<string> GetAllAuctions(string token);
    public Task<string> GetAuctionById(string auctionId, string token);
    public Task<string> GetUserById(string userId, string token);
    public Task<string> Login(LoginDTO login);

}