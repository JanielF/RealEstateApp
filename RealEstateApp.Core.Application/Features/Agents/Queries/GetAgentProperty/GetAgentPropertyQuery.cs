using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperty
{
    /// <summary>
    /// Parámetros para obtener las propiedades por su agente
    /// </summary>
    public class GetAgentPropertyQuery : IRequest<Response<IEnumerable<RealEstateRequest>>>
    {
        [SwaggerParameter(Description = "El id del agente para encontrar las propiedades")]
        public string Id { get; set; }
    }

    public class GetAgentPropertyQueryHandler : IRequestHandler<GetAgentPropertyQuery, Response<IEnumerable<RealEstateRequest>>>
    {
        private readonly IRealEstatePropertyRepository _realEstatePropertyRepository;
        private readonly IMapper _mapper;

        public GetAgentPropertyQueryHandler(IRealEstatePropertyRepository realEstatePropertyRepository, IMapper mapper)
        {
            _realEstatePropertyRepository = realEstatePropertyRepository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<RealEstateRequest>>> Handle(GetAgentPropertyQuery request, CancellationToken cancellationToken)
        {
            var properties = _mapper.Map<List<RealEstateRequest>>(await _realEstatePropertyRepository.GetByAgentAsync(request.Id));
            if (properties == null) throw new ApiException($"There are no properties from this agent.", (int)HttpStatusCode.NoContent);
            return new Response<IEnumerable<RealEstateRequest>>(properties);
        }
    }
}
