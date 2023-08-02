using BillWare.App;
using BillWare.App.Intefaces;
using BillWare.App.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(Configuration.BASE_API_URL) });

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IVehiculoEntranceService, VehiculoEntranceService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<DialogService>();
var host = builder.Build();
SweetAlertServices.Initialize((IJSRuntime)host.Services.GetService(typeof(IJSRuntime)));
builder.Services.AddSweetAlert2();
// Initialize SweetAlertServices with IJSRuntime

await builder.Build().RunAsync();
