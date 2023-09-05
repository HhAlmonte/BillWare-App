using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IBillingServiceService
    {
        Task<BaseResponseModel<BillingServiceModel>> GetBillingsServices(int pageIndex, int pageSize);
        Task<BaseResponseModel<BillingServiceModel>> GetBillingsServicesWithSearch(string search, int pageIndex, int pageSize);
        Task<BillingServiceModel> CreateBillingService(BillingServiceModel billingService);
        Task<BillingServiceModel> EditBillingService(BillingServiceModel billingService);
        Task<bool> DeleteBillingService(int id);
    }
}
