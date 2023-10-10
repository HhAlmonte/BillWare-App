using BillWare.App.Intefaces;
using BillWare.App.Models;

namespace BillWare.App.Services
{
    public class InventoryService : BaseCrudService<InventoryModel>, IInventoryService
    {
        public InventoryService(HttpClient http,
                                LocalStorageHelper localStorageService)
            : base(http, localStorageService, "Inventory")
        {
        }
    }
}
