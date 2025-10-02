using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class TypeOfPropertyRepository : GenericRepository<TypeOfProperty>, ITypeOfPropertyRepository
    {
        public TypeOfPropertyRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
