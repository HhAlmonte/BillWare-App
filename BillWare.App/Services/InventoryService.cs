using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;

        public InventoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateInvetory(Inventory inventory)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("Inventory/CreateInventory", inventory);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> DeleteInventory(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"Inventory/DeleteInventory/{id}");
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> EditInventory(Inventory inventory)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync($"Inventory/UpdateInventory", inventory);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BaseResponseModel<Inventory>> GetInventories(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<Inventory>>($"Inventory/GetInventoriesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }

        public Task<BaseResponseModel<Inventory>> GetInventoryWithSearch(string search, int pageIndex, int pageSize)
        {
            var response = _httpClient
                .GetFromJsonAsync<BaseResponseModel<Inventory>>($"Inventory/GetInventoryWithSearch?search={search}&pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }
    }
}
