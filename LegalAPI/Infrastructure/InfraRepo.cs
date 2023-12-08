using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;

namespace LegalAPI.InfraRepo;

public class InfraRepo : IInfraRepo
{

    private readonly HttpClient _client;
    private readonly ILogger<InfraRepo> _logger;


    public InfraRepo(ILogger<InfraRepo> logger)
    {
        _logger = logger;
        _client = new HttpClient()
        {
            BaseAddress = new Uri("http://nginx:4000/")
        };
    }

    public async Task<string> GetFromEndpoint(string endpoint, HttpContent? body, string? token = "")
    {
        _logger.LogInformation("GetFromEndpoint: " + endpoint);
        try
        {
            if (body != null){
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", token);
                var PostResponse = await _client.PostAsync(endpoint, body);
                var PostContent = await PostResponse.Content.ReadAsStringAsync();
                _logger.LogInformation("PostContent: " + PostContent);
                return PostContent;
            }
            //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", token);
            _client.DefaultRequestHeaders.Add("Authorization", token);
            var GetResponse = await _client.GetAsync(endpoint);
            
            string HeaderWWW = GetResponse.Headers.WwwAuthenticate.ToString();
            _logger.LogInformation("HeaderWWW: " + HeaderWWW);
            
            if (HeaderWWW.Contains("Bearer error")){
                _logger.LogError("GetResponse: " + GetResponse.Headers);
                throw new Exception("Unauthorized - Invalid token");
            }
            return GetResponse.Content.ReadAsStringAsync().Result;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw new Exception("Unauthorized - Invalid token");
        }
    }

}