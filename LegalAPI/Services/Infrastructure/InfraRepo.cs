using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Forms;

namespace LegalAPI.Services;

public class InfraRepo : IInfraRepo
{
    private readonly string INFRA_CONN;
    private readonly HttpClient _client;
    private readonly ILogger<InfraRepo> _logger;


    public InfraRepo(ILogger<InfraRepo> logger, IConfiguration configuration)
    {
        _logger = logger;
        try{
            INFRA_CONN = configuration["INFRA_CONN"]!;

        }catch(Exception e){
            throw new Exception("INFRA_CONN not set: " + e.Message);
        }
        _client = new HttpClient()
        {
            BaseAddress = new Uri(INFRA_CONN)
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