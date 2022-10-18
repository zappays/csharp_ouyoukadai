using Microsoft.EntityFrameworkCore;
using OuyouKadai.Data;
using OuyouKadai.Models.SeedData;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<OuyouKadaiContext>(options =>
options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// クッキー認証を追加
builder.Services.AddAuthentication("MyCookieAuthenticationScheme")
    .AddCookie("MyCookieAuthenticationScheme", options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/Forbidden/";
    });

// Add services to the container.
builder.Services.AddControllersWithViews();

// 初期データ投入
var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    // ↓↓SeedData だと管理者が1名だけ登録された最低限の初期データ投入ができます↓↓
    // ↓↓SeedData2 だとuser, tasukitemのテストデータが数2000件登録された初期データを投入できます↓↓
    SeedData2.Initialize(services); 
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// クッキー認証を追加
app.UseAuthentication();
// 認可
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
