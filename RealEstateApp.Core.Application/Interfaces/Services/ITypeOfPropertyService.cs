using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface ITypeOfPropertyService : IGenericService<SaveTypeOfPropertyViewModel, TypeOfPropertyViewModel, TypeOfProperty>
    {
        Task<List<TypeOfPropertyViewModel>> GetAllWithIncludeAsync(List<string> properties);
    }
}
