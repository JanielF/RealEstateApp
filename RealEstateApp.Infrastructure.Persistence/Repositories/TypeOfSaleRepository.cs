using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class TypeOfSaleRepository : GenericRepository<TypeOfSale>, ITypeOfSaleRepository
    {
        public TypeOfSaleRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
