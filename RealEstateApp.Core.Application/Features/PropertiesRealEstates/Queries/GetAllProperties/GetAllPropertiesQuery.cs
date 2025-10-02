using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.PropertiesRealEstates.Queries.GetAllProperties
{
    public class GetAllPropertiesQuery : IRequest<Response<IEnumerable<RealEstateRequest>>>
    {

    }

    public class GetAllPropertiesQueryHandler : IRequestHandler<GetAllPropertiesQuery, Response<IEnumerable<RealEstateRequest>>>
    {
        private readonly IRealEstatePropertyRepository _repository;
        private readonly IMapper _mapper;

        public GetAllPropertiesQueryHandler(IRealEstatePropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<RealEstateRequest>>> Handle(GetAllPropertiesQuery request, CancellationToken cancellationToken)
        {
            var properties = _mapper.Map<List<RealEstateRequest>>(await _repository.GetAllWithIncludeAsync( new List<string> { "TypeOfSale", "TypeProperty", "Upgrades", "Images" }));
            if (properties.Count is 0) throw new ApiException($"There are no properties.", (int)HttpStatusCode.NoContent);
            return new Response<IEnumerable<RealEstateRequest>>(properties);
        }
    }
}
