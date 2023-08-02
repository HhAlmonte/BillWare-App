using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.JSInterop;
using System.Threading.Tasks;

public static class SweetAlertServices
{
    private static SweetAlertService Swal;

    public static void Initialize(IJSRuntime jsRuntime)
    {
        Swal = new SweetAlertService(jsRuntime);
    }

    public static async Task ShowSuccessAlert(string title, string message)
    {
        await Swal.FireAsync(new SweetAlertOptions
        {
            Title = title,
            Text = message,
            Icon = SweetAlertIcon.Success
        });
    }

    public static async Task ShowErrorAlert(string title, string message)
    {
        await Swal.FireAsync(new SweetAlertOptions
        {
            Title = title,
            Text = message,
            Icon = SweetAlertIcon.Error
        });
    }

    public static async Task<bool> ShowWarningAlert(string title, string message)
    {
        var action = await Swal.FireAsync(new SweetAlertOptions
        {
            Title = title,
            Text = message,
            Icon = SweetAlertIcon.Warning,
            ShowCancelButton = true,
            ConfirmButtonText = "Aceptar",
            CancelButtonText = "Cancelar"
        });

        return !string.IsNullOrEmpty(action.Value);
    }

    // You can add more methods for other types of alerts if needed
}
