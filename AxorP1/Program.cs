using AxorP1;
using AxorP1.Components;
using AxorP1.Services;
using Syncfusion.Blazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddSyncfusionBlazor();

// Custom services
builder.Services.AddSingleton<DataProvider>();

// ILogger configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddEventLog();

var app = builder.Build();
//Register Syncfusion license https://help.syncfusion.com/common/essential-studio/licensing/how-to-generate
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NHaF5cXmVCf1FpRGVGfV5yd0VHYlZUQ3xdQ00DNHVRdkdgWH5fd3VUR2ZcVEZ+XEA=");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
