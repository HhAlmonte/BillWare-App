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

        public async Task<HttpResponseMessage> CreateCategory(Category category)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("Category/CreateCategory", category);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> DeleteCategory(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"Category/DeleteCategory/{id}");
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<Category> EditCategory(Category category)
        {
            var response = await _httpClient.PutAsJsonAsync($"Category/UpdateCategory", category);

            try
            {
                var categoryResponse = await response.Content.ReadFromJsonAsync<Category>();

                return categoryResponse;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponseModel<Category>> GetCategories(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<Category>>($"Category/GetCategoriesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }

        public async Task<BaseResponseModel<Category>> GetCategoryWithSearch(string search, int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<Category>>($"Category/GetCategoryWithSearch?search={search}&pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }
    }
}
