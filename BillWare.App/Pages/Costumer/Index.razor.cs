using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Costumer
{
    [Authorize("Administrator, Operator")]
    public partial class Index
    {
        [Inject] LocalStorageService LocalStorageService { get; set; }

        private BaseResponseModel<Models.Costumer> BaseResponse { get; set; } = new BaseResponseModel<Models.Costumer>();
        private List<Models.Costumer> Costumers { get; set; } = new List<Models.Costumer>();
        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";
        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private bool IsFiltered { get; set; } = false;
        private bool IsLoading { get; set; } = false;
        private bool IsAdmin { get; set; } = false;

        private string Search { get; set; } = "";


        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            try
            {
                Search = string.Empty;
                BaseResponse = await _costumerService.GetCostumersPaged(pageIndex, pageSize);
                Costumers = BaseResponse.Items;
            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new BaseResponseModel<Models.Costumer>();
                Costumers = new List<Models.Costumer>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new BaseResponseModel<Models.Costumer>();
                Costumers = new List<Models.Costumer>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task OpenAddDialogForm(string title)
        {
            var dialogResponse = await DialogService.OpenAsync<CostumerForm>(title,
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

        private async Task OpenEditDialogForm(string title, Models.Costumer costumer)
        {
            var dialogResponse = await DialogService.OpenAsync<CostumerForm>(title,
            new Dictionary<string, object>
                    {
                        { "FormMode", Common.FormMode.EDIT },
                        { "CostumerParameter", costumer }
                    },
            new DialogOptions
            {
                Width = "auto",
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
                BaseResponse = await _costumerService.GetCostumersPagedWithSearch(PageIndex, PageSize, search);
                Costumers = BaseResponse.Items;
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
                    await _costumerService.DeleteCostumer(id);

                    if (Costumers.Count == 1 && PageIndex != 1)
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
