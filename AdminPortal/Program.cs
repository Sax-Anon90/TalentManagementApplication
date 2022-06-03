using AdminPortal.Data.Data;
using AdminPortal.CoreBusiness.Repositories.Implementation;
using AdminPortal.CoreBusiness.Mappings;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;
using AdminPortal.CoreBusiness.Services;
using Serilog;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();
var connString = builder.Configuration.GetConnectionString("TalentManagementDbConn");
builder.Services.AddDbContext<SaxTalentManagementContext>(options => options.UseSqlServer(connString));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(q => q.LoginPath="/Authentication/Index");


//Dependancy Injection
builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IExcelFileService, ExcelFileService>();
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();

builder.Host.UseSerilog((ctx, lc) =>
    lc.WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration));

var app = builder.Build();

app.UseSerilogRequestLogging();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Courses}/{action=Index}/{id?}");

app.Run();
