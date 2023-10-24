namespace BillWare.App.Models
{
    public class HoraExtra
    {
        public string Code { get; set; }
        public string EmployeeName { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public EstadoSolicitud Estado { get; set; }
    }

    public enum EstadoSolicitud
    {
        Aprobado = 1,
        AprobadoPorGerenteArea,
        AprobadoPorGerenteGeneral,
        Cancelado
    }
}
