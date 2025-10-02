using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.PropertiesRealEstates.Queries.GetPropertyByCode
{
    /// <summary>
    /// Parámetros para obtener una propiedad por su codigo
    /// </summary>
    public class GetPropertyByCodeQuery : IRequest<Response<RealEstateRequest>>
    {
        [SwaggerParameter(Description = "El codigo de la propiedad que quiere encontrar")]
        public string Guid { get; set; }
    }

    public class GetPropertyByCodeQueryHandler : IRequestHandler<GetPropertyByCodeQuery, Response<RealEstateRequest>>
    {
        private readonly IRealEstatePropertyRepository _repositorty;
        private readonly IMapper _mapper;

        public GetPropertyByCodeQueryHandler(IRealEstatePropertyRepository repositorty, IMapper mapper)
        {
            _repositorty = repositorty;
            _mapper = mapper;
        }

        public async Task<Response<RealEstateRequest>> Handle(GetPropertyByCodeQuery request, CancellationToken cancellationToken)
        {
            var property = _mapper.Map<RealEstateRequest>(await _repositorty.GetByGuidAsync(request.Guid));
            if (property == null) throw new ApiException($"Property not found.", (int)HttpStatusCode.NoContent);
            return new Response<RealEstateRequest>(property);
        }
    }
}
