namespace BillWare.App.Models
{
    public class MenuItem
    {
        public string Name { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
        public List<string> Role { get; set; } = new List<string>();

        public List<MenuItem>? SubMenuItems { get; set; } = new List<MenuItem>();
    }
}
