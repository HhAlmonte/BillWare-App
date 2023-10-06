using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public UserService(HttpClient httpClient, LocalStorageHelper localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<bool> DeleteUser(string id)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"User/DeleteUser/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<bool>();
                }
                else
                {
                    throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserAuthResponse> GetCurrentUser()
        {
            try
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
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaginationResult<UserModel>> GetUsersPaged(int pageIndex, int pageSize)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"User/GetUsersPaged?pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<PaginationResult<UserModel>>();
                }
                else
                {
                    throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync("User/UpdateUser", user);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserModel>();
                }
                else
                {
                    throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
