using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypesOfSales.Commands.DeleteTypeOfSaleById
{
    /// <summary>
    /// Parámetros para la eliminación de un tipo de venta
    /// </summary>
    public class DeleteTypeOfSaleByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "El id del tipo de venta que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteTypeOfSaleByIdCommandHandler : IRequestHandler<DeleteTypeOfSaleByIdCommand, Response<int>>
    {
        private readonly ITypeOfSaleRepository _repository;

        public DeleteTypeOfSaleByIdCommandHandler(ITypeOfSaleRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<int>> Handle(DeleteTypeOfSaleByIdCommand request, CancellationToken cancellationToken)
        {
            var typeSale = await _repository.GetByIdAsync(request.Id);
            if (typeSale == null) throw new ApiException($"Type of sale not found.", (int)HttpStatusCode.NotFound);
            await _repository.DeleteAsync(typeSale);
            return new Response<int>(request.Id);
        }
    }
}
