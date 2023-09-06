using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;

namespace BillWare.App.Pages.Costumer
{
    [Authorize("Administrator, Operator")]
    public partial class CostumerForm
    {
        [Parameter] public Common.FormMode FormMode { get; set; }

        [Parameter] public Models.Costumer CostumerParameter { get; set; } = new Models.Costumer();

        private Models.Costumer Costumer = new Models.Costumer();
        private string ButtonTitle => FormMode == Common.FormMode.ADD ? "Agregar" : "Editar";

        private async Task OnSubmit()
        {
            if (FormMode == Common.FormMode.ADD)
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
            if (FormMode == Common.FormMode.EDIT)
            {
                Costumer = CostumerParameter;
            }
        }
    }
}
