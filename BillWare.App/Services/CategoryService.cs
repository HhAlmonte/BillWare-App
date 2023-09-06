using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageService _localStorageService;

        public CategoryService(HttpClient httpClient, LocalStorageService localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<CategoryModel> CreateCategory(CategoryModel category)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("Category/CreateCategory", category);

                if(response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CategoryModel>();
                    return result;
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

        public async Task<CategoryModel> EditCategory(CategoryModel category)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"Category/UpdateCategory", category);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<CategoryModel>();
                    return result;
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

        public async Task<bool> DeleteCategory(int id)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"Category/DeleteCategory/{id}");

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

        public async Task<BaseResponseModel<CategoryModel>> GetCategoriesPaged(int pageIndex, int pageSize)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"Category/GetCategoriesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BaseResponseModel<CategoryModel>>();
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

        public async Task<BaseResponseModel<CategoryModel>> GetCategoriesPagedWithSearch(int pageIndex, int pageSize, string search)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"Category/GetCategoriesPagedWithSearch?pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<BaseResponseModel<CategoryModel>>();
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
