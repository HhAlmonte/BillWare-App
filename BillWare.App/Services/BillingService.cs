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

        public async Task<BillingModel> CreateBilling(BillingModel model)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("Billing/CreateBilling", model);

                if (request.IsSuccessStatusCode)
                {
                    var response = await request.Content.ReadFromJsonAsync<BillingModel>();

                    return response;
                }

                return null;
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

        public async Task<BaseResponseModel<BillingModel>> GetBillingWithParams(BillingsParamsModel billingsParams)
        {
            var url = $"Billing/GetBillingsPagedWithParams?pageIndex={billingsParams.PageIndex}&pageSize={billingsParams.PageSize}";

            if (billingsParams.InitialDate != null)
            {
                url += $"&initialDate={billingsParams.InitialDate:MM/dd/yyyy}&finalDate={billingsParams.FinalDate:MM/dd/yyyy}";
            }

            var response = await _httpClient.GetFromJsonAsync<BaseResponseModel<BillingModel>>(url);

            return response;
        }

        public async Task<BaseResponseModel<BillingModel>> GetBillingWithSearch(string search, int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<BillingModel>>($"Billing/GetBillingsWithSearchPaged?search={search}&pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }

        public async Task<int> GetLastInvoiceNumber()
        {
            var response = await _httpClient
                .GetFromJsonAsync<int>("Billing/GetInvoiceNumber");

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
