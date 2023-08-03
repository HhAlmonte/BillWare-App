using Microsoft.JSInterop;

public static class JSRuntimeInvoke
{
    public static async Task PrintHtml(this IJSRuntime js, string html)
    {
        await js.InvokeVoidAsync("printHtml", html);
    }
}
