using BillWare.App.Common;

namespace BillWare.Application.Billing.Models
{
    public class BillingModel : BaseModel
    {
        // Costumer
        public string FullName { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? NumberId { get; set; }

        // Billing
        public string SellerName { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal TotalPriceWithTax { get; set; }
        public decimal TotalTax { get; set; }

        public int PaymentMethod { get; set; }
        public int BillingStatus { get; set; }
        public List<BillingItemModel> BillingItems { get; set; }
    }
}
