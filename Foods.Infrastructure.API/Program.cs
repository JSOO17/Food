using FluentValidation;
using FluentValidation.AspNetCore;
using Foods.Application.Services;
using Foods.Application.Services.Interfaces;
using Foods.Domain.HttpClients;
using Foods.Domain.HttpClients.Interfaces;
using Foods.Domain.Interfaces.API;
using Foods.Domain.Interfaces.SPI;
using Foods.Domain.UserCases;
using Foods.Infrastructure.API.Security;
using Foods.Infrastructure.Data.Adapters;
using Foods.Infrastructure.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<foodContext>(options =>
            options.UseMySql(builder.Configuration.GetConnectionString("food"), ServerVersion.Parse("10.4.28-mariadb")));

builder.Services.AddTransient<IRestaurantPersistencePort, RestaurantAdapter>();
builder.Services.AddTransient<IRestaurantServicesPort, RestaurantUsercases>();
builder.Services.AddTransient<IRestaurantServices, RestaurantServices>();

builder.Services.AddTransient<IDishPersistencePort, DishAdapter>();
builder.Services.AddTransient<IDishServicesPort, DishUsercases>();
builder.Services.AddTransient<IDishServices, DishServices>();

builder.Services.AddHttpClient<IUserMicroHttpClient, UserMicroHttpClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7019/");
});

builder.Services.AddLogging();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var JwtSettings = builder.Configuration.GetSection("Jwt").Get<JWTSettings>();

        options.IncludeErrorDetails = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = false,
            ValidIssuer = JwtSettings.Issuer,
            ValidAudience = JwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings.Key))
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
