using BillWare.App.Common;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;

namespace BillWare.App.Intefaces
{
    public interface IBillingService : IBaseCrudService<BillingModel>
    {
        Task<PaginationResult<BillingModel>> GetBillingsWithParams(BillingsParamsModel billingsParams);
        Task<int> GetLastInvoiceNumber();
    }
}
