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

                var response = await _httpClient.PostAsJsonAsync($"{_controllerName}/Create", entity);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<T>();

                    return new BaseResponse<T>
                    {
                        IsSuccessFul = true,
                        Data = result ?? new BaseResponse<T>().Data
                    };
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<T>>();

                    return new BaseResponse<T>
                    {
                        IsSuccessFul = false,
                        Message = result!.Message,
                        StatusCode = result!.StatusCode,
                        Details = result.Details
                    };
                }
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

                var response = await _httpClient.DeleteAsync($"{_controllerName}/Delete/id={identity}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<T>();

                    return new BaseResponse<T>
                    {
                        IsSuccessFul = true,
                        Data = result
                    };
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<T>>();

                    return new BaseResponse<T>
                    {
                        IsSuccessFul = false,
                        Message = result!.Message,
                        StatusCode = result!.StatusCode,
                        Details = result.Details
                    };
                }
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

            var response = await _httpClient.GetAsync($"{_controllerName}/GetListPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginationResult<T>>();
                return new BaseResponse<PaginationResult<T>>
                {
                    IsSuccessFul = true,
                    Data = result
                };
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<BaseResponse<PaginationResult<T>>>();

                return new BaseResponse<PaginationResult<T>>
                {
                    IsSuccessFul = false,
                    Message = result!.Message,
                    StatusCode = result!.StatusCode,
                    Details = result.Details
                };
            }
        }

        public async Task<BaseResponse<PaginationResult<T>>> GetEntitiesPagedWithSearchAsync(int pageIndex, int pageSize, string search)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_controllerName}/GetListPagedWithSearch?pageIndex={pageIndex}&pageSize={pageSize}&search={search}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginationResult<T>>();
                return new BaseResponse<PaginationResult<T>>
                {
                    IsSuccessFul = true,
                    Data = result
                };
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<BaseResponse<PaginationResult<T>>>();

                return new BaseResponse<PaginationResult<T>>
                {
                    IsSuccessFul = false,
                    Message = result!.Message,
                    StatusCode = result!.StatusCode,
                    Details = result.Details
                };
            }
        }

        public async Task<BaseResponse<T>> GetEntityById(object identity)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_controllerName}/GetById/id={identity}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<T>();
                return new BaseResponse<T>
                {
                    IsSuccessFul = true,
                    Data = result
                };
            }
            else
            {
                var result = await response.Content.ReadFromJsonAsync<BaseResponse<T>>();

                return new BaseResponse<T>
                {
                    IsSuccessFul = false,
                    Message = result!.Message,
                    StatusCode = result!.StatusCode,
                    Details = result.Details
                };
            }
        }

        public async Task<BaseResponse<T>> UpdateAsync(T entity)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync($"{_controllerName}/Update", entity);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<T>();

                    return new BaseResponse<T>
                    {
                        IsSuccessFul = true,
                        Data = result
                    };
                }
                else
                {
                    var result = await response.Content.ReadFromJsonAsync<BaseResponse<T>>();

                    return new BaseResponse<T>
                    {
                        IsSuccessFul = false,
                        Message = result!.Message,
                        StatusCode = result!.StatusCode,
                        Details = result.Details
                    };
                }
            }
            catch (HttpRequestException ex)
            {
                throw ex;
            }
        }
    }
}
