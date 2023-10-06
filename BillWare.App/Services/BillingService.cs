using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class BillingService : IBillingService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public BillingService(HttpClient http, LocalStorageHelper localStorageService)
        {
            _httpClient = http;
            _localStorageService = localStorageService;
        }

        public async Task<BillingModel> CreateBilling(BillingModel model)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PostAsJsonAsync("Billing/CreateBilling", model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BillingModel>();
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
        }

        public async Task<bool> DeleteBilling(int id)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.DeleteAsync($"Billing/DeleteBilling?id={id}");

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

        public async Task<PaginationResult<BillingModel>> GetBillings(int pageIndex, int pageSize)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"Billing/GetListPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginationResult<BillingModel>>();
                return result;
            }
            else
            {
                throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
            }
        }

        public async Task<PaginationResult<BillingModel>> GetBillingsWithParams(BillingsParamsModel billingsParams)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var url = $"Billing/GetListPagedPagedWithParams?pageIndex={billingsParams.PageIndex}&pageSize={billingsParams.PageSize}";

            if (billingsParams.InitialDate != null)
            {
                url += $"&initialDate={billingsParams.InitialDate:MM/dd/yyyy}&finalDate={billingsParams.FinalDate:MM/dd/yyyy}";
            }

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginationResult<BillingModel>>();
                return result;
            }
            else
            {
                throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
            }
        }

        public async Task<PaginationResult<BillingModel>> GetBillingsWithSearch(string search, int pageIndex, int pageSize)
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"Billing/GetListPagedWithSearch?search={search}&pageIndex={pageIndex}&pageSize={pageSize}");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<PaginationResult<BillingModel>>();
                return result;
            }
            else
            {
                throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
            }
        }

        public async Task<int> GetLastInvoiceNumber()
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync("Billing/GetInvoiceNumber");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<int>();
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

        public async Task<BillingModel> UpdateBilling(BillingModel model)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await _httpClient.PutAsJsonAsync("Billing/UpdateBilling", model);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<BillingModel>();
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
