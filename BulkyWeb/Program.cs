using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using BulkyWeb.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Bulky.Utility;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//we need to tell "AddDbContext" which class has the implementation of DBContext "ApplicationDbContext" + configure options i.e we will be using SQL Server
//when we are using SqlServer, we also need to pass in the connection string to "UseSqlServer()" from appsettings.json
//when you add something to the services container, that way you are adding it to dependecy injection and we won't need to do new ApplicationDbContext etc
//allows for dependency injections app wide
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//if the names inside of StripeSettings matches whats inside of appsettings then StripeSettings value will automatically be updated
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

builder.Services
    .AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

//this needs to be added after identity for it to work
//configuring custom paths to overwrite default paths
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

//allows us to add routing for razor pages with "Identity"
builder.Services.AddRazorPages();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IEmailSender, EmailSender>();

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

//configuring stripe with it's secret key
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<String>();

app.UseRouting();
//must be added before UseAuthorization()
app.UseAuthentication();

app.UseAuthorization();

//fixes the razor pages routing for identity razor pages.
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    //what area should be used as default is set here
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
