namespace BillWare.App.Common
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
            return $"{this.prefijo}-{numeroFactura:D4}";
        }
    }
}
