using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;

namespace BillWare.App.Pages.Category
{
    [Authorize("Administrator, Operator")]
    public partial class CategoryForm
    {
        [Parameter] public Common.FormMode FormMode { get; set; }
        [Parameter] public CategoryModel CategoryParameter { get; set; } = new CategoryModel();


        private CategoryModel Category = new CategoryModel();

        private string ButtonTitle => FormMode == Common.FormMode.ADD ? "Agregar" : "Editar";

        private async Task OnSubmit()
        {
            if (FormMode == Common.FormMode.ADD)
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
                var response = await _categoryService.CreateCategory(Category);

                var closeReturn = response != null ? true : false;

                DialogService.Close(response);
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
                var response = await _categoryService.EditCategory(Category);

                var closeReturn = response != null ? true : false;

                DialogService.Close(response);
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

        protected override void OnInitialized()
        {
            if (FormMode == Common.FormMode.EDIT)
            {
                Category = CategoryParameter;
            }
        }
    }
}
