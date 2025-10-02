using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Entities.TypeSale;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypesOfSales.Queries.GetTypeOfSaleId
{
    /// <summary>
    /// Parámetros para obtener un tipo de venta por su id
    /// </summary>
    public class GetTypeOfSaleByIdQuery : IRequest<Response<TypeSaleRequest>>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que quiere encontrar")]
        public int Id { get; set; }
    }

    public class GetTypeSaleByIdQueryHandler : IRequestHandler<GetTypeOfSaleByIdQuery, Response<TypeSaleRequest>>
    {
        private readonly ITypeOfSaleRepository _repository;
        private readonly IMapper _mapper;

        public GetTypeSaleByIdQueryHandler(ITypeOfSaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TypeSaleRequest>> Handle(GetTypeOfSaleByIdQuery request, CancellationToken cancellationToken)
        {
            var typeSale = _mapper.Map<TypeSaleRequest>(await _repository.GetByIdAsync(request.Id));
            if (typeSale == null) throw new ApiException($"Type of sale not found.", (int)HttpStatusCode.NoContent);
            return new Response<TypeSaleRequest>(typeSale);
        }
    }
}
