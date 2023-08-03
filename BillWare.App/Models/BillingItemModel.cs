using BillWare.App.Common;

namespace BillWare.Application.Billing.Models
{
    public class BillingItemModel 
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
