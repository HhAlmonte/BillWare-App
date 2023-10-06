using BillWare.App.Common;
using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IInventoryService
    {
        Task<PaginationResult<InventoryModel>> GetInventories(int pageIndex, int pageSize);
        Task<PaginationResult<InventoryModel>> GetInventoryWithSearch(string search, int pageIndex, int pageSize);
        Task<InventoryModel> CreateInvetory(InventoryModel inventory);
        Task<InventoryModel> EditInventory(InventoryModel inventory);
        Task<bool> DeleteInventory(int id);
    }
}
