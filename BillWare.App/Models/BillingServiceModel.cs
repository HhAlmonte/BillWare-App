using BillWare.App.Common;

namespace BillWare.App.Models
{
    public class BillingServiceModel : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
