namespace BillWare.App.Helpers
{
    public class InvoiceNumberGenerator
    {
        private string prefijo;

        public InvoiceNumberGenerator(string prefijo)
        {
            this.prefijo = prefijo;
        }

        public string GenerateNumber(int numeroFactura = 1)
        {
            numeroFactura++;
            return $"{prefijo}-{numeroFactura:D4}";
        }
    }
}
