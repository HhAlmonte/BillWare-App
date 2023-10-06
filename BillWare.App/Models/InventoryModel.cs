using BillWare.App.Common;

namespace BillWare.App.Models
{
    public class InventoryModel : BaseModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public CategoryModel Category { get; set; }
        public int CategoryId { get; set; }
        public int Quantity { get; set; }
    }
}
