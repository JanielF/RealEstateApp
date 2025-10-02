using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using System.Runtime.CompilerServices;

namespace RealEstateApp.Infrastructure.Persistence
{
    public static class ServiceRegistration
    {
        public static void AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            #region Contexts
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("RealEstateDB"));
            }
            else
            {
                services.AddDbContext<ApplicationContext>(options =>
                {
                    options.EnableSensitiveDataLogging();
                    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                        m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName));
                });
            }
            #endregion

            #region Repositories
            services.AddTransient<IPropertyUpgradeRepository, PropertyUpgradeRepository>();
            services.AddTransient<IRealEstatePropertyRepository, RealEstatePropertyRepository>();
            services.AddTransient<ITypeOfSaleRepository, TypeOfSaleRepository>();
            services.AddTransient<ITypeOfPropertyRepository, TypeOfPropertyRepository>();
            services.AddTransient<IUpgradeRepository, UpgradeRepository>();
            services.AddTransient<IPropertyImageRepository, PropertyImageRepository>();
            services.AddTransient<IFavoritePropertyRepository, FavoritePropertyRepository>();
            #endregion
        }

        public static void AddPersistenceLayerTest(this IServiceCollection services)
        {
            #region Contexts

            services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase("RealEstateDB"));

            #endregion

            #region Repositories
            services.AddTransient<IPropertyUpgradeRepository, PropertyUpgradeRepository>();
            services.AddTransient<IRealEstatePropertyRepository, RealEstatePropertyRepository>();
            services.AddTransient<ITypeOfSaleRepository, TypeOfSaleRepository>();
            services.AddTransient<ITypeOfPropertyRepository, TypeOfPropertyRepository>();
            services.AddTransient<IUpgradeRepository, UpgradeRepository>();
            services.AddTransient<IPropertyImageRepository, PropertyImageRepository>();
            services.AddTransient<IFavoritePropertyRepository, FavoritePropertyRepository>();
            #endregion
        }
    }
}
