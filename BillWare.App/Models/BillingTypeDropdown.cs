namespace BillWare.App.Models
{
    public class BillingTypeDropdown
    {
        public int Id { get; set; }
        public string Name { get; set; }



        public static List<BillingTypeDropdown> GetBillingTypeDropdowns()
        {
            List<BillingTypeDropdown> billingTypeDropdowns = new List<BillingTypeDropdown>
            {
                new BillingTypeDropdown { Id = 2, Name = "Vehículo de Entrada" },
                new BillingTypeDropdown { Id = 1, Name = "Inventario" }
            };

            return billingTypeDropdowns;
        }
    }
}
