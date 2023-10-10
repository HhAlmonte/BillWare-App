using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class UserService : BaseCrudService<UserModel>, IUserService
    {
        private readonly LocalStorageHelper _localStorageService;
        private readonly HttpClient _httpClient;

        public UserService(HttpClient http,
                           LocalStorageHelper localStorageService)
            : base(http, localStorageService, "User")
        {
            _localStorageService = localStorageService;
            _httpClient = http;
        }

        public async Task<UserAuthResponse> GetCurrentUser()
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"User/GetCurrentUser");

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserAuthResponse>();
            }
            else
            {
                throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
            }
        }
    }
}
