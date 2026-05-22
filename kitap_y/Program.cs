using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using kitap_y.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabanı Bağlantısı
builder.Services.AddDbContext<KitapYContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("KitapYContext")));

// 2. Identity (Giriş ve Rol) Ayarları
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>() // Rol yönetimi için bu şart
.AddEntityFrameworkStores<KitapYContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. Kimlik Doğrulama ve Yetkilendirme (Sıralama Önemli!)
app.UseAuthentication(); 
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages(); // Giriş/Kayıt sayfaları için gerekli

// Uygulama başlarken rolleri otomatik oluşturma kodu
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    string[] roleNames = { "Admin", "BookAddUser", "User" };

    foreach (var roleName in roleNames)
    {
        if (!roleManager.RoleExistsAsync(roleName).Result)
        {
            roleManager.CreateAsync(new IdentityRole(roleName)).Wait();
        }
    }
}

app.Run();