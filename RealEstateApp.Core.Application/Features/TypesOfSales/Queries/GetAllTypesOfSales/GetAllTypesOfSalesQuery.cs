using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.TypeSale;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypesOfSales.Queries.GetAllTypesOfSales
{
    public class GetAllTypesOfSalesQuery : IRequest<Response<IEnumerable<TypeSaleRequest>>>
    {

    }

    public class GetAllTypeOfSaleQueryHandler : IRequestHandler<GetAllTypesOfSalesQuery, Response<IEnumerable<TypeSaleRequest>>>
    {
        private readonly ITypeOfSaleRepository _repository;
        private readonly IMapper _mapper;

        public GetAllTypeOfSaleQueryHandler(ITypeOfSaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<IEnumerable<TypeSaleRequest>>> Handle(GetAllTypesOfSalesQuery request, CancellationToken cancellationToken)
        {
            var typeSales = _mapper.Map<List<TypeSaleRequest>>(await _repository.GetAllAsync());
            if (typeSales.Count is 0) throw new ApiException($"There are no sales types.", (int)HttpStatusCode.NoContent);
            return new Response<IEnumerable<TypeSaleRequest>>(typeSales);
        }
    }
}
