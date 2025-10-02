using MediatR;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using System.Reflection;

namespace RealEstateApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {
            #region Mapping
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());
            #endregion

            #region Services
            services.AddTransient<IRealEstatePropertyService, RealEstatePropertyService>();
            services.AddTransient<ITypeOfPropertyService, TypeOfPropertyService>();
            services.AddTransient<ITypeOfSaleService, TypeOfSaleService>();
            services.AddTransient<IUpgradeService, UpgradeService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IFavoritePropertyService, FavoritePropertyService>();


            #endregion
        }
    }
}
