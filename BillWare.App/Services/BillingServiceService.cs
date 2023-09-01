using BillWare.App.Intefaces;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using System.Net.Http.Json;
using System.Reflection;

namespace BillWare.App.Services
{
    public class BillingServiceService : IBillingServiceService
    {
        private readonly HttpClient _httpClient;

        public BillingServiceService(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<HttpResponseMessage> CreateBillingService(BillingServiceModel billingService)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("BillingService/CreateBillingService", billingService);

                if (request.IsSuccessStatusCode)
                {
                    return request;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> DeleteBillingService(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"BillingService/DeleteBillingService/{id}");

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> EditBillingService(BillingServiceModel billingService)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync("BillingService/UpdateBillingService", billingService);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponseModel<BillingServiceModel>> GetBillingsServices(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<BillingServiceModel>>($"BillingService/GetBillingsServicesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }

        public async Task<BaseResponseModel<BillingServiceModel>> GetBillingsServicesWithSearch(string search, int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<BillingServiceModel>>($"Billing/GetBillingsServicesPagedWithSearch?search={search}&pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }
    }
}
