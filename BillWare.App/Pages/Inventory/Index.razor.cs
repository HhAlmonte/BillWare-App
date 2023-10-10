using BillWare.App.Common;
using BillWare.App.Enum;
using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Inventory
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class Index
    {
        [Inject] private IInventoryService? _invetoryService { get; set; }
        [Inject] private DialogService? DialogService { get; set; }
        [Inject] BeamAuthenticationStateProviderHelper? AuthenticationStateProvider { get; set; }

        private PaginationResult<InventoryModel> BaseResponse { get; set; } = new PaginationResult<InventoryModel>();
        private List<InventoryModel> Inventories { get; set; } = new List<InventoryModel>();
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
            Search = string.Empty;

            var response = await _invetoryService!.GetEntitiesPagedAsync(pageIndex, pageSize);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowToastAlert("Ocurrió un error", response.Message, SweetAlertIcon.Error);
                return;
            }

            BaseResponse = response.Data!;
            Inventories = BaseResponse.Items;
        }

        private async Task GetInventoriesWithSearch(string search)
        {
            IsFiltered = true;
            Search = search;

            if (Search == "")
            {
                IsFiltered = false;
                await LoadData(PageIndex, PageSize);
                return;
            }

            var response = await _invetoryService!.GetEntitiesPagedWithSearchAsync(PageIndex, PageSize, search);

            BaseResponse = response.Data!;
            Inventories = BaseResponse.Items;

            if (Inventories.Count <= 0)
            {
                await SweetAlertServices.ShowToastAlert("No hay registros", "No se encontraron registros", SweetAlertIcon.Warning);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            var authState = await AuthenticationStateProvider!.GetAuthenticationStateAsync();

            IsAdmin = authState.User.IsInRole("Administrator");

            await LoadData(PageIndex);

            IsLoading = false;

            if (BaseResponse.Items.Count <= 0)
            {
                await SweetAlertServices.ShowToastAlert("No hay registros", "No se encontraron registros", SweetAlertIcon.Warning);
            }
        }

        private async Task PageIndexChanged(int pageIndex)
        {
            PageIndex = pageIndex + 1;

            if (IsFiltered)
            {
                await GetInventoriesWithSearch(Search);
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
                await GetInventoriesWithSearch(Search);
            }
            else
            {
                await LoadData(PageIndex, PageSize);
            }
        }

        private async Task DeleteInventory(int id)
        {
            var isConfirmed = await SweetAlertServices.ShowWarningAlert("¿Estás seguro de eliminar este registro?", "Verifica que este registro sea el que quieres eliminar");

            if (isConfirmed)
            {
                var response = await _invetoryService!.DeleteAsync(id);

                if (!response.IsSuccessFull)
                {
                    await SweetAlertServices.ShowToastAlert(response.Message, response.Details!, SweetAlertIcon.Error);
                    return;
                }

                if (Inventories.Count == 1 && PageIndex != 1)
                {
                    PageIndex -= 1;
                    await LoadData(PageIndex, PageSize);

                    return;
                }

                await LoadData(PageIndex, PageSize);

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "El servicio se ha eliminado correctamente. Se actualizará la lista en breve.", SweetAlertIcon.Success);
            }
        }

        private async Task OpenAddDialogForm(string title)
        {
            var dialogResult = await DialogService!.OpenAsync<InventoryForm>(title,
                      new Dictionary<string, object>
                      {
                          { "FormMode", FormModeEnum.ADD }
                      },
                      new DialogOptions
                      {
                          Width = "auto"
                      });

            var isLoad = dialogResult == null ? false : true;

            if (isLoad)
            {
                IsLoading = true;

                await LoadData(PageIndex, PageSize);

                IsLoading = false;

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "El item se ha creado correctamente. Se actualizará en breve.", SweetAlertIcon.Success);
            }
        }

        private async Task OpenEditDialogForm(string title, InventoryModel inventory)
        {
            var dialogResult = await DialogService!.OpenAsync<InventoryForm>(title,
                      new Dictionary<string, object>
                      {
                          { "FormMode", FormModeEnum.EDIT },
                          { "InventoryParameter", inventory }
                      },
                      new DialogOptions
                      {
                          Width = "auto",
                      });

            var isLoad = dialogResult == null ? false : true;

            if (isLoad)
            {
                IsLoading = true;

                await LoadData(PageIndex, PageSize);

                IsLoading = false;

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "El item se ha actualizado correctamente. Se actualizará la lista en breve.", SweetAlertIcon.Success);
            }
        }
    }
}
