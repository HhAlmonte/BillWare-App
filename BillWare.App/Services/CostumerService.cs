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

        public async Task<HttpResponseMessage> CreateCostumer(Costumer costumer)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("Costumer/CreateCostumer", costumer);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> DeleteCostumer(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"Costumer/DeleteCostumer/{id}");
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> EditCostumer(Costumer costumer)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync($"Costumer/UpdateCostumer", costumer);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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

        public async Task<BaseResponseModel<Costumer>> GetCostumersPaged(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<Costumer>>($"Costumer/GetCostumersPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }

        public async Task<BaseResponseModel<Costumer>> GetCostumersPagedWithSearch(int pageIndex, int pageSize, string search)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<Costumer>>($"Costumer/GetCostumersPagedWithSearch?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }
    }
}
