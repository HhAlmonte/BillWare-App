using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Inventory
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class Index
    {
        [Inject] LocalStorageService LocalStorageService { get; set; }

        private BaseResponseModel<Models.Inventory> BaseResponse { get; set; } = new BaseResponseModel<Models.Inventory>();
        private List<Models.Inventory> Inventories { get; set; } = new List<Models.Inventory>();
        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private bool IsLoading { get; set; } = false;
        private bool IsFiltered { get; set; } = false;
        private bool IsAdmin { get; set; } = false;

        private string Search { get; set; } = "";
        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";

        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            try
            {
                Search = string.Empty;
                BaseResponse = await _invetoryService.GetInventories(pageIndex, pageSize);
                Inventories = BaseResponse.Items;
            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new BaseResponseModel<Models.Inventory>();
                Inventories = new List<Models.Inventory>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new BaseResponseModel<Models.Inventory>();
                Inventories = new List<Models.Inventory>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }

        private async Task GetWithSearch(string search)
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
                BaseResponse = await _invetoryService.GetInventoryWithSearch(search, PageIndex, PageSize);
                Inventories = BaseResponse.Items;
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
                    await _invetoryService.DeleteInventory(id);
                    await SweetAlertServices.ShowSuccessAlert("Registro eliminado", "El registro se eliminó correctamente");
                    if (Inventories.Count == 1 && PageIndex != 1)
                    {
                        PageIndex -= 1;
                        await LoadData(PageIndex, PageSize);
                    }
                    else
                    {
                        await LoadData(PageIndex, PageSize);
                    }
                }
                catch (HttpRequestException ex)
                {
                    await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
                }
                catch (Exception ex)
                {
                    await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
                }
                finally
                {
                    await Task.Delay(1000);
                    IsLoading = false;
                }
            }
        }

        private async Task OpenAddDialogForm(string title)
        {
            var dialogResponse = await DialogService.OpenAsync<InventoryForm>(title,
            new Dictionary<string, object>
                {
                    { "FormMode", Common.FormMode.ADD }
                },
            new DialogOptions
                {
                    Width = "auto"
                });

            var isLoad = dialogResponse == null ? false : true;

            if (isLoad)
            {
                IsLoading = true;
                await LoadData(PageIndex, PageSize);

                await Task.Delay(1000);
                IsLoading = false;
            }
        }

        private async Task OpenEditDialogForm(string title, Models.Inventory inventory)
        {
            var action = await DialogService.OpenAsync<InventoryForm>(title,
            new Dictionary<string, object>
                {
                    { "FormMode", Common.FormMode.EDIT },
                    { "InventoryParameter", inventory }
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
    }
}
