using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Radzen;

namespace BillWare.App.Pages.Billing
{
    public partial class BillingForm
    {
        [Parameter] public BillingModel BillingParameter { get; set; } = new BillingModel();
        [Parameter] public Common.FormMode FormMode { get; set; } = Common.FormMode.ADD;

        private BillingModel Billing = new BillingModel()
        {
            CreatedAt = DateTime.Now
        };
        private RadzenDataGrid<BillingItemModel> grid;
        private Common.InvoiceNumberGenerator InvoiceNumberGenerator = new Common.InvoiceNumberGenerator("FACT");

        private List<BillingItemModel> BillingItems { get; set; } = new List<BillingItemModel>();
        private List<BillingServiceModel> BillingsServices { get; set; } = new List<BillingServiceModel>();
        private List<BillingServiceModel> BillingsServicesSelected { get; set; } = new List<BillingServiceModel>();

        public string ReturnMoney { get; set; } = "0";
        private string CostumerId { get; set; }

        private async Task GetCostumer()
        {
            if (string.IsNullOrEmpty(CostumerId))
            {
                await SweetAlertServices.ShowErrorAlert("Codigo invalido", "Primero debe ingresar un código");
            }
            else
            {
                int costumerId = Convert.ToInt32(CostumerId);

                var costumer = await _costumerService.GetCostumerById(costumerId);

                if (costumer == null)
                {
                    await SweetAlertServices.ShowErrorAlert("Error", "No se encontró ningun cliente con ese código");
                }
                else
                {
                    Billing.FullName = costumer.FullName;
                    Billing.Address = costumer.Address;
                    Billing.Phone = costumer.Phone;
                    Billing.NumberId = costumer.NumberId;
                }

                StateHasChanged();
            }
        }
        private async Task LoadInventories()
        {
            var data = await _inventoryService.GetInventories(1, 50);

            Inventories = data.Items;

            StateHasChanged();
        }
        private async Task LoadBillingsService()
        {
            var data = await _billingServiceService.GetBillingsServices(1, 50);

            BillingsServices = data.Items;

            StateHasChanged();
        }

        private async Task OpenInventoryFormDialog(BillingItemModel billingItem = null)
        {
            var dialog = await DialogService.OpenAsync<BillingItemForm>("Agregar producto"
                            , parameters: new Dictionary<string, object>
                            {
                            { "BillingItemParameter", billingItem }
                            }
                            , options: new DialogOptions
                            {
                                Width = "auto"
                            });
            if (dialog != null)
            {
                var billingItemModel = dialog as BillingItemModel;

                if (billingItem == null)
                {
                    BillingItems.Add(billingItemModel);

                    foreach (var i in BillingItems)
                    {
                        i.Amount = i.Quantity * i.Price;
                        i.Tax = CalcularITBIS((decimal)i.Quantity * (decimal)i.Price);
                    }

                    CalculateNetoAndTotalPrice();

                    await grid.Reload();
                }
                else
                {
                    var index = BillingItems.FindIndex(x => x.Code == billingItemModel.Code);

                    BillingItems[index] = billingItemModel;

                    foreach (var i in BillingItems)
                    {
                        i.Amount = i.Quantity * i.Price;
                        i.Tax = CalcularITBIS((decimal)i.Quantity * (decimal)i.Price);
                    }

                    CalculateNetoAndTotalPrice();

                    await grid.Reload();
                }
                StateHasChanged();
            }
        }
        private async Task DeleteBillingItem(BillingItemModel billingItem)
        {
            if (FormMode == Common.FormMode.EDIT)
            {
                await _billingItemService.DeleteBillingItem(billingItem.Id);
            }
            else
            {
                var index = BillingItems.FindIndex(x => x.Code == billingItem.Code);

                BillingItems.RemoveAt(index);
            }

            CalculateNetoAndTotalPrice();

            await grid.Reload();
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
        private async Task GetInventoryBySearch(string searchText)
        {
            var result = await _inventoryService.GetInventoryWithSearch(searchText, 1, 100);

            Inventories = result.Items;
        }
        private async Task GetBillingsServicesWithSearch(string searchText)
        {
            var result = await _billingServiceService.GetBillingsServicesWithSearch(searchText, 1, 100);

            BillingsServices = result.Items;
        }
        private void OnInventorySelected(object inventorySelected)
        {
            InventoriesSelected = (List<Models.Inventory>)inventorySelected;

            foreach (var i in InventoriesSelected)
            {
                var quantity = 2;
                var amount = (int)i.Price * quantity;

                BillingItems.Add(new BillingItemModel
                {
                    Code = i.Id,
                    Description = i.Name,
                    Quantity = quantity,
                    Price = i.Price,

                    Amount = amount,

                    Tax = CalcularITBIS(amount)
                });
            }

            CalculateNetoAndTotalPrice();

            StateHasChanged();

            grid.Reload();
        }
        private void OnBillingServiceSelected(object billingServiceSelected)
        {
            BillingsServicesSelected = (List<BillingServiceModel>)billingServiceSelected;

            foreach (var i in BillingsServicesSelected)
            {
                var quantity = 1;
                var amount = (int)i.Price * quantity;

                BillingItems.Add(new BillingItemModel
                {
                    Code = i.Id,
                    Description = i.Name,
                    Quantity = quantity,
                    Price = i.Price,

                    Amount = amount,

                    Tax = CalcularITBIS(amount)
                });
            }

            CalculateNetoAndTotalPrice();

            StateHasChanged();

            grid.Reload();
        }
        private async Task Add()
        {
            try
            {
                var billingCreated = await _billingService.CreateBilling(Billing);
                await SweetAlertServices.ShowSuccessAlert("Factura creada", "La factura se creó correctamente");
                DialogService.Close(billingCreated);
            }
            catch (HttpRequestException ex)
            {
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }
        private async Task Edit()
        {
            try
            {
                await _billingService.UpdateBilling(Billing);
                await SweetAlertServices.ShowSuccessAlert("Factura actualizada", "La factura se actualizó correctamente");
                DialogService.Close(Billing);
            }
            catch (HttpRequestException ex)
            {
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }
        private async Task OnSubmit()
        {
            Billing.BillingItems = BillingItems;

            if (FormMode == Common.FormMode.ADD)
            {
                await Add();
            }
            else
            {
                await Edit();
            }
        }

        protected override async void OnInitialized()
        {
            await LoadInventories();

            await LoadBillingsService();

            if (FormMode == Common.FormMode.EDIT)
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
                    var invoiceNumber = await _billingService.GetLastInvoiceNumber();

                    Billing.InvoiceNumber = InvoiceNumberGenerator.GenerateNumber(invoiceNumber);
                }
                catch (Exception)
                {
                    Billing.InvoiceNumber = InvoiceNumberGenerator.GenerateNumber();
                }

                CalculateNetoAndTotalPrice();

                StateHasChanged();
            }

            base.OnInitialized();
        }
    }
}
