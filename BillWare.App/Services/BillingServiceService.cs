using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace BillWare.App.Services
{
    public class BillingServiceService : IServicesService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public BillingServiceService(HttpClient http, LocalStorageHelper localStorageService)
        {
            _httpClient = http;
            _localStorageService = localStorageService;
        }

        public async Task<BillingServiceModel> CreateBillingService(BillingServiceModel billingService)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("BillingService/CreateBillingService", billingService);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BillingServiceModel>();
                    return result;
                }
                else
                {
                    throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteBillingService(int id)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"BillingService/DeleteBillingService/{id}");

                if (response.IsSuccessStatusCode)
                {
                    return true;
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

        public async Task<BillingServiceModel> EditBillingService(BillingServiceModel billingService)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync("BillingService/UpdateBillingService", billingService);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BillingServiceModel>();
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

        public async Task<PaginationResult<BillingServiceModel>> GetBillingsServices(int pageIndex, int pageSize)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"BillingService/GetBillingsServicesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginationResult<BillingServiceModel>>();
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

        public async Task<PaginationResult<BillingServiceModel>> GetBillingsServicesWithSearch(string search, int pageIndex, int pageSize)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"BillingService/GetBillingsServicesWithSearch?search={search}&pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginationResult<BillingServiceModel>>();
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
    }
}
