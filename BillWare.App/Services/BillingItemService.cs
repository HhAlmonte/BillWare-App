using BillWare.App.Intefaces;

namespace BillWare.App.Services
{
    public class BillingItemService : IBillingItemService
    {
        private readonly HttpClient _httpClient;

        public BillingItemService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<HttpResponseMessage> DeleteBillingItem(int id)
        {
            try
            {
                var request = await _httpClient.DeleteAsync($"BillingItem/DeleteBillingItem?id={id}");

                return request;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
