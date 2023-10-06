using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public DashboardService(HttpClient http, LocalStorageHelper localStorageService)
        {
            _httpClient = http;
            _localStorageService = localStorageService;
        }

        public async Task<List<StatisticsModel>> GetSalesLast12Month()
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"Dashboard/GetSalesLast12Month");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<StatisticsModel>>();
                return result;
            }
            else
            {
                throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
            }
        }

        public async Task<List<StatisticsModel>> GetSalesLast30Days()
        {
            var token = await _localStorageService.GetItem(Configuration.TOKEN);

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"Dashboard/GetSalesLast30Days");

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<List<StatisticsModel>>();
                return result;
            }
            else
            {
                throw new HttpRequestException($"Error de solicitud HTTP: {response.StatusCode}");
            }
        }
    }
}
