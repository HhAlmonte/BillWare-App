using BillWare.App.Intefaces;
using BillWare.App.Models;

namespace BillWare.App.Services
{
    public class CostumerService : BaseCrudService<CostumerModel>, ICostumerService
    {
        public CostumerService(HttpClient http, LocalStorageHelper localStorageService) 
            : base(http, localStorageService, "Costumer")
        {
        }
    }
}
