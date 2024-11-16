using webdemo.Models;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
}
class_Init ObjInit = new class_Init();
string iHostType = builder.Configuration.GetValue<string>("HostType");
iHostType = !string.IsNullOrEmpty(iHostType) ? iHostType : "DEV";
ObjInit.AppInitialization(iHostType);

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
