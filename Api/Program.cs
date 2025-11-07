using Api;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Api.Models;

/* Notes:
# Database (Package Manager Consoler)
- Install: dotnet tool install --global dotnet-ef
- Add migration: Add-Migration InitialCreate
- Update database: Update-Database
 */

var builder = WebApplication.CreateBuilder(args);

// Add connection string and DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlite(connectionString);
});

// Add user identity:
builder.Services.AddDefaultIdentity<User>(options =>
{
    options.User.AllowedUserNameCharacters = "aábcdeéfghiíjklmnoóöőpqrstuúüűvwxyzAÁBCDEÉFGHIÍJKLMNOÓÖŐPQRSTUÚÜŰVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<Context>()
.AddDefaultTokenProviders();

// Add services to the container.

builder.Services.AddControllers();

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
