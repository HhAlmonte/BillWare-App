using BillWare.App.Enum;
using BillWare.App.Intefaces;
using BillWare.App.Pages.Category;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace BillWare.App.Pages.HoraExtra
{
    public partial class Index
    {
        [Inject] private IHoraExtraService HoraExtraService { get; set; }
        [Inject] private DialogService DialogService { get; set; }

        public bool IsLoading { get; set; } = false;

        public List<Models.HoraExtra> HoraExtras { get; set; } = new List<Models.HoraExtra>();

        private async Task LoadData()
        {
            IsLoading = true;

            HoraExtras = await HoraExtraService.GetHorasExtras();

            IsLoading = false;
        }

        private async Task OpenAddDialogForm()
        {
            var dialogResult = await DialogService!.OpenAsync<HoraExtraForm>("Solicitud",
                    new Dictionary<string, object>
                    {
                        { "FormMode", FormModeEnum.ADD }
                    },
                    new DialogOptions
                    {
                        Width = "auto"
                    });
        }

        protected override async Task OnInitializedAsync()
        {
            await LoadData();
        }
    }
}
