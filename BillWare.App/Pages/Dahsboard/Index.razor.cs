using BillWare.App.Common;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using BlazorBootstrap;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace BillWare.App.Pages.Dahsboard
{
    [Authorize(Roles = "Administrator")]
    public partial class Index
    {
        [Inject] private IDashboardService _dashboardService { get; set; } = null!;
        [Inject] private IBillingService _billingService { get; set; } = null!;

        private PaginationResult<BillingModel> BaseResponse { get; set; } = new PaginationResult<BillingModel>();
        private BillingsParamsModel BillingsParamsModel { get; set; } = new BillingsParamsModel();
        private RadzenDataGrid<BillingDataGridDetails>? gridDetails;

        private List<BillingModel> Billings { get; set; } = new List<BillingModel>();
        private List<StatisticsModel> SalesLast30Days { get; set; } = new List<StatisticsModel>();
        private List<StatisticsModel> SalesLast12Month { get; set; } = new List<StatisticsModel>();
        private List<BillingDataGridDetails> BillingDataGridDetails { get; set; } = new List<BillingDataGridDetails>();
        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private bool IsLoading { get; set; } = false;
        private bool IsDateFiltered { get; set; } = false;
        private bool IsFiltered { get; set; } = false;

        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private string Search { get; set; } = string.Empty;
        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";

        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            var response = await _billingService.GetEntitiesPagedAsync(pageIndex, pageSize);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowToastAlert(response.Message, response.Details!, SweetAlertIcon.Error);
                return;
            }

            BaseResponse = response.Data!;
            Billings = BaseResponse.Items;
        }
        private async Task GetBillingsWithSearch(string search)
        {
            IsFiltered = true;
            Search = search;
            BillingDataGridDetails.Clear();

            if (Search == "")
            {
                IsFiltered = false;
                await LoadData(PageIndex, PageSize);
                await LoadDataGridDetails();
                StateHasChanged();
            }
            else
            {
                var response = await _billingService.GetEntitiesPagedWithSearchAsync(PageIndex, PageSize, Search);
                BaseResponse = response.Data!;
                Billings = BaseResponse.Items;
                await LoadDataGridDetails();
                StateHasChanged();
            }
        }
        private async Task LoadWithParams(int pageIndex = 1, int pageSize = 10)
        {
            BillingsParamsModel.PageIndex = pageIndex;
            BillingsParamsModel.PageSize = pageSize;

            BaseResponse = await _billingService.GetBillingsWithParams(BillingsParamsModel);
            Billings = BaseResponse.Items;
            BillingDataGridDetails.Clear();
            await LoadDataGridDetails();
            IsDateFiltered = true;
        }
        private async Task LoadSalesLast30Days()
        {
            var salesData = await _dashboardService.GetSalesLast30Days();
            SalesLast30Days = salesData;
        }
        private async Task LoadSalesLast12Month()
        {
            var salesData = await _dashboardService.GetSalesLast12Month();
            SalesLast12Month = salesData;
        }

        private async Task PageIndexChanged(int pageIndex)
        {
            PageIndex = pageIndex + 1;

            if (IsDateFiltered)
            {
                await LoadWithParams(PageIndex, PageSize);
            }
            else
            {
                if (IsFiltered)
                {
                    await GetBillingsWithSearch(Search);
                }
                else
                {
                    await LoadData(PageIndex, PageSize);
                }
            }
        }
        private async Task PageSizeChanged(int pageSize)
        {
            PageSize = pageSize;

            if (IsFiltered)
            {
                await GetBillingsWithSearch(Search);
            }
            else
            {
                await LoadData(PageIndex, PageSize);
            }
        }
        private async Task LoadDataGridDetails()
        {
            if(Billings.Count == 0)
            {
                return;
            }

            var totalAmount = Billings.Sum(x => x.TotalPriceWithTax);

            BillingDataGridDetails.Add(new BillingDataGridDetails
            {
                Amount = totalAmount,
                Currency = "DOP"
            });

            await gridDetails.Reload();
        }

        private string FormatAsMonth(object value)
        {
            if (value != null)
            {
                return Convert.ToDateTime(value).ToString("MMM");
            }

            return string.Empty;
        }
        private string FormatAsUSD(object value)
        {
            return ((double)value).ToString("C0", System.Globalization.CultureInfo.CreateSpecificCulture("en-US"));
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            var loadDataTask = LoadData(PageIndex);
            var loadSalesLast30DaysTask = LoadSalesLast30Days();
            var loadSalesLast12MonthTask = LoadSalesLast12Month();
            var loadDataGridDetailsTask = LoadDataGridDetails();

            await Task.WhenAll(loadDataTask, loadSalesLast30DaysTask, loadSalesLast12MonthTask, loadDataGridDetailsTask);

            IsLoading = false;
            StateHasChanged();
        }
    }
}
