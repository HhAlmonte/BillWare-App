using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface ICostumerService
    {
        Task<Costumer> GetCostumerById(int id);
    }
}
