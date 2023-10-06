using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly HttpClient _httpClient;
        private readonly LocalStorageHelper _localStorageService;

        public InventoryService(HttpClient httpClient, LocalStorageHelper localStorageService)
        {
            _httpClient = httpClient;
            _localStorageService = localStorageService;
        }

        public async Task<InventoryModel> CreateInvetory(InventoryModel inventory)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PostAsJsonAsync("Inventory/CreateInventory", inventory);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<InventoryModel>();
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

        public async Task<bool> DeleteInventory(int id)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.DeleteAsync($"Inventory/DeleteInventory/{id}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<bool>();
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

        public async Task<InventoryModel> EditInventory(InventoryModel inventory)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.PutAsJsonAsync("Inventory/UpdateInventory", inventory);

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<InventoryModel>();
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

        public async Task<PaginationResult<InventoryModel>> GetInventories(int pageIndex, int pageSize)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"Inventory/GetInventoriesPaged?pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginationResult<InventoryModel>>();
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

        public async Task<PaginationResult<InventoryModel>> GetInventoryWithSearch(string search, int pageIndex, int pageSize)
        {
            try
            {
                var token = await _localStorageService.GetItem(Configuration.TOKEN);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await _httpClient.GetAsync($"Inventory/GetInventoryWithSearch?search={search}&pageIndex={pageIndex}&pageSize={pageSize}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<PaginationResult<InventoryModel>>();
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
