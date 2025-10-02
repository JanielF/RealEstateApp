using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyUpgradeRepository : GenericRepository<PropertyUpgrade>, IPropertyUpgradeRepository
    {
        public PropertyUpgradeRepository(ApplicationContext context) : base(context)
        {
        }
        public async Task<List<PropertyUpgrade>> GetAllByProperty(int propertyId)
        {
            return await _dbSet.Where(x => x.PropertyId == propertyId).ToListAsync();
        }
        public async Task<List<PropertyUpgrade>> GetAllByUpgrade(int upgradeId)
        {
            return await _dbSet.Where(x => x.UpgradeId == upgradeId).ToListAsync();
        }
    }
}
