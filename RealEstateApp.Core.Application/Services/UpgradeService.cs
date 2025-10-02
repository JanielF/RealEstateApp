using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Upgrade;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Services
{
    public class UpgradeService : GenericService<SaveUpgradeViewModel, UpgradeViewModel, Upgrade>, IUpgradeService
    {
        private readonly IPropertyUpgradeRepository _propertyUpgradeRepository;
        public UpgradeService(IUpgradeRepository repo, IMapper mapper, IPropertyUpgradeRepository propertyUpgradeRepository) : base(repo, mapper)
        {
            _propertyUpgradeRepository = propertyUpgradeRepository;
        }

        public override async Task DeleteAsync(int id)
        {
            var propertyUpgrades = await _propertyUpgradeRepository.GetAllByUpgrade(id);
            if(propertyUpgrades != null && propertyUpgrades.Count() > 0)
            {
                for (int i = 0; i < propertyUpgrades.Count(); i++)
                {
                    await _propertyUpgradeRepository.DeleteAsync(propertyUpgrades[i]);
                }
            }
            await base.DeleteAsync(id);
        }
    }
}
