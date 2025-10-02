using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.Upgrade;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUpgradeService : IGenericService<SaveUpgradeViewModel, UpgradeViewModel, Upgrade>
    {
    }
}
