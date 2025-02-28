﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Infrastructure;

public static class ServiceRegisteration
{
    public static IServiceCollection AddServiceRegisteration(this IServiceCollection services, IConfiguration configuration)
    {
        #region 1). Identity Services
        services.AddIdentity<User, Role>(option =>
        {
            // Password settings.
            option.Password.RequireDigit = true;
            option.Password.RequireLowercase = true;
            option.Password.RequireNonAlphanumeric = true;
            option.Password.RequireUppercase = true;
            option.Password.RequiredLength = 6;
            option.Password.RequiredUniqueChars = 1;

            // Lockout settings.
            option.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            option.Lockout.MaxFailedAccessAttempts = 5;
            option.Lockout.AllowedForNewUsers = true;

            // User settings.
            option.User.AllowedUserNameCharacters =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            option.User.RequireUniqueEmail = true;
            option.SignIn.RequireConfirmedEmail = true;

        }).AddEntityFrameworkStores<ApplicationDBContext>().AddDefaultTokenProviders();
        #endregion

        #region 2). JWT Authentication (Bind jwtSettings (appsettings) With Class (jwtSettings))

        //JWT Authentication
        var jwtSettings = new JwtSettings();
        var emailSettings = new EmailSettings();
        configuration.GetSection(nameof(jwtSettings)).Bind(jwtSettings);
        configuration.GetSection(nameof(emailSettings)).Bind(emailSettings);
        services.AddSingleton(jwtSettings);
        services.AddSingleton(emailSettings); 

        #endregion

        #region 3). configuration of JWT
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = jwtSettings.ValidateIssuer,
                ValidIssuers = new[] { jwtSettings.Issuer },
                ValidateIssuerSigningKey = jwtSettings.ValidateIssuerSigningKey,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Secret)),
                ValidAudience = jwtSettings.Audience,
                ValidateAudience = jwtSettings.ValidateAudience,
                ValidateLifetime = jwtSettings.ValidateLifeTime,
            };
        });
        #endregion

        #region 4). Scheme of Swagger
        //Swagger Gn
        services.AddSwaggerGen(c =>
        {
            // Basic Swagger configuration
            c.SwaggerDoc("v1", new OpenApiInfo 
            { 
                Title = "School Management System API", 
                Version = "v1",
                Description = "API for managing school system. Connect with me on LinkedIn: https://www.linkedin.com/in/ahmedeprahim/ or GitHub: https://github.com/AhmedIbrahim-tech",
                Contact = new OpenApiContact
                {
                    Name = "Ahmed Eprahim",
                    Email = "ahmedeprahim.official@gmail.com",
                    Url = new Uri("https://www.linkedin.com/in/ahmedeprahim/")

                }
            });
            c.EnableAnnotations();

            c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
            {
                //Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                Description = "Insert your JWT token without 'Bearer' prefix.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });


            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
             {
             new OpenApiSecurityScheme
             {
                 Reference = new OpenApiReference
                 {
                     Type = ReferenceType.SecurityScheme,
                     Id = JwtBearerDefaults.AuthenticationScheme
                 }
             },
             Array.Empty<string>()
             }
           });
        });
        #endregion

        return services;
    }
}