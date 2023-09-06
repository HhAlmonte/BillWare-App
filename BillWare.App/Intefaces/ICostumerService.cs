using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface ICostumerService
    {
        Task<Costumer> GetCostumerById(int id);

        Task<BaseResponseModel<Costumer>> GetCostumersPaged(int pageIndex, int pageSize);

        Task<BaseResponseModel<Costumer>> GetCostumersPagedWithSearch(int pageIndex, int pageSize, string search);

        Task<bool> DeleteCostumer(int id);

        Task<Costumer> CreateCostumer(Costumer costumer);

        Task<Costumer> EditCostumer(Costumer costumer);
    }
}
