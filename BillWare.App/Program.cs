using BillWare.App;
using BillWare.App.Intefaces;
using BillWare.App.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Configuration.BASE_API_URL) });

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<DialogService>();

await builder.Build().RunAsync();
