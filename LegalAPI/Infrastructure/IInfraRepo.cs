namespace LegalAPI.InfraRepo;
using LegalAPI.Services;

public interface IInfraRepo
{
    public Task<string> GetFromEndpoint(string endpoint, HttpContent? body, string? token = null);

}