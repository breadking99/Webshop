using Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using System.Text;

/* NOTES FOR ME (Migration):
# Database (Package Manager Consoler)
- Install: dotnet tool install --global dotnet-ef
- Add migration: Add-Migration InitialCreate
- Update database: Update-Database
 */

var builder = WebApplication.CreateBuilder(args);

//! Add connection string and DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlite(connectionString);
});

//! Remove cors policy:
builder.Services.AddCors(options =>
{
    options.AddPolicy("AnyCorsPolicy", builder =>
        builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader());
});

//! Add user identity:
builder.Services.AddDefaultIdentity<User>(options =>
{
    options.User.AllowedUserNameCharacters = "aábcdeéfghiíjklmnoóöőpqrstuúüűvwxyzAÁBCDEÉFGHIÍJKLMNOÓÖŐPQRSTUÚÜŰVWXYZ0123456789-._@+ ";
    options.User.RequireUniqueEmail = false;
    options.SignIn.RequireConfirmedAccount = false;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<Context>()
.AddDefaultTokenProviders();

//! Authentication and Authorization:
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Authentication:Issuer"],
        ValidAudience = builder.Configuration["Authentication:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Authentication:Key"]!))
    };
})
.AddCookie();

//! Add services to the container.
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault;
});

//! Health checks (standard built-in)
builder.Services.AddHealthChecks();

//! Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

//! Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//! Use any CORS:
if (true)
{
    app.UseCors("AnyCorsPolicy");
}

app.UseHttpsRedirection();

//! Ensure authentication is used before authorization
app.UseAuthentication();
app.UseAuthorization();

//! Map health check endpoint at /status and allow anonymous access
app.MapHealthChecks("/status").AllowAnonymous();

app.MapControllers();

app.Run();
