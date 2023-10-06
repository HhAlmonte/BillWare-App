using BillWare.App.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using System;

namespace BillWare.App.Pages.Costumer
{
    [Authorize("Administrator, Operator")]
    public partial class CostumerForm
    {
        [Parameter] public FormModeEnum FormMode { get; set; }

        [Parameter] public Models.CostumerModel CostumerParameter { get; set; } = new Models.CostumerModel();

        private Models.CostumerModel Costumer = new Models.CostumerModel();
        private string ButtonTitle => FormMode == FormModeEnum.ADD ? "Agregar" : "Editar";

        private async Task OnSubmit()
        {
            if (FormMode == FormModeEnum.ADD)
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
                var response = await _costumerService.CreateAsync(Costumer);

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
                var response = await _costumerService.UpdateAsync(Costumer);

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
            if (FormMode == FormModeEnum.EDIT)
            {
                Costumer = CostumerParameter;
            }
        }
    }
}
