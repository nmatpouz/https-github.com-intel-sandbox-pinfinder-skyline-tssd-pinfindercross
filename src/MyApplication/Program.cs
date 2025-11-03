using Autofac;
using Autofac.Extensions.DependencyInjection;

//using MediatR;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

using PinFinder.Core.Domain.Commands.Calls;
using PinFinder.Core.Extension;
using PinFinder.Database;

using PinFinderSkyPF.Client.Services;
using PinFinderSkyPF.Components;

using System.Data.Odbc;

var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<AppState>();

//builder.Services.AddGrpcClient<YourGrpcService.YourGrpcServiceClient>(o =>
//{
//    o.Address = new Uri("https://localhost:5001");
//});

//var connectionString =
//    builder.Configuration.GetConnectionString("DefaultConnection")
//        ?? throw new InvalidOperationException("Connection string"
//        + "'DefaultConnection' not found.");

OdbcConnectionStringBuilder odbc = new()
{
    Driver = "ODBC Driver 18 for SQL Server"
};
odbc.Add("SERVER", "sql2101-fm1-in.amr.corp.intel.com");
odbc.Add("Port", "3181");
odbc.Add("DATABASE", "pinfinder_production");
odbc.Add("UID", "pinfinder_pr_rw");
odbc.Add("PWD", "6OsW32m4x3x68Qx");
builder.Services.AddSingleton(odbc);
builder.Services.AddSingleton(typeof(ApplicationDbContext));



// Add MudBlazor services
builder.Services.AddMudServices();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddBlazorBootstrap();

builder.Services.AddHttpClient();
builder.Services.RegisterRequestHandlers();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
   .ConfigureContainer<ContainerBuilder>(b =>
    {
        b.RegisterControllers();
        b.PopulateMediaTCollection();
    } );

    var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode();
    //.AddAdditionalAssemblies(typeof(PinFinderSkyPF.Client._Imports).Assembly);

app.Run();
