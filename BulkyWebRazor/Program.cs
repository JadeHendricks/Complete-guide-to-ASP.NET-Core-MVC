using BulkyWebRazor.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//we need to tell "AddDbContext" which class has the implementation of DBContext "ApplicationDbContext" + configure options i.e we will be using SQL Server
//when we are using SqlServer, we also need to pass in the connection string to "UseSqlServer()" from appsettings.json
//when you add something to the services container, that way you are adding it to dependecy injection and we won't need to do new ApplicationDbContext etc
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
