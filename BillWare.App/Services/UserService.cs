using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResponseModel<UserModel>> GetUsersPaged(int pageIndex, int pageSize)
        {
            var users = await  _httpClient.GetFromJsonAsync<BaseResponseModel<UserModel>>($"User/GetUsersPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return users;
        }

        public async Task<UserModel> UpdateUser(UserModel user)
        {
            var response = await _httpClient.PutAsJsonAsync("User/UpdateUser", user);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<UserModel>();
            }

            return null;
        }
    }
}
