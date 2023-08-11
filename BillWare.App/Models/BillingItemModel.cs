using BillWare.App.Common;

namespace BillWare.Application.Billing.Models
{
    public class BillingItemModel : BaseModel
    {
        public int Code { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Tax { get; set; }
        public decimal Amount { get; set; }
    }
}
