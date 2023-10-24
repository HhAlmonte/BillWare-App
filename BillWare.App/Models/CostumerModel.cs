using BillWare.App.Common;

namespace BillWare.App.Models
{
    public class CostumerModel : BaseModel
    {
        public string FullName { get; set; } = string.Empty;
        public string? Phone { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
        public string? NumberId { get; set; } = string.Empty; 

        public string? FullNameWithId => $"{FullName} ({NumberId})";
    }
}
