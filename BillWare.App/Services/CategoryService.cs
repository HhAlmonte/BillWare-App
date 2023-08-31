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

        public async Task<HttpResponseMessage> CreateCategory(CategoryModel category)
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

        public async Task<HttpResponseMessage> EditCategory(CategoryModel category)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync($"Category/UpdateCategory", category);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponseModel<CategoryModel>> GetCategoriesPaged(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<CategoryModel>>($"Category/GetCategoriesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }

        public async Task<BaseResponseModel<CategoryModel>> GetCategoriesPagedWithSearch(int pageIndex, int pageSize, string search)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<CategoryModel>>($"Category/GetCategoriesPagedWithSearch?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }
    }
}
