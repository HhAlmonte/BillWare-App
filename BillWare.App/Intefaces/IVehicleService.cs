using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IVehicleService
    {
        Task<HttpResponseMessage> DeleteVehicle(int id);
        Task<HttpResponseMessage> CreateVehicle(Vehicle vehicle);
        Task<HttpResponseMessage> UpdateVehicle(Vehicle vehicle);
    }
}
