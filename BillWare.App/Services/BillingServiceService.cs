using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Reflection;

namespace BillWare.App.Services
{
    public class BillingServiceService : BaseCrudService<BillingServiceModel>, IServicesService
    {
        public BillingServiceService(HttpClient http, 
                                     LocalStorageHelper localStorageService) : 
            base(http, localStorageService, "Service")
        {
        }
    }
}
