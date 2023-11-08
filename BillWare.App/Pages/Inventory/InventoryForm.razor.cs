using BillWare.App.Enum;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Inventory
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class InventoryForm
    {
        [Inject] private IInventoryService? _inventoryService { get; set; }
        [Inject] private ICategoryService? _categoryService { get; set; }
        [Inject] private DialogService? DialogService { get; set; }

        [Parameter] public FormModeEnum FormMode { get; set; }
        [Parameter] public InventoryModel InventoryParameter { get; set; } = new InventoryModel();

        private InventoryModel Inventory = new InventoryModel();
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
            var response = await _inventoryService!.CreateAsync(Inventory);

            if(!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            var closeReturn = response != null ? true : false;
            DialogService!.CloseSide(closeReturn);
        }

        private async Task Edit()
        {
            var response = await _inventoryService!.UpdateAsync(Inventory);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            var closeReturn = response != null ? true : false;
            DialogService!.CloseSide(closeReturn);
        }

        private async Task LoadCategories()
        {
            var response = await _categoryService!.GetEntitiesPagedAsync(1, 100);

            Categories = response.Data!.Items;

            StateHasChanged();
        }

        private async Task LoadCategoriesWithSearch(string searchText)
        {
            var response = await _categoryService!.GetEntitiesPagedWithSearchAsync(1, 100, searchText);

            Categories = response.Data!.Items;

            StateHasChanged();
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
