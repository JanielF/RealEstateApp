using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class RealEstatePropertyRepository : GenericRepository<RealEstateProperty>, IRealEstatePropertyRepository
    {
        public RealEstatePropertyRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<RealEstateProperty>> GetByAgentAsync(string agentId)
        {
            return await _dbSet.Include(x => x.Upgrades)
                .ThenInclude(x => x.Upgrade)
                .Include(x => x.Images)
                .Include(x => x.TypeProperty)
                .Include(x => x.TypeOfSale)
                .Where(x => x.AgentId == agentId)
                .ToListAsync();
        }

        public async Task<RealEstateProperty> GetByGuidAsync(string guid)
        {
            return await _dbSet.Include(x => x.Upgrades)
                .ThenInclude(x => x.Upgrade)
                .Include(x => x.Images)
                .Include(x => x.TypeProperty)
                .Include(x => x.TypeOfSale)
                .FirstOrDefaultAsync(x => x.Guid == guid);
        }
        public virtual async Task<RealEstateProperty> GetByIdWithIncludeAsync(int id,List<string> properties)
        {
            var query = _dbSet.AsQueryable();
            foreach (var property in properties)
            {
                query = query.Include(property);
            }
            return await query.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<RealEstateProperty>> GetAllWithIncludeAsync()
        {
            return await _dbSet.Include(x => x.Upgrades)
                .ThenInclude(x => x.Upgrade)
                .Include(x => x.Images)
                .Include(x => x.TypeProperty)
                .Include(x => x.TypeOfSale)
                .ToListAsync();
        }
        public async Task<RealEstateProperty> GetByIdWithIncludeAsync(int id)
        {
            return await _dbSet.Include(x => x.Upgrades)
                .ThenInclude(x => x.Upgrade)
                .Include(x => x.Images)
                .Include(x => x.TypeProperty)
                .Include(x => x.TypeOfSale)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<List<RealEstateProperty>> GetAllByFilter(RealEstatePropertyFilterDTO filter)
        {
            return await _dbSet.Include(x => x.Upgrades)
                .ThenInclude(x => x.Upgrade)
                .Include(x => x.Images)
                .Include(x => x.TypeProperty)
                .Include(x => x.TypeOfSale)
                .Where(p =>
                (filter.TypeOfProperty <= 0 ? (true) : p.TypePropertyId == filter.TypeOfProperty) &&
                (filter.MinPrice <= 0 ? (true) : p.Price >= filter.MinPrice) &&
                (filter.MaxPrice <= 0 ? (true) : p.Price <= filter.MaxPrice) &&
                (filter.NumberOfBathrooms <= 0 ? (true) : p.NumberOfBathrooms == filter.NumberOfBathrooms) &&
                (filter.NumberOfBedrooms <= 0 ? (true) : p.NumberOfBedrooms == filter.NumberOfBedrooms))
                .ToListAsync(); 
        }
        public async Task<int> GetTotalProperties()
        {
            return await _dbSet.CountAsync();
        }

        public async Task<int> GetTotalPropertiesByAgent(string agentId)
        {
            return await _dbSet.Where(x => x.AgentId == agentId).CountAsync();
        }

    }
}
