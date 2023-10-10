using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;
using Microsoft.AspNetCore.Authorization;
using BillWare.App.Helpers;
using BillWare.App.Enum;
using BillWare.App.Intefaces;

namespace BillWare.App.Pages.Billing
{
    [Authorize("Administrator, Operator")]
    public partial class BillingForm
    {
        [Inject] private IBillingService? _billingService { get; set; }
        [Inject] private IBillingItemService? _billingItemService { get; set; }
        [Inject] private ICostumerService? _costumerService { get; set; }
        [Inject] private IInventoryService? _inventoryService { get; set; }
        [Inject] private IServicesService? _billingServiceService { get; set; }
        [Inject] private DialogService? DialogService { get; set; }
        [Inject] BeamAuthenticationStateProviderHelper? BeamAuthenticationStateProviderHelper { get; set; }

        [Parameter] public BillingModel BillingParameter { get; set; } = new BillingModel();
        [Parameter] public FormModeEnum FormMode { get; set; } = FormModeEnum.ADD;


        private BillingModel Billing = new BillingModel()
        {
            CreatedAt = DateTime.Now
        };
        private RadzenDataGrid<BillingItemModel>? grid;
        private InvoiceNumberGenerator InvoiceNumberGenerator = new InvoiceNumberGenerator("FACT");

        private List<PaymentMethod> PaymentMethods { get; set; } = new List<PaymentMethod>
        {
            new PaymentMethod
            {
                Name = "Efectivo",
                Id = 1
            },
            new PaymentMethod
            {
                Name = "Transferencia",
                Id = 2
            },
        };

        private List<BillingItemModel> BillingItems { get; set; } = new List<BillingItemModel>();

        private List<BillingItemModel> BillingInventoriesItems { get; set; } = new List<BillingItemModel>();
        private List<BillingItemModel> BillingServicesItems { get; set; } = new List<BillingItemModel>();

        private List<BillingItemModel> Inventories { get; set; } = new List<BillingItemModel>();
        private List<BillingItemModel> BillingsServices { get; set; } = new List<BillingItemModel>();


        public string ReturnMoney { get; set; } = "0";
        private string? CostumerId { get; set; }
        private string? SellerName { get; set; }

        private void OnDropDowmChange()
        {
            BillingItems.Clear();

            foreach (var item in BillingInventoriesItems)
            {
                if (!BillingItems.Contains(item))
                {
                    BillingItems.Add(item);
                }
            }

            foreach (var item in BillingServicesItems)
            {
                if (!BillingItems.Contains(item))
                {
                    BillingItems.Add(item);
                }
            }

            CalculateNetoAndTotalPrice();
            grid.Reload();
            StateHasChanged();
        }

        private async Task OpenQuantityormDialog(int quantity, int code)
        {
            var dialogResponse = await DialogService!.OpenAsync<QuantityForm>("Modificar cantidad"
                , parameters: new Dictionary<string, object>
                {
                    { "QuantityParameter", quantity }
                }
                , options: new DialogOptions
                {
                    Width = "auto"
                });

            var index = BillingItems.FindIndex(x => x.Code == code);

            if(dialogResponse != null)
            {
                BillingItems[index].Quantity = dialogResponse;
            }
            else
            {
                BillingItems[index].Quantity = quantity;
            }


            foreach (var i in BillingItems)
            {
                i.Amount = i.Quantity * i.Price;
                i.Tax = CalcularITBIS(i.Quantity * i.Price);
            }

            CalculateNetoAndTotalPrice();

            await grid!.Reload();

        }

        private async Task GetCostumer()
        {
            if (string.IsNullOrEmpty(CostumerId))
            {
                await SweetAlertServices.ShowErrorAlert("Codigo invalido", "Primero debe ingresar un código");
                return;
            }

            int costumerId = Convert.ToInt32(CostumerId);

            var costumer = await _costumerService!.GetEntityById(costumerId);

            Billing.FullName = costumer.Data!.FullName;
            Billing.Address = costumer.Data.Address;
            Billing.Phone = costumer.Data.Phone;
            Billing.NumberId = costumer.Data.NumberId;

            StateHasChanged();
        }

        private async Task LoadInventories()
        {
            var data = await _inventoryService!.GetEntitiesPagedAsync(1, 50);

            Inventories = data.Data!.Items.Select(x => new BillingItemModel
            {
                Code = x.Id,
                Description = x.Name,
                Quantity = 1,
                Price = x.Price,
                Tax = 0,
                Amount = x.Price * x.Quantity
            }).ToList();

            StateHasChanged();
        }
        private async Task LoadBillingsService()
        {
            var data = await _billingServiceService!.GetEntitiesPagedAsync(1, 50);

            BillingsServices = data.Data!.Items.Select(x => new BillingItemModel
            {
                Code = x.Id,
                Description = x.Name,
                Quantity = 1,
                Price = x.Price,
                Tax = 0,
                Amount = x.Price * 1

            }).ToList();

            StateHasChanged();
        }

