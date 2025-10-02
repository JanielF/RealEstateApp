using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.Upgrade;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Upgrades.Queries.GetUpgradeById
{
    /// <summary>
    /// Parámetros para obtener una mejora por su id
    /// </summary>
    public class GetUpgradeByIdQuery : IRequest<Response<UpgradeRequest>>
    {
        [SwaggerParameter(Description = "El id de la mejora que quiere encontrar")]
        public int Id { get; set; }
    }

    public class GetUpgradeByIdQueryHandler : IRequestHandler<GetUpgradeByIdQuery, Response<UpgradeRequest>>
    {
        private readonly IUpgradeRepository _repository;
        private readonly IMapper _mapper;

        public GetUpgradeByIdQueryHandler(IUpgradeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<UpgradeRequest>> Handle(GetUpgradeByIdQuery request, CancellationToken cancellationToken)
        {
            var upgrade = _mapper.Map<UpgradeRequest>(await _repository.GetByIdAsync(request.Id));
            if (upgrade == null) throw new ApiException($"Upgrades not found.", (int)HttpStatusCode.NoContent);
            return new Response<UpgradeRequest>(upgrade);
        }
    }
}
