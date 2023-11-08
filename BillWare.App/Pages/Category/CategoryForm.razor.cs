using BillWare.App.Enum;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Category
{
    [Authorize("Administrator, Operator")]
    public partial class CategoryForm
    {
        [Inject] private ICategoryService? _categoryService { get; set; }
        [Inject] private DialogService? DialogService { get; set; }

        [Parameter] public FormModeEnum FormMode { get; set; }
        [Parameter] public CategoryModel CategoryParameter { get; set; } = new CategoryModel();


        private CategoryModel Category = new CategoryModel();

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
            var response = await _categoryService!.CreateAsync(Category);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            var closeReturn = response != null ? true : false;

            DialogService!.CloseSide(closeReturn);
        }

        private async Task Edit()
        {
            var response = await _categoryService!.UpdateAsync(Category);

            var closeReturn = response != null ? true : false;

            if (response!.IsSuccessFull)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            DialogService?.CloseSide(closeReturn);
        }

        protected override void OnInitialized()
        {
            if (FormMode == FormModeEnum.EDIT)
            {
                Category = CategoryParameter;
            }
        }
    }
}