        private async Task DeleteBillingItem(BillingItemModel billingItem)
        {
            if (FormMode == FormModeEnum.EDIT)
            {
                await _billingItemService!.DeleteBillingItem(billingItem.Id);
            }
            else
            {
                var index = BillingItems.FindIndex(x => x.Code == billingItem.Code);
                var inventoyIndex = BillingInventoriesItems.FindIndex(x => x.Code == billingItem.Code);
                var serviceIndex = BillingServicesItems.FindIndex(x => x.Code == billingItem.Code);

                if (inventoyIndex != -1)
                {
                    BillingInventoriesItems.RemoveAt(inventoyIndex);
                }

                if (serviceIndex != -1)
                {
                    BillingServicesItems.RemoveAt(serviceIndex);
                }

                BillingItems.RemoveAt(index);
            }

            CalculateNetoAndTotalPrice();

            await grid!.Reload();
        }

        public decimal CalcularITBIS(decimal monto)
        {
            decimal porcentajeITBIS = 0.18m;
            decimal impuestoCalculado = monto * porcentajeITBIS;
            return 0;
        }
        public void CalcularDevolucion(decimal monto)
        {
            CalculateNetoAndTotalPrice();

            var returnMoney = monto - Billing.TotalPriceWithTax;

            ReturnMoney = returnMoney.ToString("C");

            StateHasChanged();
        }
        private void CalculateNetoAndTotalPrice()
        {
            Billing.TotalPrice = (decimal)BillingItems.Sum(x => x.Amount);

            Billing.TotalPriceWithTax = Billing.TotalPrice + (decimal)BillingItems.Sum(x => x.Tax);

            Billing.TotalTax = (decimal)BillingItems.Sum(x => x.Tax);

            StateHasChanged();
        }

        private async Task GetInventoryWithSearch(string searchText)
        {
            var result = await _inventoryService!.GetEntitiesPagedWithSearchAsync(1, 100, searchText);

            Inventories = result.Data!.Items.Select(x => new BillingItemModel
            {
                Code = x.Id,
                Description = x.Name,
                Quantity = 1,
                Price = x.Price,
                Tax = 0,
                Amount = x.Price * x.Quantity
            }).ToList();

            StateHasChanged();
        }
        private async Task GetBillingsServicesWithSearch(string searchText)
        {
            var result = await _billingServiceService!.GetEntitiesPagedWithSearchAsync(1, 100, searchText);

            BillingsServices = result.Data!.Items.Select(x => new BillingItemModel
            {
                Code = x.Id,
                Description = x.Name,
                Quantity = 1,
                Price = x.Price,
                Tax = 0,
                Amount = x.Price * 1

            }).ToList();

            StateHasChanged(); ;
        }

        private async Task Add(int billingStatus)
        {
            Billing.SellerName = SellerName!;
            Billing.BillingStatus = billingStatus;

            var billingCreated = await _billingService!.CreateAsync(Billing);
            await SweetAlertServices.ShowSuccessAlert("Factura creada", "La factura se creó correctamente");
            DialogService!.Close(billingCreated);
        }
        private async Task Edit(int billingStatus)
        {
            Billing.SellerName = SellerName!;
            Billing.BillingStatus = billingStatus;

            await _billingService!.UpdateAsync(Billing);
            await SweetAlertServices.ShowSuccessAlert("Factura actualizada", "La factura se actualizó correctamente");
            DialogService!.Close(Billing);
        }
        private async Task OnSubmit(int billingStatus = 1)
        {
            Billing.BillingItems = BillingItems;

            if (FormMode == FormModeEnum.ADD)
            {
                await Add(billingStatus);
            }
            else
            {
                await Edit(billingStatus);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            var userAuth = await BeamAuthenticationStateProviderHelper!.GetAuthenticationStateAsync();

            SellerName = userAuth.User!.Identity!.Name;

            await LoadInventories();

            await LoadBillingsService();

            if (FormMode == FormModeEnum.EDIT)
            {
                Billing = BillingParameter;

                BillingItems = Billing.BillingItems;

                CalculateNetoAndTotalPrice();

                StateHasChanged();

                return;
            }
            else
            {
                try
                {
                    var invoiceNumber = await _billingService!.GetLastInvoiceNumber();

                    Billing.InvoiceNumber = InvoiceNumberGenerator.GenerateNumber(invoiceNumber);
                }
                catch (Exception)
                {
                    Billing.InvoiceNumber = InvoiceNumberGenerator.GenerateNumber();
                }

                CalculateNetoAndTotalPrice();

                StateHasChanged();
            }
        }
    }

    public class PaymentMethod
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
