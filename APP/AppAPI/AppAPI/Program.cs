using AppAPI.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Sieve.Services;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(optons =>
{
    optons.AddSecurityDefinition("Auth", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using Bearer Scheme.",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    optons.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

builder.Services.AddSingleton<SieveProcessor>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,  // Ensures token expiration is checked
            ValidateIssuerSigningKey = true,

            // Set the valid issuer and audience from the configuration
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            // The security key to validate the token signature
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),

            // Optional: Allow for some clock skew when validating expiration (default is 5 minutes)
            ClockSkew = TimeSpan.Zero // Optional: Set to zero if you don't want any clock skew allowance
        };

        // Optional: Define how token errors are handled (such as expired tokens)
        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                // Log or handle token authentication failure
                if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                {
                    // Handle token expiration logic if needed
                    Console.WriteLine("Token expired.");
                }
                return Task.CompletedTask;
            }
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

app.UseAuthorization();

app.UseCors();

app.MapControllers();

app.Run();
