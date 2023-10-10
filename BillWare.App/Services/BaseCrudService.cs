using BillWare.App.Common;
using BillWare.App.Intefaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class BaseCrudService<T> : IBaseCrudService<T> where T : BaseModel
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;
        private readonly string _controllerName;

        public BaseCrudService(HttpClient http, LocalStorageHelper localStorageService, string controllerName)
        {
            _httpClient = http;
            _localStorageService = localStorageService;
            _controllerName = controllerName;
        }

        public async Task<BaseResponse<T>> CreateAsync(T entity)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = await _httpClient.PostAsJsonAsync($"{_controllerName}/Create", entity);

                if (!request.IsSuccessStatusCode)
                {
                    var errorResponse = await request.Content.ReadFromJsonAsync<BaseResponse<T>>();

                    return BaseResponse<T>.BuildErrorResponse(errorResponse!);
                }

                var response = await request.Content.ReadFromJsonAsync<T>();

                return BaseResponse<T>.BuildSuccessResponse(response!);
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<T>> DeleteAsync(object identity)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"{_controllerName}/Delete/{identity}");

                if (!response.IsSuccessStatusCode)
                {
                    var errorResponse = await response.Content.ReadFromJsonAsync<BaseResponse<T>>();

                    return BaseResponse<T>.BuildErrorResponse(errorResponse!);
                }

                var result = await response.Content.ReadFromJsonAsync<bool>();

                return new BaseResponse<T>
                {
                    IsSuccessFull = result!
                };
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }

        public async Task<BaseResponse<PaginationResult<T>>> GetEntitiesPagedAsync(int pageIndex, int pageSize)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"{_controllerName}/GetListPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            if (!request.IsSuccessStatusCode)
            {
                var errorResponse = await request.Content.ReadFromJsonAsync<BaseResponse<PaginationResult<T>>>();

                return BaseResponse<PaginationResult<T>>.BuildErrorResponse(errorResponse!);
            }

            var response = await request.Content.ReadFromJsonAsync<PaginationResult<T>>();

            return BaseResponse<PaginationResult<T>>.BuildSuccessResponse(response!);
        }

        public async Task<BaseResponse<PaginationResult<T>>> GetEntitiesPagedWithSearchAsync(int pageIndex, int pageSize, string search)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"{_controllerName}/GetListPagedWithSearch?pageIndex={pageIndex}&pageSize={pageSize}&search={search}");

            if (!request.IsSuccessStatusCode)
            {
                var errorResponse = await request.Content.ReadFromJsonAsync<BaseResponse<PaginationResult<T>>>();

                return BaseResponse<PaginationResult<T>>.BuildErrorResponse(errorResponse!);
            }
            
            var response = await request.Content.ReadFromJsonAsync<PaginationResult<T>>();

            return BaseResponse<PaginationResult<T>>.BuildSuccessResponse(response!);
        }

        public async Task<BaseResponse<T>> GetEntityById(object identity)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync($"{_controllerName}/GetById/id={identity}");

            if (!request.IsSuccessStatusCode)
            {
                var errorResponse = await request.Content.ReadFromJsonAsync<BaseResponse<T>>();

                return BaseResponse<T>.BuildErrorResponse(errorResponse!);
            }

            var response = await request.Content.ReadFromJsonAsync<BaseResponse<T>>();

            return BaseResponse<T>.BuildSuccessResponse(response!.Data!);
        }

        public async Task<BaseResponse<T>> UpdateAsync(T entity)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var request = await _httpClient.PutAsJsonAsync($"{_controllerName}/Update", entity);

                if (!request.IsSuccessStatusCode)
                {
                    var errorResponse = await request.Content.ReadFromJsonAsync<BaseResponse<T>>();

                    return BaseResponse<T>.BuildErrorResponse(errorResponse!);
                }

                var response = await request.Content.ReadFromJsonAsync<BaseResponse<T>>();

                return BaseResponse<T>.BuildSuccessResponse(response!.Data!);
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }
    }
}
