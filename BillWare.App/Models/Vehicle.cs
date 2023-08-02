namespace BillWare.App.Models
{
    public class Vehicle 
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Color { get; set; }
        public string Year { get; set; }
        public string Plate { get; set; }
        public string? Comments { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime DeletedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }

        public int VehicleEntranceEntityId { get; set; }
    }
}
