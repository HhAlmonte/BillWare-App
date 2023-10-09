using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class BillingService : BaseCrudService<BillingModel> ,IBillingService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public BillingService(HttpClient http, LocalStorageHelper localStorageService) : base(http, localStorageService, "Billing")
        {
            _httpClient = http;
            _localStorageService = localStorageService;
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

        public async Task<int> GetLastInvoiceNumber()
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = await _httpClient.GetAsync("Billing/GetInvoiceNumber");

            if (request.IsSuccessStatusCode)
            {
                var result = await request.Content.ReadFromJsonAsync<int>();

                return result;
            }

            throw new HttpRequestException($"Error de solicitud HTTP: {request.StatusCode}");
        }
    }
}
