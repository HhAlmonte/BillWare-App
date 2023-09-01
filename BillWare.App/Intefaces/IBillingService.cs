using BillWare.App.Models;
using BillWare.Application.Billing.Models;

namespace BillWare.App.Intefaces
{
    public interface IBillingService
    {
        Task<BillingModel> CreateBilling(BillingModel model);
        Task<HttpResponseMessage> UpdateBilling(BillingModel model);
        Task<HttpResponseMessage> DeleteBilling(int id);
        Task<BaseResponseModel<BillingModel>> GetBillingWithSearch(string search, int pageIndex, int pageSize);
        Task<BaseResponseModel<BillingModel>> GetBillingWithParams(BillingsParamsModel billingsParams);
        Task<BaseResponseModel<BillingModel>> GetBilling(int pageIndex, int pageSize);
        Task<int> GetLastInvoiceNumber();
    }
}
