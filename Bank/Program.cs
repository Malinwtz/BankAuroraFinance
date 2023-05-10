using Utilities.Data;
using Utilities.Models;
using Utilities.Services;
using Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Utilities.Services.Interfaces;
using Utilities.Infrastructure;
using Utilies.Services;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()  
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddRazorPages();

// Add ToastNotification
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

builder.Services.AddDbContext<BankAppDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddTransient<DataInitializer>();

builder.Services.AddTransient<ICustomerService, CustomerService>();
builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ISingleCustomerService, SingleCustomerService>();
builder.Services.AddTransient<ISingleAccountService, SingleAccountService>();
builder.Services.AddTransient<IUserService, UserService>();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddResponseCaching();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetService<DataInitializer>().SeedData();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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

app.UseNotyf();
app.MapRazorPages();
app.UseResponseCaching();

app.Run();
