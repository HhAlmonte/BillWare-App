using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<BaseResponseModel<Category>> GetCategories(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<Category>>($"Category/GetCategoriesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }
    }
}
