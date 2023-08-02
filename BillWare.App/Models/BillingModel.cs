using BillWare.App.Common;

namespace BillWare.Application.Billing.Models
{
    public class BillingModel : BaseModel
    {
        public string FullName { get; set; }

        public List<BillingItemModel> BillingItems { get; set; }

        public int BillingType { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
