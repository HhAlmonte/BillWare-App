using BillWare.App.Common;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;

namespace BillWare.App.Intefaces
{
    public interface IBillingService
    {
        Task<BillingModel> CreateBilling(BillingModel model);
        Task<BillingModel> UpdateBilling(BillingModel model);
        Task<bool> DeleteBilling(int id);
        Task<PaginationResult<BillingModel>> GetBillingsWithSearch(string search, int pageIndex, int pageSize);
        Task<PaginationResult<BillingModel>> GetBillingsWithParams(BillingsParamsModel billingsParams);
        Task<PaginationResult<BillingModel>> GetBillings(int pageIndex, int pageSize);
        Task<int> GetLastInvoiceNumber();
    }
}
