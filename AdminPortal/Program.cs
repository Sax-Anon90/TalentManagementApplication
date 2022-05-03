using AdminPortal.Data;
using AdminPortal.CoreBusiness.Repositories.Implementation;
using AdminPortal.UI.Mappings;
using AdminPortal.CoreBusiness.Repositories.Contracts;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connString = builder.Configuration.GetConnectionString("TalentManagementDbConn");
builder.Services.AddDbContext<SaxTalentManagementContext>(options => options.UseSqlServer(connString));

//Dependancy Injection
builder.Services.AddAutoMapper(typeof(MapperConfig));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
