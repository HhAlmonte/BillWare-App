using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;

namespace BillWare.App.Pages.BillingService
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class BillingServiceForm
    {
        [Parameter] public Common.FormModeEnum FormMode { get; set; }

        [Parameter] public BillingServiceModel BillingServiceParameter { get; set; } = new BillingServiceModel();

        private BillingServiceModel BillingService = new BillingServiceModel();

        private string ButtonTitle => FormMode == Common.FormModeEnum.ADD ? "Agregar" : "Editar";

        private async Task OnSubmit()
        {
            if (FormMode == Common.FormModeEnum.ADD)
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
                var response = await _billingService.CreateBillingService(BillingService);

                var closeReturn = response != null ? true : false;

                DialogService.Close(closeReturn);
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
                var response = await _billingService.EditBillingService(BillingService);

                var closeReturn = response != null ? true : false;

                DialogService.Close(closeReturn);
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
            if (FormMode == Common.FormModeEnum.EDIT)
            {
                BillingService = BillingServiceParameter;
            }
        }
    }
}
