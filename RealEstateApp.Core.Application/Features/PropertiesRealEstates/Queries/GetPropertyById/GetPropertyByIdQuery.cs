using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.PropertiesRealEstates.Queries.GetPropertyById
{
    /// <summary>
    /// Parámetros para obtener una propiedad por su id
    /// </summary>
    public class GetPropertyByIdQuery : IRequest<Response<RealEstateRequest>>
    {
        [SwaggerParameter(Description = "El id de la propiedad que quiere encontrar")]
        public int Id { get; set; }
    }

    public class GetPropertyIdQueryHandler : IRequestHandler<GetPropertyByIdQuery, Response<RealEstateRequest>>
    {
        private readonly IRealEstatePropertyRepository _repository;
        private readonly IMapper _mapper;

        public GetPropertyIdQueryHandler(IRealEstatePropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<RealEstateRequest>> Handle(GetPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var property = _mapper.Map<RealEstateRequest>(await _repository.GetByIdWithIncludeAsync(request.Id, new List<string> { "TypeOfSale", "TypeProperty", "Upgrades", "Images" }));
            if (property == null) throw new ApiException($"Property not found.", (int)HttpStatusCode.NoContent);
            return new Response<RealEstateRequest>(property);
        }
    }
}
