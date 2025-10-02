using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Dtos.Entities.TypeProperty;
using RealEstateApp.Core.Application.Dtos.Entities.TypeSale;
using RealEstateApp.Core.Application.Dtos.Entities.Upgrade;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeProperty;
using RealEstateApp.Core.Application.Features.TypesOfSales.Commands.CreateTypeOfSale;
using RealEstateApp.Core.Application.Features.TypesOfSales.Commands.UpdateTypeOfSale;
using RealEstateApp.Core.Application.Features.Upgrades.Commands.CreateUpgrade;
using RealEstateApp.Core.Application.Features.Upgrades.Commands.UpdateUpgrade;
using RealEstateApp.Core.Application.ViewModels.Account;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.Upgrade;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile()
        {
            #region RealEstateProfile

            CreateMap<RealEstateProperty, RealEstatePropertyViewModel>()
                .ForMember(x => x.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.ImagePath)))
                .ReverseMap();

            CreateMap<RealEstatePropertyViewModel, RealEstatePropertyDetailsViewModel>()
                .ForMember(src => src.Agent, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<RealEstateProperty, SaveRealEstatePropertyViewModel>()
                .ForMember(x => x.ImagesPath, opt => opt.MapFrom(src => src.Images.Select(x => x.ImagePath).ToList()))
                .ForMember(x => x.Upgrades, opt => opt.MapFrom(src => src.Upgrades.Select(x => x.UpgradeId).ToList()))
                .ForMember(x => x.Images, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.Upgrades, opt => opt.MapFrom(src => MapUpgrades(src)))
                .ForMember(x => x.Images, opt => opt.MapFrom(src => MapImages(src)));

            CreateMap<RealEstatePropertyFilterViewModel, RealEstatePropertyFilterDTO>()
                .ReverseMap();
            CreateMap<RealEstateProperty, RealEstateRequest>()
                .ForMember(x => x.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.ImagePath)))
                .ReverseMap();
            
            CreateMap<PropertyUpgrade, RealEstatePropertyViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.PropertyId))
                .ForMember(x => x.AgentId, opt => opt.MapFrom(src => src.Property.AgentId))
                .ForMember(x => x.AgentName, opt => opt.MapFrom(src => src.Property.AgentName))
                .ForMember(x => x.TypePropertyId, opt => opt.MapFrom(src => src.Property.TypePropertyId))
                .ForMember(x => x.NumberOfBathrooms, opt => opt.MapFrom(src => src.Property.NumberOfBathrooms))
                .ForMember(x => x.TypeOfSaleId, opt => opt.MapFrom(src => src.Property.TypeOfSaleId))
                .ForMember(x => x.Updated, opt => opt.MapFrom(src => src.Property.Updated))
                .ForMember(x => x.Created, opt => opt.MapFrom(src => src.Property.Created))
                .ForMember(x => x.Size, opt => opt.MapFrom(src => src.Property.Size))
                .ForMember(x => x.Price, opt => opt.MapFrom(src => src.Property.Price))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Property.Description))
                .ForMember(x => x.Guid, opt => opt.MapFrom(src => src.Property.Guid))
                .ForMember(x => x.NumberOfBedrooms, opt => opt.MapFrom(src => src.Property.NumberOfBedrooms))
                .ForMember(x => x.Images, opt => opt.Ignore())
                .ForMember(x => x.TypeOfSale, opt => opt.Ignore())
                .ForMember(x => x.TypeProperty, opt => opt.Ignore())
                .ForMember(x => x.Upgrades, opt => opt.Ignore());
            #endregion

            #region User
            CreateMap<SaveUserViewModel, UserRegisterRequest>()
                .ReverseMap();

            CreateMap<SaveUserViewModel, UserEditRequest>()
                .ReverseMap();

            #endregion

            #region TypeOfPropertyProfile

            CreateMap<TypeOfProperty, TypeOfPropertyViewModel>()
                .ForMember(x => x.QuantityOfProperties, opt => opt.MapFrom(src => src.Properties.Count()))
                .ReverseMap();

            CreateMap<TypeOfProperty, SaveTypeOfPropertyViewModel>()
                .ReverseMap();

            CreateMap<TypeOfProperty, TypePropertyRequest>()
                .ReverseMap()
                .ForMember(x => x.Properties, opt => opt.Ignore());

            #endregion

            #region TypeOfSaleProfile

            CreateMap<TypeOfSale, TypeOfSaleViewModel>()
                .ForMember(x => x.QuantityOfProperties, opt => opt.MapFrom(src => src.Properties.Count()))
                .ReverseMap();

            CreateMap<TypeOfSale, SaveTypeOfSaleViewModel>()
                .ReverseMap();

            CreateMap<TypeOfSale, TypeSaleRequest>()
                .ReverseMap()
                .ForMember(x => x.Properties, opt => opt.Ignore());

            #endregion

            #region UpgradeProfile

            CreateMap<Upgrade, UpgradeViewModel>()
                .ReverseMap();

            CreateMap<Upgrade, SaveUpgradeViewModel>()
                .ReverseMap();

            CreateMap<PropertyUpgrade, UpgradeViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.UpgradeId))
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Upgrade.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Upgrade.Description))
                .ForMember(x => x.Properties, opt => opt.Ignore());

            CreateMap<Upgrade, UpgradeRequest>()
                .ReverseMap()
                .ForMember(x => x.Properties, opt => opt.Ignore());

            #endregion

            #region FavoritePropertyProfile
            CreateMap<FavoriteProperty, CreateFavoritePropertyViewModel>()
                .ReverseMap();
            #endregion

            #region CQRS

            #region CQRS RealEstateProperty



            #endregion

            #region CQRS TypeOfSale

            CreateMap<CreateTypeOfSaleCommand, TypeOfSale>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateTypeOfSaleCommand, TypeOfSale>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TypeOfSaleUpdateResponse, TypeOfSale>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TypeOfSale, TypeSaleRequest>().ReverseMap();
            #endregion

            #region CQRS TypeOfProperty

            CreateMap<CreateTypeOfPropertyCommand, TypeOfProperty>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateTypeOfPropertyCommand, TypeOfProperty>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TypeOfPropertyUpdateResponse, TypeOfProperty>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<TypeOfProperty, TypePropertyRequest>().ReverseMap();



            #endregion

            #region CQRS Upgrade

            CreateMap<CreateUpgradeCommand, Upgrade>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpdateUpgradeCommand, Upgrade>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<UpgradeUpdateResponse, Upgrade>()
                .ForMember(x => x.Properties, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<PropertyUpgrade, UpgradeRequest>()
                .ForMember(x => x.Name, opt => opt.MapFrom(src => src.Upgrade.Name))
                .ForMember(x => x.Description, opt => opt.MapFrom(src => src.Upgrade.Description));



            #endregion

            #endregion
        }

        private ICollection<PropertyImage> MapImages(SaveRealEstatePropertyViewModel source)
        {
            var images = new List<PropertyImage>();

            if (source.ImagesPath != null)
            {
                foreach (var imagePath in source.ImagesPath)
                {
                    var image = new PropertyImage
                    {
                        ImagePath = imagePath,
                        PropertyId = source.Id ?? 0
                    };
                    images.Add(image);
                }
            }

            return images;
        }
        private ICollection<PropertyUpgrade> MapUpgrades(SaveRealEstatePropertyViewModel source)
        {
            var upgrades = new List<PropertyUpgrade>();

            if (source.Upgrades != null)
            {
                foreach (var upgradeId in source.Upgrades)
                {
                    var upgrade = new PropertyUpgrade
                    {
                        UpgradeId = upgradeId,
                        PropertyId = source.Id ?? 0
                    };
                    upgrades.Add(upgrade);
                }
            }

            return upgrades;
        }
    }
}
