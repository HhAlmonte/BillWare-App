using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class VehiculoEntranceService : IVehiculoEntranceService
    {
        private readonly HttpClient _httpClient;

        public VehiculoEntranceService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> CreateVehiculoEntrance(VehicleEntrance vehicleEntrance)
        {
            try
            {
                var request = await _httpClient.PostAsJsonAsync("VehicleEntrance/CreateVehicleEntrance", vehicleEntrance);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> DeleteVehiculoEntrance(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"VehicleEntrance/DeleteVehicleEntrance/{id}");
                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<HttpResponseMessage> EditVehiculoEntrance(VehicleEntrance vehicleEntrance)
        {
            try
            {
                var request = await _httpClient.PutAsJsonAsync("VehicleEntrance/UpdateVehicleEntrance", vehicleEntrance);

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        public async Task<BaseResponseModel<VehicleEntrance>> GetVehicleEntranceWithParams(string fullName, int pageIndex, int pageSize)
        {
            var route = $"VehicleEntrance/GetVehicleEntranceWithParams?fullName={fullName}&pageIndex={pageIndex}&pageSize={pageSize}";

            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<VehicleEntrance>>(route);

            return response;
        }

        public async Task<BaseResponseModel<VehicleEntrance>> GetVehiculoEntrance(int pageIndex, int pageSize)
        {
            var response = await _httpClient
                .GetFromJsonAsync<BaseResponseModel<VehicleEntrance>>($"VehicleEntrance/GetVehicleEntrancePaged?pageIndex={pageIndex}&pageSize={pageSize}");

            return response;
        }
    }
}

