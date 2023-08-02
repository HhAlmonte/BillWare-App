using BillWare.App.Common;

namespace BillWare.Application.Billing.Models
{
    public class BillingItemModel 
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
