using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.Upgrade;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Upgrades.Queries.GetAllUpgrades
{
    public class GetAllUpgradesQuery : IRequest<Response<IEnumerable<UpgradeRequest>>>
    {

    }

    public class GetAllUpgradesQueryHandler : IRequestHandler<GetAllUpgradesQuery, Response<IEnumerable<UpgradeRequest>>>
    {
        private readonly IUpgradeRepository _repository;
        private readonly IMapper _mapper;

        public GetAllUpgradesQueryHandler(IUpgradeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<UpgradeRequest>>> Handle(GetAllUpgradesQuery request, CancellationToken cancellationToken)
        {
            var typeSales = _mapper.Map<List<UpgradeRequest>>(await _repository.GetAllAsync());
            if (typeSales.Count is 0) throw new ApiException($"There are no upgrades.", (int)HttpStatusCode.NoContent);
            return new Response<IEnumerable<UpgradeRequest>>(typeSales);
        }
    }
}
