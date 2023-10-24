using BillWare.App.Common;
using BillWare.App.Models;

namespace BillWare.Application.Billing.Models
{
    public class BillingModel : BaseModel
    {
        public int CostumerId { get; set; }
        public CostumerModel Costumer { get; set; } = new CostumerModel();
        public string SellerName { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceWithTax { get; set; }
        public decimal TotalTax { get; set; }

        public int PaymentMethod { get; set; }
        public int BillingStatus { get; set; }
        public List<BillingItemModel> BillingItems { get; set; } = new List<BillingItemModel>();
    }
}
