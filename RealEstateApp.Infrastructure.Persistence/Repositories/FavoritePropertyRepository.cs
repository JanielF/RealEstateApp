using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class FavoritePropertyRepository : GenericRepository<FavoriteProperty>, IFavoritePropertyRepository
    {
        public FavoritePropertyRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<FavoriteProperty> GetFavorite(int propertyId, string userId)
        {

            return await _dbSet
                .FirstOrDefaultAsync(x => x.UserId == userId && x.PropertyId == propertyId);
        }

        public async Task<List<string>> GetAllUserIdByProperty(int propertyId)
        {
            return await _dbSet
                .Where(x => x.PropertyId == propertyId)
                .Select(x => x.UserId)
                .ToListAsync();
        }

        public async Task<List<RealEstateProperty>> GetAllPropertyByUser(string userId)
        {
            var result = await _dbSet
                .Include(x => x.Property)
                .ThenInclude(x => x.Upgrades)
                .ThenInclude(x => x.Upgrade)
                .Include(x => x.Property)
                .ThenInclude(x => x.Images)
                .Include(x => x.Property)
                .ThenInclude(x => x.TypeProperty)
                .Include(x => x.Property)
                .ThenInclude(x => x.TypeOfSale)
                .Where(x => x.UserId == userId)
                .Select(x => x.Property)
                .ToListAsync();
            return result;
        }
        public async Task<List<int>> GetAllPropertyIdByUser(string userId)
        {
            return await _dbSet
                .Where(x => x.UserId == userId)
                .Select(x => x.PropertyId)
                .ToListAsync();
        }

        public async Task<List<FavoriteProperty>> GetAllByProperty(int propertyId)
        {
            return await _dbSet
                .Where(x => x.PropertyId == propertyId)
                .ToListAsync();
        }
    }
}
