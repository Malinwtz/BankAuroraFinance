using Utilities.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TransactionMonitor;
using Utilities.Data;
using Utilities.Services.Interfaces;
using Utilities.Models;
using System.Reflection;
using Utilities.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddDbContext<BankAppDataContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddTransient<IAccountService, AccountService>();
builder.Services.AddTransient<ITransactionMonitorService, TransactionMonitorService>();



builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);



//?? 
builder.Services.AddSingleton<Application>();

var serviceProvider = builder.Services.BuildServiceProvider();
var application = serviceProvider.GetService<Application>();
application.Run();

