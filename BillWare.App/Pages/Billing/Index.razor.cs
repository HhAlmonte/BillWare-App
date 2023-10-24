using BillWare.App.Common;
using BillWare.App.Enum;
using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using BillWare.Application.Billing.Models;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace BillWare.App.Pages.Billing
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class Index
    {
        [Inject] private IBillingItemService? _billingItemService { get; set; }
        [Inject] private IBillingService? _billingService { get; set; }
        [Inject] private IJSRuntime? js { get; set; }
        [Inject] private DialogService? DialogService { get; set; }
        [Inject] BeamAuthenticationStateProviderHelper? BeamAuthenticationStateProviderHelper { get; set; }

        private PaginationResult<BillingModel> BaseResponse { get; set; } = new PaginationResult<BillingModel>();
        private List<BillingModel> Billings { get; set; } = new List<BillingModel>();
        private IEnumerable<int> PageSizeOptions { get; set; } = new int[] { 5, 10, 20, 50 };

        private bool IsLoading { get; set; } = false;
        private bool IsFiltered { get; set; } = false;
        private bool IsAdmin { get; set; } = false;

        private string Search { get; set; } = string.Empty;
        private string pagingSummaryFormat = "Desplegando página {0} de {1} total {2} registros";

        private int PageSize { get; set; } = 5;
        private int PageIndex { get; set; } = 1;

        private async Task LoadData(int pageIndex, int pageSize = 5)
        {
            Search = string.Empty;

            var response = await _billingService!.GetEntitiesPagedAsync(pageIndex, pageSize);

            BaseResponse = response.Data!;
            Billings = BaseResponse.Items;
        }
        private async Task GetBillingsWithSearch(string search)
        {
            IsFiltered = true;
            Search = search;

            if (Search == "")
            {
                IsFiltered = false;
                await LoadData(PageIndex, PageSize);
                return;
            }

            var response = await _billingService!.GetEntitiesPagedWithSearchAsync(PageIndex, PageSize, Search);
            BaseResponse = response.Data!;
            Billings = BaseResponse.Items;

            if (Billings.Count <= 0)
            {
                await SweetAlertServices.ShowToastAlert("No hay registros", "No se encontraron registros", SweetAlertIcon.Warning);
            }
        }

        private async Task PageIndexChanged(int pageIndex)
        {
            PageIndex = pageIndex + 1;

            if (IsFiltered)
            {
                await GetBillingsWithSearch(Search);
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
                await GetBillingsWithSearch(Search);
            }
            else
            {
                await LoadData(PageIndex, PageSize);
            }
        }

        private async Task DeleteBilling(int? id)
        {
            var isConfirm = await SweetAlertServices.ShowWarningAlert("¿Está seguro de eliminar este registro?", "Confirma de que este sea el registro que deseas eliminar");

            if (isConfirm)
            {
                var response = await _billingService!.DeleteAsync((int)id!);

                if (!response.IsSuccessFull)
                {
                    await SweetAlertServices.ShowToastAlert(response.Message, response.Details!, SweetAlertIcon.Error);
                    return;
                }

                if (Billings.Count == 1 && PageIndex != 1)
                {
                    PageIndex -= 1;
                    await LoadData(PageIndex, PageSize);
                    return;
                }

                await LoadData(PageIndex, PageSize);
                await SweetAlertServices.ShowToastAlert("Operación exitosa", "La factura se ha eliminado correctamente. La lista se actualizará en breve.", SweetAlertIcon.Success);
            }
        }
        private async Task DeleteBillingItem(int? id)
        {
            var isConfirm = await SweetAlertServices.ShowWarningAlert("¿Está seguro de eliminar este registro?", "Confirma de que este sea el registro que deseas eliminar");

            if (isConfirm)
            {
                var response = await _billingItemService!.DeleteBillingItem((int)id!);

                if (!response.IsSuccessStatusCode)
                {
                    return;
                }

                await LoadData(PageIndex, PageSize);

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "El item se ha eliminado correctamente. La lista se actualizará en breve.", SweetAlertIcon.Success);
            }
        }

        private async Task PrintInvoice(BillingModel billing)
        {
            billing.BillingStatus = (int)BillingStatus.Pagado;

            var htmlContent = InvoiceToHtml(billing);

            await _billingService!.UpdateAsync(billing);

            var printInvoice = await SweetAlertServices.ShowWarningAlert("¿Desea imprimir la factura?", "Se abrirá una nueva ventana para imprimir la factura.");

            if (printInvoice)
            {
                await JSRuntimeInvoke.PrintHtml(js!, htmlContent);
            }

            await SweetAlertServices.ShowToastAlert("Proceso completado", $"La factura se ha pagado correctamente. Se envió la factura al siguiente correo: hbalmontess272@gmail.com");
            await LoadData(PageIndex, PageSize);
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
            var dialogResult = await DialogService!.OpenAsync<BillingForm>("Crear Factura"
                      ,options: new DialogOptions
                      {
                          Width = "1080px",
                          Height = "auto",
                          Draggable = true
                      });

            if (dialogResult != null)
            {
                var billingResult = dialogResult as BillingModel;

                if (billingResult!.BillingStatus == 2)
                {
                    await LoadData(PageIndex, PageSize);
                    await PrintInvoice(billingResult);
                    return;
                }

                await LoadData(PageIndex, PageSize);

                await SweetAlertServices.ShowToastAlert("Operación exitosa", "La factura se ha creado correctamente. La lista se actualizará en breve.", SweetAlertIcon.Success);
            }
        }
        private async Task OpenEditDialogForm(BillingModel billing)
        {
            var dialogResponse = await DialogService!.OpenAsync<BillingForm>("Editar Factura"
                      , parameters: new Dictionary<string, object>() { { "BillingParameter", billing }, { "FormMode", FormModeEnum.EDIT } }
                      , options: new DialogOptions
                      {
                          Width = "1080px",
                          Height = "auto",
                          Draggable = true
                      });

            if (dialogResponse != null)
            {
                var billingResult = dialogResponse as BillingModel;

                if (billingResult!.BillingStatus == 2)
                {
                    await LoadData(PageIndex, PageSize);
                    await PrintInvoice(billingResult);
                    return;
                }

                await LoadData(PageIndex, PageSize);
                await SweetAlertServices.ShowToastAlert("Operación exitosa", "La factura se ha actualizado correctamente. La lista se actualizará en breve.", SweetAlertIcon.Success);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            IsLoading = true;

            var userAuth = await BeamAuthenticationStateProviderHelper!.GetAuthenticationStateAsync();

            IsAdmin = userAuth.User.IsInRole("Administrator");

            await LoadData(PageIndex);

            IsLoading = false;
        }
    }
}
