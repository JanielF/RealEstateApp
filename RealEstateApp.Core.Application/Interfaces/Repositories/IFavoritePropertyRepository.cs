using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Interfaces.Repositories
{
    public interface IFavoritePropertyRepository : IGenericRepository<FavoriteProperty>
    {
        Task<List<RealEstateProperty>> GetAllPropertyByUser(string userId);
        Task<List<FavoriteProperty>> GetAllByProperty(int propertyId);

        Task<List<string>> GetAllUserIdByProperty(int propertyId);
        Task<List<int>> GetAllPropertyIdByUser(string userId);
        Task<FavoriteProperty> GetFavorite(int propertyId, string userId);

    }
}
