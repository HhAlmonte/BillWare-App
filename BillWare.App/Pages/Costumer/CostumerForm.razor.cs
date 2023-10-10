using BillWare.App.Enum;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Costumer
{
    [Authorize("Administrator, Operator")]
    public partial class CostumerForm
    {
        [Inject] private ICostumerService? _costumerService { get; set; }
        [Inject] DialogService? DialogService { get; set; }

        [Parameter] public FormModeEnum FormMode { get; set; }
        [Parameter] public CostumerModel CostumerParameter { get; set; } = new CostumerModel();

        private CostumerModel Costumer = new CostumerModel();
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
            var response = await _costumerService!.CreateAsync(Costumer);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            var closeReturn = response != null ? true : false;
            DialogService!.Close(closeReturn);
        }

        private async Task Edit()
        {
            var response = await _costumerService!.UpdateAsync(Costumer);

            if (!response.IsSuccessFull)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            var closeReturn = response != null ? true : false;
            DialogService!.Close(closeReturn);
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
