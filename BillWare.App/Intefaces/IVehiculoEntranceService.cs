using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IVehiculoEntranceService
    {
        Task<BaseResponseModel<VehicleEntrance>> GetVehiculoEntrance(int pageIndex, int pageSize);
        Task<BaseResponseModel<VehicleEntrance>> GetVehicleEntranceWithParams(string fullName, int pageIndex, int pageSize);
        Task<HttpResponseMessage> CreateVehiculoEntrance(VehicleEntrance vehicleEntrance);
        Task<HttpResponseMessage> EditVehiculoEntrance(VehicleEntrance vehicleEntrance);
        Task<HttpResponseMessage> DeleteVehiculoEntrance(int id);
    }
}

