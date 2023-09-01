using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IBillingServiceService
    {
        Task<BaseResponseModel<BillingServiceModel>> GetBillingsServices(int pageIndex, int pageSize);
        Task<BaseResponseModel<BillingServiceModel>> GetBillingsServicesWithSearch(string search, int pageIndex, int pageSize);
        Task<HttpResponseMessage> CreateBillingService(BillingServiceModel billingService);
        Task<HttpResponseMessage> EditBillingService(BillingServiceModel billingService);
        Task<HttpResponseMessage> DeleteBillingService(int id);
    }
}
