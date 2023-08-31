using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface ICostumerService
    {
        Task<Costumer> GetCostumerById(int id);

        Task<BaseResponseModel<Costumer>> GetCostumersPaged(int pageIndex, int pageSize);

        Task<BaseResponseModel<Costumer>> GetCostumersPagedWithSearch(int pageIndex, int pageSize, string search);

        Task<HttpResponseMessage> DeleteCostumer(int id);

        Task<HttpResponseMessage> CreateCostumer(Costumer costumer);

        Task<HttpResponseMessage> EditCostumer(Costumer costumer);
    }
}
