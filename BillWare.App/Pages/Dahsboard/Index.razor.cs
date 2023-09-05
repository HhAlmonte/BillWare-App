using BillWare.App.Models;
using BillWare.Application.Billing.Models;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace BillWare.App.Pages.Dahsboard
{
    public partial class Index
    {
        [Inject] NavigationManager NavigationManager { get; set; }

        private BaseResponseModel<BillingModel> BaseResponse { get; set; } = new BaseResponseModel<BillingModel>();
        private BillingsParamsModel BillingsParamsModel { get; set; } = new BillingsParamsModel();
        private RadzenDataGrid<BillingItemModel> grid;
        private RadzenDataGrid<BillingDataGridDetails> gridDetails;

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
            try
            {
                BaseResponse = await _billingService.GetBilling(pageIndex, pageSize);
                Billings = BaseResponse.Items;
            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new BaseResponseModel<BillingModel>();
                Billings = new List<BillingModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new BaseResponseModel<BillingModel>();
                Billings = new List<BillingModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }

            BaseResponse = await _billingService.GetBilling(pageIndex, pageSize);
            Billings = BaseResponse.Items;
        }
        private async Task GetWithSearch(string search)
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
                BaseResponse = await _billingService.GetBillingWithSearch(Search, PageIndex, PageSize);
                Billings = BaseResponse.Items;
                await LoadDataGridDetails();
                StateHasChanged();
            }
        }
        private async Task LoadWithParams(int pageIndex = 1, int pageSize = 10)
        {
            BillingsParamsModel.PageIndex = pageIndex;
            BillingsParamsModel.PageSize = pageSize;

            BaseResponse = await _billingService.GetBillingWithParams(BillingsParamsModel);
            Billings = BaseResponse.Items;
            BillingDataGridDetails.Clear();
            await LoadDataGridDetails();
            IsDateFiltered = true;
        }
        private async Task LoadSalesLast30Days()
        {
            try
            {
                var salesData = await _dashboardService.GetSalesLast30Days();
                SalesLast30Days = salesData;
            }
            catch (HttpRequestException ex)
            {
                SalesLast30Days = new List<StatisticsModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                SalesLast30Days = new List<StatisticsModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
        }
        private async Task LoadSalesLast12Month()
        {
            try
            {
                var salesData = await _dashboardService.GetSalesLast12Month();
                SalesLast12Month = salesData;
            }
            catch (HttpRequestException ex)
            {
                SalesLast12Month = new List<StatisticsModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                SalesLast12Month = new List<StatisticsModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
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
                    await GetWithSearch(Search);
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
                await GetWithSearch(Search);
            }
            else
            {
                await LoadData(PageIndex, PageSize);
            }
        }
        private async Task LoadDataGridDetails()
        {
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

        private async Task EvaluateRol()
        {
            var role = await _localStorageService.GetItem(Configuration.ROLE);

            if (role != "Administrator")
            {
                NavigationManager.NavigateTo("/account/signin");
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            await EvaluateRol();
            await LoadData(PageIndex);
            await LoadSalesLast30Days();
            await LoadSalesLast12Month();
            await LoadDataGridDetails();

            await Task.Delay(1000);
            IsLoading = false;
        }
    }
}
