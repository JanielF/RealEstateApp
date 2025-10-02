using RealEstateApp.Core.Application.Dtos.Entities.FavoriteProperty;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IFavoritePropertyService
    {
        Task<CreateFavoriteResponse> CreateFavoriteAsync(CreateFavoritePropertyViewModel vm);
        Task<DeleteFavoriteResponse> DeleteAsync(int favoriteId);
        Task<List<RealEstatePropertyViewModel>> GetAllPropertyByUser(string userId);
        Task<List<string>> GetAllUserIdByProperty(int propertyId);
        Task<List<int>> GetAllPropertyIdByUser(string userId);
        Task<DeleteFavoriteResponse> DeleteAsync(int propertyId, string userId);
    }
}
