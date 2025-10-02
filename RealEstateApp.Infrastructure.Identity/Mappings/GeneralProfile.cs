using AutoMapper;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.ViewModels.Account;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Identity.Models;

namespace RealEstateApp.Infrastructure.Identity.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile() 
        {



            CreateMap<RealEstateUser, UserViewModel>()
                .ReverseMap();

            CreateMap<RealEstateUser, SaveUserViewModel>()
                .ReverseMap();

            CreateMap<RealEstateUser, UserDTO>()
                .ReverseMap();

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();
            CreateMap<RealEstateProperty, RealEstateRequest>()
                .ForMember(x => x.Images, opt => opt.MapFrom(src => src.Images.Select(x => x.ImagePath)))
                .ReverseMap();
        }



    }
}
