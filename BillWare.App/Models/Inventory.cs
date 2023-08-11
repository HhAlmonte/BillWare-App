using BillWare.App.Common;

namespace BillWare.App.Models
{
    public class Inventory : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public int Quantity { get; set; }
    }
}
