namespace BillWare.App.Models
{
    public class BillingsParamsModel
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public DateTime? InitialDate { get; set; }
        public DateTime? FinalDate { get; set; }
    }
}
