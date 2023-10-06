using BillWare.App.Common;
using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;

namespace BillWare.App.Services
{
    public class JWTAuthenticationStateProviderService : AuthenticationStateProvider, IJWTAuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public JWTAuthenticationStateProviderService(HttpClient httpClient, LocalStorageHelper localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            if (string.IsNullOrEmpty(token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            return AuthenticationStateProviderHelper.BuildAuthenticationState(token);
        }

        public async Task<BaseResponse<LoginResponse>> LoginAsync(LoginModel loginModel)
        {
            var request = await _httpClient.PostAsJsonAsync("Account/login", loginModel);

            if (!request.IsSuccessStatusCode)
            {
                var errorResponse = await request.Content.ReadFromJsonAsync<BaseResponse<LoginResponse>>();

                return BaseResponse<LoginResponse>.BuildErrorResponse(errorResponse!);
            }

            var response = await request.Content.ReadFromJsonAsync<LoginResponse>();

            await _localStorageService.SetItem(Configuration.TOKEN, response!.Token);

            var authState = AuthenticationStateProviderHelper.BuildAuthenticationState(response.Token);

            NotifyAuthenticationStateChanged(Task.FromResult(authState));

            return BaseResponse<LoginResponse>.BuildSuccessResponse(response);
        }

        public async Task<HttpResponseMessage> RegisterAsync(RegistrationModel request)
        {
            var response = await _httpClient.PostAsJsonAsync("Account/register", request);

            return response;
        }

        public async Task LogOut()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            await _localStorageService.RemoveItem(Configuration.TOKEN);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()))));
        }
    }
}
