using BillWare.App.Intefaces;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class BillingService : IBillingService
    {
        private readonly HttpClient _httpClient;

        public BillingService(HttpClient http)
        {
            _httpClient = http;
        }

        public async Task<HttpResponseMessage> CreateBilling(BillingModel model)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("Billing/CreateBilling", model);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> DeleteBilling(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"Billing/DeleteBilling?id={id}");

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponseModel<BillingModel>> GetBilling(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<BillingModel>>($"Billing/GetBillingsPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }

        public async Task<HttpResponseMessage> UpdateBilling(BillingModel model)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync("Billing/UpdateBilling", model);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
