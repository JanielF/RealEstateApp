using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.TypeProperty;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetAllTypeOfProperties
{
    public class GetAllTypesOfPropertiesQuery : IRequest<Response<IEnumerable<TypePropertyRequest>>>
    {

    }

    public class GetAllTypesOfPropertiesQueryHandler : IRequestHandler<GetAllTypesOfPropertiesQuery, Response<IEnumerable<TypePropertyRequest>>>
    {
        private readonly ITypeOfPropertyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTypesOfPropertiesQueryHandler(ITypeOfPropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<TypePropertyRequest>>> Handle(GetAllTypesOfPropertiesQuery request, CancellationToken cancellationToken)
        {
            var typeProperty = _mapper.Map<List<TypePropertyRequest>>(await _repository.GetAllAsync());
            if (typeProperty.Count is 0) throw new ApiException($"There are no properties types.", (int)HttpStatusCode.NoContent);
            return new Response<IEnumerable<TypePropertyRequest>>(typeProperty);
        }
    }
}
