using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypesOfSales.Commands.UpdateTypeOfSale
{
    /// <summary>
    /// Parámetros para la actualización de un tipo de venta
    /// </summary>
    public class UpdateTypeOfSaleCommand : IRequest<Response<TypeOfSaleUpdateResponse>>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que desea actualizar")]
        public int Id { get; set; }
        /// <example> Venta en blanco </example>
        [SwaggerParameter(Description = "El nuevo nombre del tipo de venta")]
        public string Name { get; set; }
        /// <example> Este tipo de venta, es establecido en zona urbanas </example>
        [SwaggerParameter(Description = "La nueva descripcion del tipo de venta")]
        public string Description { get; set; }
    }

    public class UpdateTypeOfSaleCommandHandler : IRequestHandler<UpdateTypeOfSaleCommand, Response<TypeOfSaleUpdateResponse>>
    {
        private readonly ITypeOfSaleRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTypeOfSaleCommandHandler(ITypeOfSaleRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TypeOfSaleUpdateResponse>> Handle(UpdateTypeOfSaleCommand command, CancellationToken cancellationToken)
        {
            var typeSale = await _repository.GetByIdAsync(command.Id);

            if (typeSale == null) 
            {
                throw new ApiException($"Type of Sale not found.", (int)HttpStatusCode.NotFound); 
            } 
            else
            {
                typeSale = _mapper.Map<TypeOfSale>(command);
                await _repository.UpdateAsync(typeSale, typeSale.Id);
                var typeSaleVm = _mapper.Map<TypeOfSaleUpdateResponse>(typeSale);
                return new Response<TypeOfSaleUpdateResponse>(typeSaleVm);
            }
        }
    }
}
