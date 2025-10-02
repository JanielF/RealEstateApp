using AutoMapper;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Domain.Models;

namespace RealEstateApp.Core.Application.Services
{
    public class TypeOfSaleService : GenericService<SaveTypeOfSaleViewModel, TypeOfSaleViewModel, TypeOfSale>, ITypeOfSaleService
    {
        public TypeOfSaleService(ITypeOfSaleRepository repo, IMapper mapper) : base(repo, mapper)
        {
        }
        public async Task<List<TypeOfSaleViewModel>> GetAllWithIncludeAsync(List<string> properties)
        {
            return _mapper.Map<List<TypeOfSaleViewModel>>(await _repo.GetAllWithIncludeAsync(properties));

        }
    }
}
