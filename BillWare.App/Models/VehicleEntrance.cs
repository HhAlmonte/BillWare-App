namespace BillWare.App.Models
{
    public class VehicleEntrance
    {
        public int Id { get; set; }
        public Costumer Costumer { get; set; }
        public List<Vehicle> Vehicles { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
