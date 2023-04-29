using Utilities.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionMonitor;
using Utilities.Data;
using Utilities.Services.Interfaces;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddTransient<IAccountService, AccountService>();
//?? 
builder.Services.AddSingleton<Application>();

var serviceProvider = builder.Services.BuildServiceProvider();
var application = serviceProvider.GetService<Application>();
application.Run();

