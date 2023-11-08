using BillWare.App.Common;
using BillWare.App.Enum;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.User
{
    [Authorize("Administrator")]
    public partial class Index
    {
        [Inject] private IUserService? _userService { get; set; }
        [Inject] private DialogService? DialogService { get; set; }

        private PaginationResult<UserModel> BaseResponse = new PaginationResult<UserModel>();
        private List<UserModel> Users = new List<UserModel>();

        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";
        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private bool IsLoading { get; set; } = false;

        private async Task OpenAddDialogForm(string title)
        {
            var dialogResult = await DialogService!.OpenSideAsync<UserForm>(title,
                      options: new SideDialogOptions
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

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "El usuario se ha creado correctamente. La lista se actualizará en breve.", SweetAlertIcon.Success);
            }
        }
        private async Task OpenEditDialogForm(UserModel user)
        {
            var dialogResult = await DialogService!.OpenSideAsync<UserForm>("Modificar datos de usuario",
                    parameters: new Dictionary<string, object>()
                    {
                        { "UserParameter", user },
                        { "FormMode", FormModeEnum.EDIT }
                    },
                    options: new SideDialogOptions
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

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "El cliente se ha actualizado correctamente. La lista se actualizará en breve.", SweetAlertIcon.Success);
            }
        }
        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            var response = await _userService!.GetEntitiesPagedAsync(pageIndex, pageSize);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowToastAlert("Ocurrió un error", response.Message, SweetAlertIcon.Error);
                return;
            }

            BaseResponse = response.Data!;
            Users = BaseResponse.Items;
        }
        private async Task DeleteUser(string id)
        {
            var isConfirmed = await SweetAlertServices.ShowWarningAlert("¿Está seguro de eliminar este usuario?", "Esta acción no se puede deshacer");

            if (isConfirmed)
            {
                var response = await _userService!.DeleteAsync(id);

                if (!response.IsSuccessFull)
                {
                    await SweetAlertServices.ShowToastAlert(response.Message, response.Details!, SweetAlertIcon.Error);
                    return;
                }

                if (Users.Count == 1 && PageIndex != 1)
                {
                    PageIndex -= 1;
                    await LoadData(PageIndex, PageSize);
                    return;
                }

                await LoadData(PageIndex, PageSize);

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "El usuario se ha eliminado correctamente. La lista se actualizará en breve.");
            }
        }
        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

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

            await LoadData(PageIndex, PageSize);
        }
        private async Task PageSizeChanged(int pageSize)
        {
            PageSize = pageSize;

            await LoadData(PageIndex, PageSize);
        }
    }
}
