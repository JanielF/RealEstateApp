using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Infrastructure.Identity.Contexts;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.Infrastructure.Identity.Models;
using RealEstateApp.Infrastructure.Identity.Services;
using RealEstateApp.Core.Domain.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RealEstateApp.Core.Application.Dtos.Account;
using System.Reflection;
using Azure.Core;

namespace RealEstateApp.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            //Configurar el appsettings.json

            #region Contexts
            if (config.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDB"));

            }
            else
            {
                services.AddDbContext<IdentityContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(config.GetConnectionString("IdentityConnection"),
                    m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));
                });

            }
            #endregion

            #region Mapings
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            #endregion

            #region Identity
            services.AddIdentity<RealEstateUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccessDenied";
                options.ReturnUrlParameter = "/AccessDenied";
            });

            services.Configure<JWTSettings>(config.GetSection("JWTSettings"));
            if (config.GetValue<bool>("UseBearerToken"))
            {
                services.AddAuthentication(opt =>
                {
                    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                }).AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = true;
                    opt.SaveToken = false;
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = config["JWTSettings:Issuer"],
                        ValidAudience = config["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:Key"]))
                    };
                    opt.Events = new JwtBearerEvents()
                    {
                        OnAuthenticationFailed = c =>
                        {
                            c.NoResult();
                            c.Response.StatusCode = StatusCodes.Status500InternalServerError;
                            c.Response.ContentType = ContentType.TextPlain.ToString();
                            return c.Response.WriteAsync(c.Exception.ToString());
                        },
                        OnChallenge = c =>
                        {
                            c.HandleResponse();
                            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            c.Response.ContentType = ContentType.ApplicationJson.ToString();
                            var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not authorized" });
                            return c.Response.WriteAsync(result);
                        },
                        OnForbidden = c =>
                        {
                            c.Response.StatusCode = StatusCodes.Status403Forbidden;
                            c.Response.ContentType = ContentType.ApplicationJson.ToString();
                            var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not authorized to access this resource" });
                            return c.Response.WriteAsync(result);
                        }
                    };


                });

            }
            else
            {

                services.AddAuthentication();
            }

            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion
        }
        public static void AddIdentityInfrastructureTesting(this IServiceCollection services)
        {

            #region Contexts

            services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDB"));

            #endregion

            #region Mapings

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #endregion

            #region Identity
            services.AddIdentity<RealEstateUser, IdentityRole>()
                .AddEntityFrameworkStores<IdentityContext>()
                .AddDefaultTokenProviders();


            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/User";
                options.AccessDeniedPath = "/User/AccessDenied";
                options.ReturnUrlParameter = "/AccessDenied";
            });

            //services.Configure<JWTSettings>(config.GetSection("JWTSettings"));
            services.AddAuthentication();
            //services.AddAuthentication(opt =>
            //{
            //    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            //}).AddJwtBearer(opt =>
            //{
            //    opt.RequireHttpsMetadata = true;
            //    opt.SaveToken = false;
            //    opt.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = true,
            //        ValidateIssuerSigningKey = true,
            //        ValidateAudience = true,
            //        ValidateLifetime = true,
            //        ClockSkew = TimeSpan.Zero,
            //        ValidIssuer = config["JWTSettings:Issuer"],
            //        ValidAudience = config["JWTSettings:Audience"],
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSettings:Key"]))
            //    };
            //    opt.Events = new JwtBearerEvents()
            //    {


            //        OnAuthenticationFailed = c =>
            //        {
            //            c.NoResult();
            //            c.Response.StatusCode = StatusCodes.Status500InternalServerError;
            //            c.Response.ContentType = ContentType.TextPlain.ToString();
            //            return c.Response.WriteAsync(c.Exception.ToString());
            //        },
            //        OnChallenge = c =>
            //        {
            //            c.HandleResponse();
            //            c.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //            c.Response.ContentType = ContentType.ApplicationJson.ToString();
            //            var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not authorized" });
            //            return c.Response.WriteAsync(result);
            //        },
            //        OnForbidden = c =>
            //        {
            //            c.Response.StatusCode = StatusCodes.Status403Forbidden;
            //            c.Response.ContentType = ContentType.ApplicationJson.ToString();
            //            var result = JsonConvert.SerializeObject(new JwtResponse { HasError = true, Error = "You are not authorized to access this resource" });
            //            return c.Response.WriteAsync(result);
            //        }
            //    };


            //});
            #endregion

            services.AddTransient<IAccountService, AccountService>();

        }
    }
}
