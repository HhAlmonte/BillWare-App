namespace BillWare.App.Intefaces
{
    public interface IBillingItemService
    {
        Task<HttpResponseMessage> DeleteBillingItem(int id);
    }
}
