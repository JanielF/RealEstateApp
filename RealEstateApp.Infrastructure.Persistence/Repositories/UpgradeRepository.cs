using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence.Contexts;

namespace RealEstateApp.Infrastructure.Persistence.Repositories
{
    public class UpgradeRepository : GenericRepository<Upgrade>, IUpgradeRepository
    {
        public UpgradeRepository(ApplicationContext context) : base(context)
        {
        }
    }
}
