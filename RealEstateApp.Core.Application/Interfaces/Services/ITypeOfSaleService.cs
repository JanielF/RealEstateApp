using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface ITypeOfSaleService : IGenericService<SaveTypeOfSaleViewModel,TypeOfSaleViewModel,TypeOfSale>
    {
        Task<List<TypeOfSaleViewModel>> GetAllWithIncludeAsync(List<string> properties);
    }
}
