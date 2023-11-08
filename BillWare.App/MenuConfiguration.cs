using BillWare.App.Models;
using System.Net.Http.Headers;

namespace BillWare.App
{
    public class MenuConfiguration
    {
        public List<MenuItem>? MenuItems { get; set; }

        public MenuConfiguration()
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem
                {
                    Name = "Dashboard",
                    Url = "/",
                    Icon = "bx bx-category",
                    Role = 
                    { 
                        "Administrator" 
                    }
                },
                new MenuItem
                {
                    Name = "Facturación",
                    Url = "billing/index",
                    Icon = "bx bxs-badge-dollar",
                    Role =
                    {
                        "Administrator",
                        "Operator"
                    }
                },
                new MenuItem 
                { 
                    Name = "Servicios", 
                    Url = "service/index", 
                    Icon = "bx bx-leaf", 
                    Role =
                    {
                        "Administrator",
                        "Operator"
                    } 
                },
                new MenuItem 
                { 
                    Name = "Inventario", 
                    Url = "inventory/index", 
                    Icon = "bx bx-store-alt", 
                    Role =
                    {
                        "Administrator",
                        "Operator"
                    } 
                },
                new MenuItem 
                { 
                    Name = "Categoría", 
                    Url = "category/index", 
                    Icon = "bx bx-category", 
                    Role =
                    {
                        "Administrator",
                        "Operator"
                    }
                },
                new MenuItem 
                { 
                    Name = "Cliente", 
                    Url = "costumer/index", 
                    Icon = "bx bx-user",
                    Role =
                    {
                        "Administrator",
                        "Operator"
                    }
                },
                new MenuItem
                {
                    Name = "Usuarios",
                    Url = "user/index",
                    Icon = "bx bx-user-circle",
                    Role =
                    {
                        "Administrator"
                    }
                }
            };
        }
    }
}
