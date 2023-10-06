using BillWare.Application.Billing.Models;
using Radzen.Blazor;
using Radzen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using BillWare.App.Services;
using BillWare.App.Enum;
using BillWare.App.Common;

namespace BillWare.App.Pages.Billing
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class Index
    {
        [Inject] LocalStorageHelper LocalStorageService { get; set; }
        [Inject] PdfConversionHelper pdfConversionService { get; set; }

        private PaginationResult<BillingModel> BaseResponse { get; set; } = new PaginationResult<BillingModel>();
        private List<BillingModel> Billings { get; set; } = new List<BillingModel>();
        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private RadzenDataGrid<BillingItemModel> grid;

        private bool IsLoading { get; set; } = false;
        private bool IsFiltered { get; set; } = false;
        private bool IsAdmin { get; set; } = false;

        private string Search { get; set; } = string.Empty;
        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";

        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            try
            {
                BaseResponse = await _billingService.GetBillings(pageIndex, pageSize);
                Billings = BaseResponse.Items;
            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new PaginationResult<BillingModel>();
                Billings = new List<BillingModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new PaginationResult<BillingModel>();
                Billings = new List<BillingModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            finally
            {
                IsLoading = false;
            }
        }
        private async Task GetWithSearch(string search)
        {
            IsFiltered = true;
            Search = search;

            try
            {
                if (Search == "")
                {
                    IsFiltered = false;
                    await LoadData(PageIndex, PageSize);
                    return;
                }
                else
                {
                    BaseResponse = await _billingService.GetBillingsWithSearch(Search, PageIndex, PageSize);
                    Billings = BaseResponse.Items;
                }
            }
            catch (HttpRequestException ex)
            {
                BaseResponse = new PaginationResult<BillingModel>();
                Billings = new List<BillingModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
            }
            catch (Exception ex)
            {
                BaseResponse = new PaginationResult<BillingModel>();
                Billings = new List<BillingModel>();
                await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
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

        private async Task Delete(int? id)
        {
            var isConfirm = await SweetAlertServices.ShowWarningAlert("¿Está seguro de eliminar este registro?", "Confirma de que este sea el registro que deseas eliminar");

            if (isConfirm)
            {
                try
                {
                    var billingDeleted = await _billingService.DeleteBilling((int)id);

                    await LoadData(PageIndex, PageSize);

                    await SweetAlertServices.ShowSuccessAlert("Registro eliminado", "El registro se ha eliminado correctamente");
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
        private async Task DeleteBillingItem(int? id)
        {
            var isConfirm = await SweetAlertServices.ShowWarningAlert("¿Está seguro de eliminar este registro?", "Confirma de que este sea el registro que deseas eliminar");

            if (isConfirm)
            {
                try
                {
                    var billingItemDeleted = await _billingItemService.DeleteBillingItem((int)id);

                    await LoadData(PageIndex, PageSize);

                    await SweetAlertServices.ShowSuccessAlert("Registro eliminado", "El registro se ha eliminado correctamente");
                }
                catch (HttpRequestException ex)
                {
                    BaseResponse = new PaginationResult<BillingModel>();
                    Billings = new List<BillingModel>();
                    await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
                }
                catch (Exception ex)
                {
                    BaseResponse = new PaginationResult<BillingModel>();
                    Billings = new List<BillingModel>();
                    await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
                }
            }
        }

        private async Task PrintInvoice(BillingModel billing)
        {
            billing.BillingStatus = (int)Common.BillingStatusEnum.Pagado;

            try
            {

                var htmlContent = InvoiceToHtml(billing);
                

                billing.InvoiceDocument = await pdfConversionService.ConvertHtmlToPdf(htmlContent);

                var invoiceUpdated = await _billingService.UpdateBilling(billing);
                var printInvoice = await SweetAlertServices.ShowWarningAlert("¿Desea imprimir la factura?", "Se abrirá una nueva ventana para imprimir la factura.");
                
                if (printInvoice)
                {
                    await JSRuntimeInvoke.PrintHtml(js, htmlContent);
                }

                await SweetAlertServices.ShowSuccessAlert("Proceso de facturació completado exitosamente", $"La factura se ha pagado correctamente. Se envió la factura al siguiente correo: hbalmontess272@gmail.com");
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
        private string InvoiceToHtml(BillingModel invoice)
        {
            var totalPrice = invoice.TotalPrice.ToString("C");
            var paymentMethod = invoice.PaymentMethod == (int)PaymentMethodEnum.Efectivo ? "Efectivo" : "Tarjeta";

            var html = $@"
    <style>
    .invoice-box {{
        max-width: 800px;
        margin: auto;
        padding: 30px;
        border: 1px solid #eee;
        box-shadow: 0 0 10px rgba(0, 0, 0, 0.15);
        font-size: 16px;
        line-height: 24px;
        font-family: 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
        color: #555;
    }}
    .invoice-box table {{
        width: 100%;
        line-height: inherit;
        text-align: left;
    }}
    .invoice-box table td {{
        padding: 5px;
        vertical-align: top;
    }}
    .invoice-box table tr td:nth-child(n + 2) {{
        text-align: right;
    }}
    .invoice-box table tr.top table td {{
        padding-bottom: 20px;
    }}
    .invoice-box table tr.top table td.title {{
        font-size: 45px;
        line-height: 45px;
        color: #333;
    }}
    .invoice-box table tr.information table td {{
        padding-bottom: 40px;
    }}
    .invoice-box table tr.heading td {{
        background: #eee;
        border-bottom: 1px solid #ddd;
        font-weight: bold;
    }}
    .invoice-box table tr.details td {{
        padding-bottom: 20px;
    }}
    .invoice-box table tr.item td {{
        border-bottom: 1px solid #eee;
    }}
    .invoice-box table tr.item.last td {{
        border-bottom: none;
    }}
    .invoice-box table tr.item input {{
        padding-left: 5px;
    }}
    .invoice-box table tr.item td:first-child input {{
        margin-left: -5px;
        width: 100%;
    }}
    .invoice-box table tr.total td:nth-child(2) {{
        border-top: 2px solid #eee;
        font-weight: bold;
    }}
    .invoice-box input[type='number'] {{
        width: 60px;
    }}
    @media only screen and (max-width: 600px) {{
        .invoice-box table tr.top table td {{
            width: 100%;
            display: block;
            text-align: center;
        }}
        .invoice-box table tr.information table td {{
            width: 100%;
            display: block;
            text-align: center;
        }}
    }}
    /** RTL **/
    .rtl {{
        direction: rtl;
        font-family: Tahoma, 'Helvetica Neue', 'Helvetica', Helvetica, Arial, sans-serif;
    }}
    .rtl table {{
        text-align: right;
    }}
    .rtl table tr td:nth-child(2) {{
        text-align: left;
    }}
</style>
    <div class='invoice-box'>
        <table cellpadding='0' cellspacing='0'>
            <tr class='top'>
                <td colspan='4'>
                    <table>
                        <tr>
                            <td class='title'>
                                <h3>Factura</h3>
                            </td>
                            <td>
                                Número de factura: {invoice.InvoiceNumber} <br> Creado por: {invoice.SellerName}
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class='information'>
                <td colspan='4'>
                    <table>
                        <tr>
                            <td>
                                Santo Domingo<br>Distrito Nacional<br>C/ La Esperilla Esq. C/ Mary Don Bosco
                            </td>
                            <td>
                                Flor de Lis<br> 849-653-3384<br> @flordelisbsalon
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr class='heading'>
                <td colspan='3'>Método de pago</td>
                <td>Costo de facturación</td>
            </tr>
            <tr class='details'>
                <td colspan='3'>{paymentMethod}</td>
                <td>{invoice.TotalPrice.ToString("C")}</td>
            </tr>
            <tr class='heading'>
                <td>Descripción</td>
                <td>Costo X. Unidad</td>
                <td>Cantidad</td>
                <td>Precio</td>
            </tr>";

            foreach (var item in invoice.BillingItems)
            {
                var price = item.Price.ToString("C");

                html += $@"
            <tr class='item'>
                <td>{item.Description}</td>
                <td>{item.Price}</td>
                <td>{item.Quantity}</td>
                <td>{item.Price.ToString("C")}</td>
            </tr>";
            }

            html += $@"
            <tr>
                <td colspan='4'>

                </td>
            </tr>
            <tr class='total'>
                <td colspan='3'></td>
                <td>Total: {totalPrice}</td>
            </tr>
        </table>
    </div>";

            return html;
        }

        private async Task OpenAddDialogForm()
        {
            var dialogResponse = await DialogService.OpenAsync<BillingForm>("Crear Factura"
                            , options: new DialogOptions
                            {
                                Width = "1080px",
                                Height = "auto",
                                Draggable = true
                            });

            if(dialogResponse != null)
            {
                try
                {
                    var billingResult = dialogResponse as BillingModel;

                    if (billingResult.BillingStatus == 2)
                    {
                        await LoadData(PageIndex, PageSize);
                        await PrintInvoice(billingResult);
                    }
                    else
                    {
                        await LoadData(PageIndex, PageSize);
                    }
                }
                catch (HttpRequestException ex)
                {
                    BaseResponse = new PaginationResult<BillingModel>();
                    Billings = new List<BillingModel>();
                    await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
                }
                catch (Exception ex)
                {
                    BaseResponse = new PaginationResult<BillingModel>();
                    Billings = new List<BillingModel>();
                    await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
                }
            }
        }
        private async Task OpenEditDialogForm(BillingModel billing)
        {
            var dialogResponse = await DialogService.OpenAsync<BillingForm>("Editar Factura"
                            , parameters: new Dictionary<string, object>() { { "BillingParameter", billing }, { "FormMode", Common.FormModeEnum.EDIT } }
                            , options: new DialogOptions
                            {
                                Width = "1080px",
                                Height = "auto",
                                Draggable = true
                            });

            if (dialogResponse != null)
            {
                try
                {
                    var billingResult = dialogResponse as BillingModel;

                    if (billingResult.BillingStatus == 2)
                    {
                        await LoadData(PageIndex, PageSize);
                        await PrintInvoice(billingResult);
                    }
                    else
                    {
                        await LoadData(PageIndex, PageSize);
                    }
                }
                catch (HttpRequestException ex)
                {
                    BaseResponse = new PaginationResult<BillingModel>();
                    Billings = new List<BillingModel>();
                    await SweetAlertServices.ShowErrorAlert("Ocurrió un error", ex.Message);
                }
                catch (Exception ex)
                {
                    BaseResponse = new PaginationResult<BillingModel>();
                    Billings = new List<BillingModel>();
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
