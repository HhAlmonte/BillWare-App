using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<LoginResponse> LoginAsync(LoginModel request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Account/login", request);

                var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();

                return loginResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<HttpResponseMessage> RegisterAsync(RegistrationModel request)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/register", request);

            return response;
        }
    }
}
