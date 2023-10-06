using Microsoft.JSInterop;

namespace BillWare.App.Helpers
{
    public class PdfConversionHelper
    {
        private readonly IJSRuntime _jsRuntime;

        public PdfConversionHelper(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public async Task<byte[]> ConvertHtmlToPdf(string htmlContent)
        {
            try
            {
                var pdfBytesBase64 = await _jsRuntime.InvokeAsync<string>("generatePdfFromHtml", htmlContent);

                var pdfBytes = Convert.FromBase64String(pdfBytesBase64);

                return pdfBytes;
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al convertir HTML a PDF", ex);
            }
        }
    }
}
