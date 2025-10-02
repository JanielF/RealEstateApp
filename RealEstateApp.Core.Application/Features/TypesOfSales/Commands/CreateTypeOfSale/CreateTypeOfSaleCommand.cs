using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.TypesOfSales.Commands.CreateTypeOfSale
{
    /// <summary>
    /// Parámetros para crear un tipo de venta
    /// </summary>
    public class CreateTypeOfSaleCommand : IRequest<Response<int>>
    {
        /// <example> Venta en blanco </example>
        [SwaggerParameter(Description = "El nombre del tipo de venta")]
        public string Name { get; set; }
        /// <example> Este tipo de venta, es establecido en zona urbanas </example>
        [SwaggerParameter(Description = "La descripcion del tipo de venta")]
        public string Description { get; set; }
    }

    public class CreateTypeOfSaleCommandHandler : IRequestHandler<CreateTypeOfSaleCommand, Response<int>>
    {
        private readonly ITypeOfSaleRepository _repository;
        private readonly IMapper _mapper;

        public CreateTypeOfSaleCommandHandler(ITypeOfSaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTypeOfSaleCommand request, CancellationToken cancellationToken)
        {
            var typeSales = _mapper.Map<TypeOfSale>(request);
            await _repository.CreateAsync(typeSales);
            return new Response<int>(typeSales.Id);
        }
    }
}
