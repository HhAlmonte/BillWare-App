using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IInventoryService
    {
        Task<BaseResponseModel<Inventory>> GetInventories(int pageIndex, int pageSize);
        Task<BaseResponseModel<Inventory>> GetInventoryWithSearch(string search, int pageIndex, int pageSize);
        Task<HttpResponseMessage> CreateInvetory(Inventory inventory);
        Task<HttpResponseMessage> EditInventory(Inventory inventory);
        Task<HttpResponseMessage> DeleteInventory(int id);
    }
}
