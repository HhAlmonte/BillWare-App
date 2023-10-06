using BillWare.App.Common;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Radzen;

namespace BillWare.App.Pages.User
{
    [Authorize("Administrator")]
    public partial class Index
    {
        private PaginationResult<UserModel> BaseResponse = new PaginationResult<UserModel>();
        private List<UserModel> Users = new List<UserModel>();

        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";
        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private bool IsLoading { get; set; } = false;

        private async Task OpenAddDialogForm(string title)
        {
            var action = await DialogService.OpenAsync<UserForm>(title,
            options: new DialogOptions
            {
                Width = "700px",
                Draggable = true,
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
        private async Task OpenEditDialogForm(UserModel user)
        {
            var action = await DialogService.OpenAsync<UserForm>("Editar usuario",
            parameters: new Dictionary<string, object>() { { "UserParameter", user }, { "FormMode", Common.FormModeEnum.EDIT } },
            options: new DialogOptions
            {
                Width = "700px",
                Draggable = true,
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
            try
            {
                BaseResponse = await _userService.GetUsersPaged(pageIndex, pageSize);
                Users = BaseResponse.Items;
            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new PaginationResult<UserModel>();
                Users = new List<UserModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new PaginationResult<UserModel>();
                Users = new List<UserModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task DeleteUser(string id)
        {
            var result = await SweetAlertServices.ShowWarningAlert("¿Está seguro de eliminar este usuario?", "Esta acción no se puede deshacer");

            if (result)
            {
                try
                {
                    await _userService.DeleteUser(id);

                    await SweetAlertServices.ShowSuccessAlert("Usuario eliminado", "El usuario se eliminó correctamente");

                    await LoadData(PageIndex, PageSize);
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
            await LoadData(PageIndex);

            await Task.Delay(1000);
            IsLoading = false;
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
