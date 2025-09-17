using CurrieTechnologies.Razor.SweetAlert2;
using DAL.BaseConn;
using DAL.Repo;
using IdentityAuthentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Connections;
using ReportKehadiranFromThumbPrint.Components;
using SAL;
using Utilities;

var builder = WebApplication.CreateBuilder(args);


var prodConnStr = builder.Configuration.GetConnectionString("PROD") ?? "";
var DevConnStr = builder.Configuration.GetConnectionString("DEV") ?? "";
var EHRConnStr = builder.Configuration.GetConnectionString("EHR") ?? "";

// Register connection factory
builder.Services.AddScoped<ServerProd>(conn => new ServerProd(prodConnStr));
builder.Services.AddScoped<ServerDev>(conn => new ServerDev(DevConnStr));
builder.Services.AddScoped<ServerEHR>(conn => new ServerEHR(EHRConnStr));

builder.Services.AddSweetAlert2(options =>
{
    options.Theme = SweetAlertTheme.Default;
});



builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.Cookie.Name = "ReportStaff";
        opt.LoginPath = "/Main";
        opt.LogoutPath = "/logout";
        opt.AccessDeniedPath = "/AccessDenied";
        opt.Cookie.MaxAge = TimeSpan.FromMinutes(2);
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddHttpContextAccessor();
builder.Services.Configure<CircuitOptions>(options => options.DetailedErrors = true);

builder.Services.AddScoped<IIdentityAuthenticationLib, IdentityAuthenticationLib>();
builder.Services.AddScoped<IRepoData, RepoData>();
builder.Services.AddScoped<IServices, Services>();
builder.Services.AddScoped<IReportRepo, ReportRepo>();
builder.Services.AddScoped<ImyUtils, myUtils>();
builder.Services.AddScoped<IUserRepo, UserRepo>();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapStaticAssets();
app.UseAntiforgery();
app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
