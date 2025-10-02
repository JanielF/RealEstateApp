using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Enums.Upload;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Services
{
    public class RealEstatePropertyService : GenericService<SaveRealEstatePropertyViewModel, RealEstatePropertyViewModel, RealEstateProperty>, IRealEstatePropertyService
    {
        private readonly IRealEstatePropertyRepository _repository;
        private readonly IPropertyImageRepository _imageRepository;
        private readonly IPropertyUpgradeRepository _upgradeRepository;
        private readonly IFavoritePropertyRepository _favoriteRepository;
        public RealEstatePropertyService(IRealEstatePropertyRepository repo, IMapper mapper, IPropertyImageRepository imageRepository, IPropertyUpgradeRepository upgradeRepository, IFavoritePropertyRepository favoriteRepository) : base(repo, mapper)
        {
            _repository = repo;
            _imageRepository = imageRepository;
            _upgradeRepository = upgradeRepository;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<List<RealEstatePropertyViewModel>> GetByAgentAsync(string agentId)
        {
            var result = _mapper.Map<List<RealEstatePropertyViewModel>>(await _repository.GetByAgentAsync(agentId));
            return result;
        }

        public async Task<RealEstatePropertyViewModel> GetByGuidAsync(string guid)
        {
            return _mapper.Map<RealEstatePropertyViewModel>(await _repository.GetByGuidAsync(guid));
        }

        public override async Task<SaveRealEstatePropertyViewModel> GetByIdSaveViewModelAsync(int id)
        {
            return _mapper.Map<SaveRealEstatePropertyViewModel>(await _repository.GetByIdWithIncludeAsync(id, new List<string> { "Images", "Upgrades" }));
        }

        public async override Task<SaveRealEstatePropertyViewModel> CreateAsync(SaveRealEstatePropertyViewModel viewModel)
        {
            SaveRealEstatePropertyViewModel result = new();
            RealEstateProperty propertyWithSameGuid;
            string guid;
            do
            {
                guid = GuidHelper.Guid();
                propertyWithSameGuid = await _repository.GetByGuidAsync(guid);

            } while (propertyWithSameGuid != null);
            viewModel.Guid = guid;


            var upgrades = viewModel.Upgrades;
            viewModel.Upgrades = null;
            var createdProperty = await _repository.CreateAsync(_mapper.Map<RealEstateProperty>(viewModel));
            result = _mapper.Map<SaveRealEstatePropertyViewModel>(createdProperty);
            if (createdProperty != null && createdProperty.Id != default)
            {
                if (viewModel.Images != null)
                {

                    foreach (var image in viewModel.Images)
                    {
                        var imagePath = UploadHelper.UploadFile(image, createdProperty.Id.ToString(), nameof(UploadEntities.RealEstateProperty));
                        await _imageRepository.CreateAsync(new PropertyImage
                        {
                            ImagePath = imagePath,
                            PropertyId = createdProperty.Id
                        });
                        if (result.ImagesPath == null)
                            result.ImagesPath = new();
                        result.ImagesPath.Add(imagePath);

                    }
                }
                if (upgrades != null)
                {

                    foreach (var upgrade in upgrades)
                    {
                        await _upgradeRepository.CreateAsync(new PropertyUpgrade
                        {
                            UpgradeId = upgrade,
                            PropertyId = createdProperty.Id
                        });
                        if (result.Upgrades == null)
                            result.Upgrades = new();
                        result.Upgrades.Add(upgrade);

                    }
                }
            }

            return result;
        }

        public override async Task<SaveRealEstatePropertyViewModel> UpdateAsync(SaveRealEstatePropertyViewModel viewModel, int id)
        {
            SaveRealEstatePropertyViewModel result = new();
            if ((viewModel.Images?.Count() ?? 0) + (viewModel.ImagesPath?.Count() ?? 0) > 4)
            {
                result.Error = "You can't have more than 4 images";
                result.HasError = true;
                return result;
            }
            var originalProperty = await _repository.GetByIdWithIncludeAsync(viewModel.Id ?? 0, new List<string> { "TypeOfSale", "TypeProperty", "Upgrades", "Images" });
            if (originalProperty == null)
            {
                result.Error = "We could't found the property to update";
                result.HasError = true;
                return result;
            }
            for (int i = 0; i < originalProperty.Images.Count(); i++)
            {
                var image = originalProperty.Images.ElementAt(i);

                if (viewModel.ImagesPath == null || !viewModel.ImagesPath.Contains(image.ImagePath))
                {
                    await _imageRepository.DeleteAsync(image);
                }
            }

            for (int i = 0; i < originalProperty.Upgrades.Count(); i++)
            {
                var upgrade = originalProperty.Upgrades.ElementAt(i);
                if (!viewModel.Upgrades.Contains(upgrade.UpgradeId))
                {
                    await _upgradeRepository.DeleteAsync(upgrade);
                }
            }

            if (viewModel.Upgrades != null && viewModel.Upgrades.Count() > 0)
            {
                foreach (var upgrade in viewModel.Upgrades)
                {
                    if (!originalProperty.Upgrades.Any(x => x.UpgradeId == upgrade))
                    {

                        await _upgradeRepository.CreateAsync(new PropertyUpgrade
                        {
                            UpgradeId = upgrade,
                            PropertyId = viewModel.Id.Value
                        });
                    }
                }
            }

            viewModel.Guid = originalProperty.Guid;
            viewModel.AgentId = originalProperty.AgentId;
            viewModel.AgentName = originalProperty.AgentName;
            viewModel.Address = originalProperty.Address;

            result = await base.UpdateAsync(viewModel, id);

            if (result != null && result.Id != null)
            {
                if (viewModel.Images != null && viewModel.Images.Count() > 0)
                {
                    foreach (var image in viewModel.Images)
                    {
                        var imagePath = UploadHelper.UploadFile(image, result.Id.Value.ToString(), nameof(UploadEntities.RealEstateProperty));
                        if (imagePath != null && imagePath.Count() > 0)
                        {

                            await _imageRepository.CreateAsync(new PropertyImage
                            {
                                ImagePath = imagePath,
                                PropertyId = result.Id.Value
                            });
                            if (result.ImagesPath == null)
                                result.ImagesPath = new();

                            result.ImagesPath.Add(imagePath);
                        }
                    }
                }
            }
            else
            {
                result = new();
                result.Error = "There was an error editing the property";
                result.HasError = true;
                return result;
            }


            return result;
        }

        public override async Task DeleteAsync(int id)
        {
            var images = await _imageRepository.GetAllByProperty(id);
            if (images != null && images.Count() > 0)
            {

                for (int i = 0; i < images.Count; i++)
                {
                    await _imageRepository.DeleteAsync(images[i]);
                }
            }
            var upgrades = await _upgradeRepository.GetAllByProperty(id);
            for (int i = 0; i < upgrades.Count; i++)
            {
                await _upgradeRepository.DeleteAsync(upgrades[i]);
            }
            var favorite = await _favoriteRepository.GetAllByProperty(id);
            for (int i = 0; i < favorite.Count; i++)
            {
                await _favoriteRepository.DeleteAsync(favorite[i]);
            }
            await base.DeleteAsync(id);

        }
        public async Task DeleteAsync(string agentId)
        {
            var properties = await _repository.GetByAgentAsync(agentId);
            foreach (var property in properties)
            {

                var images = await _imageRepository.GetAllByProperty(property.Id);
                if (images != null && images.Count() > 0)
                {

                    for (int i = 0; i < images.Count; i++)
                    {
                        await _imageRepository.DeleteAsync(images[i]);
                    }
                }
                var upgrades = await _upgradeRepository.GetAllByProperty(property.Id);
                for (int i = 0; i < upgrades.Count; i++)
                {
                    await _upgradeRepository.DeleteAsync(upgrades[i]);
                }
                var favorite = await _favoriteRepository.GetAllByProperty(property.Id);
                for (int i = 0; i < favorite.Count; i++)
                {
                    await _favoriteRepository.DeleteAsync(favorite[i]);
                }
                await base.DeleteAsync(property.Id);

            }
        }

        public async Task<List<RealEstatePropertyViewModel>> GetAllByFilter(RealEstatePropertyFilterViewModel filter)
        {
            return _mapper.Map<List<RealEstatePropertyViewModel>>(await _repository.GetAllByFilter(_mapper.Map<RealEstatePropertyFilterDTO>(filter)));
        }
        public async Task<RealEstatePropertyViewModel> GetAllByIdWithIncludeAsync(int id)
        {
            return _mapper.Map<RealEstatePropertyViewModel>(await _repository.GetByIdWithIncludeAsync(id));
        }
        public async Task<List<RealEstatePropertyViewModel>> GetAllWithIncludeAsync(List<string> properties)
        {
            return _mapper.Map<List<RealEstatePropertyViewModel>>(await _repository.GetAllWithIncludeAsync(properties));
        }
        public async Task<List<RealEstatePropertyViewModel>> GetAllWithIncludeAsync()
        {
            return _mapper.Map<List<RealEstatePropertyViewModel>>(await _repository.GetAllWithIncludeAsync());
        }
        public async Task<int> GetTotalProperties()
        {
            return await _repository.GetTotalProperties();
        }

        public async Task<int> GetTotalPropertiesByAgent(string agentId)
        {
            return await _repository.GetTotalPropertiesByAgent(agentId);
        }
    }
}
