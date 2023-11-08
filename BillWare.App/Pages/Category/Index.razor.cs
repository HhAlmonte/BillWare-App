using BillWare.App.Common;
using BillWare.App.Enum;
using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BillWare.App.Pages.Category
{
    [Authorize("Administrator, Operator")]
    public partial class Index
    {
        [Inject] private ICategoryService? _categoryService { get; set; }
        [Inject] private DialogService? DialogService { get; set; }
        [Inject] BeamAuthenticationStateProviderHelper? AuthenticationStateProvider { get; set; }


        private PaginationResult<CategoryModel> BaseResponse { get; set; } = new PaginationResult<CategoryModel>();
        private List<CategoryModel> Categories { get; set; } = new List<CategoryModel>();
        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";
        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private bool IsFiltered { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private bool IsAdmin { get; set; } = false;

        private string Search { get; set; } = "";


        private async Task OpenAddDialogForm()
        {
            var dialogResult = await DialogService!.OpenSideAsync<CategoryForm>("Registrar nueva categoría",
                    new Dictionary<string, object>
                    {
                        { "FormMode", FormModeEnum.ADD }
                    },
                    new SideDialogOptions
                    {
                        CloseDialogOnOverlayClick = true,
                        Position = DialogPosition.Right,
                        ShowMask = false
                    });

            var isLoad = dialogResult == null ? false : true;

            if (isLoad)
            {
                IsLoading = true;

                await LoadData(PageIndex, PageSize);

                IsLoading = false;

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "La categoría se ha creado correctamente. La se actualizará en breve.", SweetAlertIcon.Success);
            }
        }

        private async Task OpenEditDialogForm(CategoryModel category)
        {
            var dialogResult = await DialogService!.OpenSideAsync<CategoryForm>("Modificar información de categoría",
                    new Dictionary<string, object>
                        {
                            { "FormMode", FormModeEnum.EDIT },
                            { "CategoryParameter", category }
                        },
                    new SideDialogOptions
                    {
                        CloseDialogOnOverlayClick = true,
                        Position = DialogPosition.Right,
                        ShowMask = false
                    });

            var isLoad = dialogResult == null ? false : true;

            if (isLoad)
            {
                IsLoading = true;

                await LoadData(PageIndex, PageSize);

                IsLoading = false;

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "La categoría se ha modificado correctamente. La lista se actualizará en breve.", SweetAlertIcon.Success);
            }
        }

        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            Search = string.Empty;

            var response = await _categoryService!.GetEntitiesPagedAsync(pageIndex, pageSize);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowToastAlert("Ocurrió un error", response.Message, SweetAlertIcon.Error);
                return;
            }

            BaseResponse = response.Data!;
            Categories = BaseResponse.Items;
        }

        private async Task GetCategoriesWithSearch(string search)
        {
            IsFiltered = true;
            Search = search;

            if (Search == "")
            {
                IsFiltered = false;
                await LoadData(PageIndex, PageSize);
                return;
            }

            var response = await _categoryService!.GetEntitiesPagedWithSearchAsync(PageIndex, PageSize, Search);

            BaseResponse = response.Data!;
            Categories = BaseResponse.Items;

            if (Categories.Count <= 0)
            {
                await SweetAlertServices.ShowToastAlert("No hay registros", "No se encontraron registros", SweetAlertIcon.Warning);
            }
        }

        private async Task PageIndexChanged(int pageIndex)
        {
            PageIndex = pageIndex + 1;

            if (IsFiltered)
            {
                await GetCategoriesWithSearch(Search);
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
                await GetCategoriesWithSearch(Search);
            }
            else
            {
                await LoadData(PageIndex, PageSize);
            }
        }

        private async Task DeleteCategory(int id)
        {
            var isConfirmed = await SweetAlertServices.ShowWarningAlert("¿Estás seguro de eliminar este registro?", "Verifica que este registro sea el que quieres eliminar");

            if (isConfirmed)
            {
                var response = await _categoryService!.DeleteAsync(id);

                if (!response.IsSuccessFull)
                {
                    await SweetAlertServices.ShowToastAlert(response.Message, response.Details!, SweetAlertIcon.Error);
                    return;
                }

                if (Categories.Count == 1 && PageIndex != 1)
                {
                    PageIndex -= 1;
                    await LoadData(PageIndex, PageSize);

                    return;
                }

                await LoadData(PageIndex, PageSize);

                await SweetAlertServices.ShowToastAlert("Registro eliminado", "La categoría se eliminó correctamente", SweetAlertIcon.Success);
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
    }
}
