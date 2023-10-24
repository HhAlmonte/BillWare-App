using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IHoraExtraService
    {
        Task<List<HoraExtra>> GetHorasExtras();
        Task<bool> CreateSolicitudHoraExtra(HoraExtra solicitudRequest);
        Task<bool> UpdateSolicitudHoraExtra(HoraExtra solicitudRequest);
        Task<bool> DenegarSolicitud(int idSolicitud);
        Task<bool> UpdateStatusHoraExtra(int idSolicitud, int estatusId);
    }
}
