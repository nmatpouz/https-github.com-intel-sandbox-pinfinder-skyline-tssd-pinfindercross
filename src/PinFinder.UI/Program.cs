using Autofac;
using Autofac.Extensions.DependencyInjection;

//using MediatR;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PinFinder.Core.Extension;
using PinFinder.Database;
using MudBlazor.Services;
using PinFinder.UI;
using PinFinder;
using PinFinder.UI.Components;
using System.Data.Odbc;
using PinFinder.Core.Controllers;
using BlazorPanzoom;

string siteRoot = "D:/SkyPF_repo1/pinfinder-sttd_skyline_pinfinder/pinfinder_classic/pinfinder_classic_web";

var builder = WebApplication.CreateBuilder(args);


OdbcConnectionStringBuilder odbc = new()
{
    Driver = "ODBC Driver 18 for SQL Server"
};
odbc.Add("SERVER", "sql2101-fm1-in.amr.corp.intel.com,3181");
odbc.Add("DATABASE", "pinfinder_production");
odbc.Add("UID", "pinfinder_pr_rw");
odbc.Add("PWD", "6OsW32m4x3x68Qx");
//odbc.Add("UID", "pinfinder_pr_so");
//odbc.Add("PWD", "e978pRo2jGoZvGs");
builder.Services.AddSingleton(odbc);
builder.Services.AddSingleton(typeof(ApplicationDbContext));

// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddBlazorPanzoomServices();

builder.Services.AddSingleton<ProductStateService>();

builder.Services.AddHttpClient();
builder.Services.RegisterRequestHandlers();
//builder.Services.AddBootstrapBlazor();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
   .ConfigureContainer<ContainerBuilder>(b =>
   {
       b.RegisterControllers();
       b.PopulateMediaTCollection();
   });

// Register the ItuffQueryController service
//builder.Services.AddScoped<IItuffQueryController, ItuffQueryController>();
builder.Services.AddScoped<ProductConfigQueryController>();
builder.Services.AddScoped<SvgQueryController>();
builder.Services.AddScoped<ProductDetailQueryController>();
builder.Services.AddScoped<PostProcessingQueryController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
//app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();
//app.MapRazorComponents<App>()
//    .AddInteractiveWebAssemblyRenderMode();


app.Run();

