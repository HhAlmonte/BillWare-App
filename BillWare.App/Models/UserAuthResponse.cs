namespace BillWare.App.Models
{
    public class UserAuthResponse
    {
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
