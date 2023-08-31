using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<StatisticsModel>> GetSalesLast12Month()
        {
            try
            {
                var request = await _httpClient.GetFromJsonAsync<List<StatisticsModel>>($"Dashboard/GetSalesLast12Month");

                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<List<StatisticsModel>> GetSalesLast30Days()
        {
            try
            {
                var request = await _httpClient.GetFromJsonAsync<List<StatisticsModel>>($"Dashboard/GetSalesLast30Days");

                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
