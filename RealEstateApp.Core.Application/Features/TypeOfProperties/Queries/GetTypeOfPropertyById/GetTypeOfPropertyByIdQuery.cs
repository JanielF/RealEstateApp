using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.TypeProperty;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetTypeOfPropertyById
{
    /// <summary>
    /// Parámetros para obtener un tipo de propiedad por su id
    /// </summary>
    public class GetTypeOfPropertyByIdQuery : IRequest<Response<TypePropertyRequest>>
    {
        [SwaggerParameter(Description = "El id del tipo de propiedad que quiere encontrar")]
        public int Id { get; set; }
    }

    public class GetTypeOfPropertyByIdQueryHandler : IRequestHandler<GetTypeOfPropertyByIdQuery, Response<TypePropertyRequest>>
    {
        private readonly ITypeOfPropertyRepository _repository;
        private readonly IMapper _mapper;

        public GetTypeOfPropertyByIdQueryHandler(ITypeOfPropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TypePropertyRequest>> Handle(GetTypeOfPropertyByIdQuery request, CancellationToken cancellationToken)
        {
            var typeProperty = _mapper.Map<TypePropertyRequest>(await _repository.GetByIdAsync(request.Id));
            if (typeProperty == null) throw new ApiException($"Type of Property not found.", (int)HttpStatusCode.NoContent);
            return new Response<TypePropertyRequest>(typeProperty);
        }
    }
}
