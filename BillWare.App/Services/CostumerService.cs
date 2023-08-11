using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class CostumerService : ICostumerService
    {
        private readonly HttpClient _httpClient;

        public CostumerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Costumer> GetCostumerById(int id)
        {
            try
            {
                var request = await _httpClient.GetFromJsonAsync<Costumer>($"Costumer/GetCostumerById?id={id}");

                return request;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
