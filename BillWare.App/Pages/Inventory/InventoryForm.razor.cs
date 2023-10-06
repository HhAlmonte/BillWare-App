using BillWare.App.Enum;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BillWare.App.Pages.Inventory
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class InventoryForm
    {
        [Parameter] public FormModeEnum FormMode { get; set; }

        [Parameter] public Models.InventoryModel InventoryParameter { get; set; } = new Models.InventoryModel();

        private Models.InventoryModel Inventory = new Models.InventoryModel();
        private List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        private string ButtonTitle => FormMode == FormModeEnum.ADD ? "Agregar" : "Editar";

        private async Task OnSubmit()
        {
            if (FormMode == FormModeEnum.ADD)
            {
                await Add();
            }
            else
            {
                await Edit();
            }
        }

        private async Task Add()
        {
            try
            {
                var response = await _inventoryService.CreateInvetory(Inventory);
                var closeReturn = response != null ? true : false;
                DialogService.Close(closeReturn);
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
                var response = await _inventoryService.EditInventory(Inventory);
                var closeReturn = response != null ? true : false;
                DialogService.Close(closeReturn);
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

        private async Task LoadCategories()
        {
            try
            {
                var response = await _categoryService.GetCategoriesPaged(1, 100);
                Categories = response.Items;
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Categories = new List<CategoryModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                Categories = new List<CategoryModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }

        private async Task LoadCategoriesWithSearch(string searchText)
        {
            try
            {
                var response = await _categoryService.GetCategoriesPagedWithSearch(1, 100, searchText);
                Categories = response.Items;
                StateHasChanged();
            }
            catch (HttpRequestException ex)
            {
                Categories = new List<CategoryModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                Categories = new List<CategoryModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadCategories();

            if (FormMode == FormModeEnum.EDIT)
            {
                Inventory = InventoryParameter;
            }
        }
    }
}
