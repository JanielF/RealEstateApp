using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Entities.FavoriteProperty;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Services
{
    public class FavoritePropertyService : IFavoritePropertyService
    {
        private readonly IFavoritePropertyRepository _repository;
        private readonly IMapper _mapper;

        public FavoritePropertyService(IFavoritePropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<CreateFavoriteResponse> CreateFavoriteAsync(CreateFavoritePropertyViewModel vm)
        {
            CreateFavoriteResponse response = new();
            var creationResponse = await _repository.CreateAsync(_mapper.Map<FavoriteProperty>(vm));
            if(creationResponse == null)
            {
                response.Error = "Was an error while saving your favorite";
                response.HasError = true;
                return response;
            }
            return response;
        }

        public async Task<DeleteFavoriteResponse> DeleteAsync(int favoriteId)
        {
            DeleteFavoriteResponse response = new();
            var propertyFavorite = await _repository.GetByIdAsync(favoriteId);
            if(propertyFavorite == null)
            {
                response.Error = $"There's no favorite property with id {favoriteId}";
                response.HasError = true;
                return response;
            }
            await _repository.DeleteAsync(propertyFavorite);

            return response;
        }
        public async Task<DeleteFavoriteResponse> DeleteAsync(int propertyId, string userId)
        {
            DeleteFavoriteResponse response = new();
            var propertyFavorite = await _repository.GetFavorite(propertyId,userId);
            if (propertyFavorite == null)
            {
                response.Error = $"There's no favorite property that fulfil the conditions";
                response.HasError = true;
                return response;
            }
            await _repository.DeleteAsync(propertyFavorite);

            return response;
        }

        public async Task<List<string>> GetAllUserIdByProperty(int propertyId)
        {
            return await _repository.GetAllUserIdByProperty(propertyId);
        }

        public async Task<List<RealEstatePropertyViewModel>> GetAllPropertyByUser(string userId)
        {
            return  _mapper.Map<List<RealEstatePropertyViewModel>>(await _repository.GetAllPropertyByUser(userId));
        }

        public async Task<List<int>> GetAllPropertyIdByUser(string userId)
        {
            return await _repository.GetAllPropertyIdByUser(userId);
        }
    }
}
