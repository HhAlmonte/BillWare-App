using BillWare.App.Common;
using BillWare.App.Enum;
using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

namespace BillWare.App.Pages.Costumer
{
    [Authorize("Administrator, Operator")]
    public partial class Index
    {
        [Inject] ICostumerService _costumerService { get; set; }
        [Inject] public DialogService DialogService { get; set; }
        [Inject] BeamAuthenticationStateProviderHelper BeamAuthenticationStateProviderHelper { get; set; }

        private PaginationResult<CostumerModel> PaginationResult { get; set; } = new PaginationResult<CostumerModel>();
        private List<CostumerModel> Costumers { get; set; } = new List<CostumerModel>();
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

                var response = await _costumerService.GetEntitiesPagedAsync(pageIndex, pageSize);

                PaginationResult = response.Data;

                Costumers = PaginationResult.Items;
            }
            catch (Exception)
            {
                await SweetAlertServices.ShowErrorAlert("Error", "Error al cargar los datos. Favor de contactar con el administrador del sistema.");
            }
        }

        private async Task OpenAddDialogForm(string title)
        {
            var dialogResponse = await DialogService.OpenAsync<CostumerForm>(title,
                    new Dictionary<string, object>
                    {
                        { "FormMode", FormModeEnum.ADD }
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

                IsLoading = false;
            }
        }

        private async Task OpenEditDialogForm(string title, CostumerModel costumer)
        {
            var dialogResponse = await DialogService.OpenAsync<CostumerForm>(title,
                    new Dictionary<string, object>
                    {
                        { "FormMode", FormModeEnum.EDIT },
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
            }
            else
            {
                var response = await _costumerService.GetEntitiesPagedWithSearchAsync(PageIndex, PageSize, search);

                PaginationResult = response.Data;

                Costumers = PaginationResult.Items;
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

        private async Task DeleteCostumer(int id)
        {
            try
            {
                var isConfirmed = await SweetAlertServices.ShowWarningAlert("¿Estás seguro de eliminar este registro?", "Verifica que este registro sea el que quieres eliminar");

                if (isConfirmed)
                {
                    var response = await _costumerService.DeleteAsync(id);

                    if (!response.IsSuccessFul)
                    {
                        await SweetAlertServices.ShowErrorAlert("Error", response.Message);
                        return;
                    }

                    if (Costumers.Count == 1 && PageIndex != 1)
                    {
                        PageIndex -= 1;

                        await LoadData(PageIndex, PageSize);
                    }
                    else
                    {
                        await LoadData(PageIndex, PageSize);
                    }
                }
            }
            catch (Exception)
            {
                await SweetAlertServices.ShowErrorAlert("Error", "Se producieron algunos errrores internos. Favor de reportar con el administrador");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            var userAuth = await BeamAuthenticationStateProviderHelper.GetAuthenticationStateAsync();

            IsAdmin = userAuth.User.IsInRole("Administrator");

            await LoadData(PageIndex);

            IsLoading = false;
        }
    }
}
