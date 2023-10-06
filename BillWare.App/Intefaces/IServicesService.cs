using BillWare.App.Common;
using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IServicesService
    {
        Task<PaginationResult<BillingServiceModel>> GetBillingsServices(int pageIndex, int pageSize);
        Task<PaginationResult<BillingServiceModel>> GetBillingsServicesWithSearch(string search, int pageIndex, int pageSize);
        Task<BillingServiceModel> CreateBillingService(BillingServiceModel billingService);
        Task<BillingServiceModel> EditBillingService(BillingServiceModel billingService);
        Task<bool> DeleteBillingService(int id);
    }
}
