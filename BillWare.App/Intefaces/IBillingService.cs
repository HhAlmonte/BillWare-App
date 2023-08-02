using BillWare.App.Models;
using BillWare.Application.Billing.Models;

namespace BillWare.App.Intefaces
{
    public interface IBillingService
    {
        Task<HttpResponseMessage> CreateBilling(BillingModel model);
        Task<HttpResponseMessage> UpdateBilling(BillingModel model);
        Task<HttpResponseMessage> DeleteBilling(int id);

        Task<BaseResponseModel<BillingModel>> GetBilling(int pageIndex, int pageSize);
    }
}
