using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;

namespace BillWare.App.Pages.Costumer
{
    [Authorize("Administrator, Operator")]
    public partial class CostumerForm
    {
        [Parameter] public Common.FormModeEnum FormMode { get; set; }

        [Parameter] public Models.CostumerModel CostumerParameter { get; set; } = new Models.CostumerModel();

        private Models.CostumerModel Costumer = new Models.CostumerModel();
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
                var response = await _costumerService.CreateCostumer(Costumer);

                var closeReturn = response != null ? true : false;

                DialogService.Close(closeReturn);
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
                var response = await _costumerService.EditCostumer(Costumer);

                var closeReturn = response != null ? true : false;

                DialogService.Close(closeReturn);
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
                Costumer = CostumerParameter;
            }
        }
    }
}
