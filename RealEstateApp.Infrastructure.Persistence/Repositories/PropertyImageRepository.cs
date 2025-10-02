using Microsoft.EntityFrameworkCore;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class PropertyImageRepository : GenericRepository<PropertyImage>, IPropertyImageRepository
    {
        public PropertyImageRepository(ApplicationContext context) : base(context)
        {
        }

        public async Task<List<PropertyImage>> GetAllByProperty(int propertyId)
        {
            return await _dbSet.Where(x => x.PropertyId == propertyId).ToListAsync();
        }
    }
}
