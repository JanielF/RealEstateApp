using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IPropertyImageRepository : IGenericRepository<PropertyImage>
    {
        Task<List<PropertyImage>> GetAllByProperty(int propertyId);
    }
}
