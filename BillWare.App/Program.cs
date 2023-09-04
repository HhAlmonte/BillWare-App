using BillWare.App;
using BillWare.App.Intefaces;
using BillWare.App.Services;
using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using Radzen;
using System.Net.Http.Headers;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<LocalStorageService>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<TokenInterceptor>();

var jsRuntime = builder.Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();
var localStorageService = new LocalStorageService(jsRuntime);
var tokenService = new TokenService(localStorageService);

var httpClient = new HttpClient(new TokenInterceptor(tokenService)) { 
    BaseAddress = new Uri(Configuration.BASE_API_URL) 
};

builder.Services.AddSingleton(httpClient);

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IBillingService, BillingService>();
builder.Services.AddScoped<IBillingServiceService, BillingServiceService>();
builder.Services.AddScoped<IBillingItemService, BillingItemService>();
builder.Services.AddScoped<ICostumerService, CostumerService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<DialogService>();

var host = builder.Build();
SweetAlertServices.Initialize((IJSRuntime)host.Services.GetService(typeof(IJSRuntime)));
builder.Services.AddSweetAlert2();
// Initialize SweetAlertServices with IJSRuntime

await builder.Build().RunAsync();
