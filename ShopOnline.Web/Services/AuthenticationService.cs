using System.Net.Http.Json;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Api.Services;
public class AuthenticationService : IAuthenticationService
{
    private readonly HttpClient httpClient;

    public AuthenticationService(HttpClient httpClient)
    {
        this.httpClient=httpClient;
    }

    public async Task<bool> Authenticate(string username, string password)
    {
        var loginDto = new LoginDto();

        loginDto.Username = username;
        loginDto.Password = password;

        var response = await httpClient.PostAsJsonAsync<LoginDto>("api/Login", loginDto);

        if (response.IsSuccessStatusCode)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return false;
            }

            return await response.Content.ReadFromJsonAsync<bool>();

        }
        else
        {
            var message = await response.Content.ReadAsStringAsync();
            throw new Exception($"Http status:{response.StatusCode} Message -{message}");
        }
    }
}
