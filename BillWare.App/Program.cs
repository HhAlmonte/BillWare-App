using BillWare.App;
using BillWare.App.Helpers;
using BillWare.App.Intefaces;
using BillWare.App.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazorBootstrap();
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped(typeof(IBaseCrudService<>), typeof(BaseCrudService<>));
builder.Services.AddScoped<JWTAuthenticationStateProviderService>();
builder.Services.AddScoped<BeamAuthenticationStateProviderHelper>();
builder.Services.AddScoped<IJWTAuthenticationStateProvider, JWTAuthenticationStateProviderService>(provider => provider.GetRequiredService<JWTAuthenticationStateProviderService>());
builder.Services.AddScoped<AuthenticationStateProvider, BeamAuthenticationStateProviderHelper>();
builder.Services.AddScoped<LocalStorageHelper>();
builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
builder.Services.AddScoped<PdfConversionHelper>();
var httpClient = new HttpClient()
{
    BaseAddress = new Uri(Configuration.BASE_API_URL)
};

builder.Services.AddSingleton(httpClient);

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IServicesService, BillingServiceService>();
builder.Services.AddScoped<IBillingItemService, BillingItemService>();
builder.Services.AddScoped<ICostumerService, CostumerService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<DialogService>();

var host = builder.Build();
SweetAlertServices.Initialize((IJSRuntime)host.Services.GetService(typeof(IJSRuntime)));
builder.Services.AddSweetAlert2();
// Initialize SweetAlertServices with IJSRuntime

await builder.Build().RunAsync();
