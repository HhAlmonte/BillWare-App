using BillWare.App.Models;

namespace BillWare.App.Intefaces
{
    public interface IDashboardService
    {
        Task<List<StatisticsModel>> GetSalesLast30Days();
        Task<List<StatisticsModel>> GetSalesLast12Month();
    }
}
