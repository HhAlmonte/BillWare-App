using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly HttpClient _httpClient;

        public VehicleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateVehicle(Vehicle vehicle)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("Vehicle/CreateVehicle", vehicle);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> DeleteVehicle(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"Vehicle/DeleteVehicle/{id}");
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> UpdateVehicle(Vehicle vehicle)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync("Vehicle/UpdateVehicle", vehicle);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
