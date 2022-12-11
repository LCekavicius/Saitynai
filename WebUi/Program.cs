using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using Syncfusion.Blazor;
using WebApi.Auth.Model;
using WebApi.Data;
using WebUi.Auth;
using WebUi.Components.Popovers;
using WebUi.Data;
using WebUi.Data.Services.ToastService;
using WebUi.Data.Services.UserService;
using WebUi.Helpers.Http;

var builder = WebApplication.CreateBuilder(args);

StaticWebAssetsLoader.UseStaticWebAssets(builder.Environment, builder.Configuration);

// Add services to the container.
builder.Services.AddTransient<IAuthService, AuthService>();
builder.Services.AddTransient<ICompanyService, CompanyService>();
builder.Services.AddTransient<IOrderService, OrderService>();
builder.Services.AddTransient<IWorkService, WorkService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<LaucekERPDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddMudServices();
builder.Services.AddIdentity<ERPUser, IdentityRole>()
    .AddEntityFrameworkStores<LaucekERPDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<AuthenticationStateProvider, ErpStateProvider>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddScoped<ILaucekHttpClient, LaucekHttpClient>();
builder.Services.AddScoped(sp => sp
            .GetRequiredService<IHttpClientFactory>()
            .CreateClient("WebApi"))
        .AddHttpClient("WebApi", client =>
        {
            client.BaseAddress = new Uri(builder.Configuration.GetSection("WebApiBaseUrl").Value);
        });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();