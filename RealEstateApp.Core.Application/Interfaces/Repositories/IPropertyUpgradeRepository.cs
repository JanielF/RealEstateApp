using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyUpgradeRepository : IGenericRepository<PropertyUpgrade>
    {
        Task<List<PropertyUpgrade>> GetAllByProperty(int propertyId);
        Task<List<PropertyUpgrade>> GetAllByUpgrade(int upgradeId);
    }
}
