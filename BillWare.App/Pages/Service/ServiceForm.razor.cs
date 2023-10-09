﻿using BillWare.App.Enum;
using BillWare.App.Intefaces;
using BillWare.App.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.Service
{
    [Authorize(Roles = "Administrator, Operator")]
    public partial class ServiceForm
    {
        [Inject] private IServicesService? _billingService { get; set; }
        [Inject] private DialogService? DialogService { get; set; }

        [Parameter] public FormModeEnum FormMode { get; set; }

        [Parameter] public BillingServiceModel BillingServiceParameter { get; set; } = new BillingServiceModel();

        private BillingServiceModel BillingService = new BillingServiceModel();

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
            var response = await _billingService!.CreateAsync(BillingService);

            if (!response.IsSuccessFul)
            {
                await SweetAlertServices.ShowErrorAlert(response.Message, response.Details!);
                return;
            }

            var closeReturn = response != null ? true : false;

            DialogService!.Close(closeReturn);
        }

        private async Task Edit()
        {
            var response = await _billingService!.UpdateAsync(BillingService);

            if (!response.IsSuccessFul)
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
                BillingService = BillingServiceParameter;
            }
        }
    }
}