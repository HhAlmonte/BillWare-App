using BillWare.App.Intefaces;
using BillWare.App.Models;
using System.Net.Http.Json;

namespace BillWare.App.Services
{
    public class HoraExtraService : IHoraExtraService
    {
        private readonly HttpClient _httpClient;

        public HoraExtraService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public Task<bool> CreateSolicitudHoraExtra(HoraExtra solicitudRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DenegarSolicitud(int idSolicitud)
        {
            throw new NotImplementedException();
        }

        public async Task<List<HoraExtra>> GetHorasExtras()
        {
            var response = await _httpClient.GetFromJsonAsync<List<HoraExtra>>("HoraExtra/GetHorasExtras");

            return response;
        }

        public Task<bool> UpdateSolicitudHoraExtra(HoraExtra solicitudRequest)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStatusHoraExtra(int idSolicitud, int estatusId)
        {
            throw new NotImplementedException();
        }
    }
}
