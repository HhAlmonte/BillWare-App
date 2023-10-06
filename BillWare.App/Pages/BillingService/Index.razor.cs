using BillWare.App.Common;
using BillWare.App.Enum;
using BillWare.App.Models;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.BillingService
{
    public partial class Index
    {
        [Inject] private LocalStorageHelper LocalStorageService { get; set; }

        private PaginationResult<BillingServiceModel> BaseResponse { get; set; } = new PaginationResult<BillingServiceModel>();
        private List<BillingServiceModel> BillingsServices { get; set; } = new List<BillingServiceModel>();
        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";
        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private bool IsFiltered { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private bool IsAdmin { get; set; } = false;
        private string Search { get; set; } = "";

        private async Task OpenAddDialogForm(string title)
        {
            var action = await DialogService.OpenAsync<BillingServiceForm>(title,
            new Dictionary<string, object>
                    {
            { "FormMode", FormModeEnum.ADD }
                    },
            new DialogOptions
            {
                Width = "auto"
            });

            var isLoad = action == null ? false : true;

            if (isLoad)
            {
                IsLoading = true;
                await LoadData(PageIndex, PageSize);

                await Task.Delay(1000);
                IsLoading = false;
            }
        }
        private async Task OpenEditDialogForm(string title, BillingServiceModel billingService)
        {
            var action = await DialogService.OpenAsync<BillingServiceForm>(title,
            new Dictionary<string, object>
                            {
                    { "FormMode", FormModeEnum.EDIT },
                    { "BillingServiceParameter", billingService }
                            },
            new DialogOptions
            {
                Width = "auto",
            });

            var isLoad = action == null ? false : true;

            if (isLoad)
            {
                IsLoading = true;
                await LoadData(PageIndex, PageSize);

                await Task.Delay(1000);
                IsLoading = false;
            }
        }

        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            Search = string.Empty;

            try
            {
                BaseResponse = await _billingService.GetBillingsServices(pageIndex, pageSize);
                BillingsServices = BaseResponse.Items;
            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new PaginationResult<BillingServiceModel>();
                BillingsServices = new List<BillingServiceModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new PaginationResult<BillingServiceModel>();
                BillingsServices = new List<BillingServiceModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }
        private async Task GetWithSearch(string search)
        {
            try
            {
                IsFiltered = true;
                Search = search;

                if (Search == "")
                {
                    IsFiltered = false;
                    await LoadData(PageIndex, PageSize);
                    return;
                }
                else
                {
                    BaseResponse = await _billingService.GetBillingsServicesWithSearch(search, PageIndex, PageSize);
                    BillingsServices = BaseResponse.Items;
                }

            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new PaginationResult<BillingServiceModel>();
                BillingsServices = new List<BillingServiceModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new PaginationResult<BillingServiceModel>();
                BillingsServices = new List<BillingServiceModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }

        private async Task PageIndexChanged(int pageIndex)
        {
            PageIndex = pageIndex + 1;

            if (IsFiltered)
            {
                await GetWithSearch(Search);
            }
            else
            {
                await LoadData(PageIndex, PageSize);
            }
        }
        private async Task PageSizeChanged(int pageSize)
        {
            PageSize = pageSize;

            if (IsFiltered)
            {
                await GetWithSearch(Search);
            }
            else
            {
                await LoadData(PageIndex, PageSize);
            }
        }
        private async Task Delete(int id)
        {
            var isConfirmed = await SweetAlertServices.ShowWarningAlert("¿Estás seguro de eliminar este registro?", "Verifica que este registro sea el que quieres eliminar");

            if (isConfirmed)
            {
                try
                {
                    var response = await _billingService.DeleteBillingService(id);

                    if (BillingsServices.Count == 1 && PageIndex != 1)
                    {
                        PageIndex -= 1;
                        await LoadData(PageIndex, PageSize);
                    }
                    else
                    {
                        await LoadData(PageIndex, PageSize);
                    }

                    await SweetAlertServices.ShowSuccessAlert("Registro eliminado", "El registro se eliminó correctamente");
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
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;
            IsAdmin = await LocalStorageService.GetItem(Configuration.ROLE) == "Administrator" ? true : false;

            await LoadData(PageIndex);
            await Task.Delay(1000);

            IsLoading = false;
        }
    }
}
