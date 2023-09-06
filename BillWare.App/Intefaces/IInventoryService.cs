using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IInventoryService
    {
        Task<BaseResponseModel<Inventory>> GetInventories(int pageIndex, int pageSize);
        Task<BaseResponseModel<Inventory>> GetInventoryWithSearch(string search, int pageIndex, int pageSize);
        Task<Inventory> CreateInvetory(Inventory inventory);
        Task<Inventory> EditInventory(Inventory inventory);
        Task<bool> DeleteInventory(int id);
    }
}
