namespace BillWare.App
{
    public class MenuConfiguration
    {
        public List<MenuItems> MenuItems { get; set; }

        public MenuConfiguration()
        {
            MenuItems.Add(new MenuItems("bx bx-category", "Dashboard", "/", new string[] {  }));

            MenuItems.Add(new MenuItems("bx bx-category", "Servicios", "service/index", new string[] {  }));

            MenuItems.Add(new MenuItems("bx bx-store-alt", "Inventario", "inventory/index", new string[] {  }));

            MenuItems.Add(new MenuItems("bx bx-category", "Categoría", "category/index", new string[] {  }));

            MenuItems.Add(new MenuItems("bx bx-user", "Cliente", "costumer/index", new string[] {  }));

            MenuItems.Add(new MenuItems("bx bxs-badge-dollar", "Facturación", "billing/index", new string[] {  }));
        }
    }

    public class MenuItems
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Route { get; set; }
        public string[] Roles { get; set; }

        public MenuItems(string icon, string name, string route, string[] roles)
        {
            Icon = icon;
            Name = name;
            Route = route;
            Roles = roles;
        }
    }
}
