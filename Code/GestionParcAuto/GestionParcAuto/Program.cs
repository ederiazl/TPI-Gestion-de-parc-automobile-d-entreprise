using GestionParcAuto.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using GestionParcAuto.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using GestionParcAuto.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString)
    .UseSeeding((context, _) => { })
    .UseAsyncSeeding(async (context, _, ct) =>
    {
        UserManager<User> userManager = context.GetService<UserManager<User>>();
        RoleManager<IdentityRole> roleManager = context.GetService<RoleManager<IdentityRole>>();

        //Create Roles
        IdentityRole? roleAdminExist = await roleManager.FindByNameAsync("Admin");
        IdentityRole? roleEmployeeExist = await roleManager.FindByNameAsync("Employee");

        if (roleAdminExist == null)
            await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });

        if (roleEmployeeExist == null)
            await roleManager.CreateAsync(new IdentityRole() { Name = "Employee" });

        //Create user if no user created
        List<User> users = userManager.Users.ToList();
        string defaultPwd = ".Admin1";
        User defaultAdmin = new User
        {
            Email = "admin@GestParc.local",
            UserName = "admin@GestParc.local",
        };

        if (users.Count() == 0)
        {
            await userManager.CreateAsync(defaultAdmin, defaultPwd);

            await userManager.AddToRoleAsync(defaultAdmin, "Admin");
        }
    })
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

await using(var serviceScope = app.Services.CreateAsyncScope())
await using(var dbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>())
{
    await dbContext.Database.EnsureCreatedAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
