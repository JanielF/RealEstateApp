using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.DeleteTypeProperty
{
    /// <summary>
    /// Parámetros para la eliminación de un tipo de propiedad
    /// </summary>
    public class DeleteTypeOfPropertyByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "El id del tipo de propiedad que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteTypeOfPropertyByIdCommandHandler : IRequestHandler<DeleteTypeOfPropertyByIdCommand, Response<int>>
    {
        private readonly ITypeOfPropertyRepository _repository;

        public DeleteTypeOfPropertyByIdCommandHandler(ITypeOfPropertyRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<int>> Handle(DeleteTypeOfPropertyByIdCommand request, CancellationToken cancellationToken)
        {
            var typeProperty = await _repository.GetByIdAsync(request.Id);
            if (typeProperty == null) throw new ApiException($"Type of Property Not Found.", (int)HttpStatusCode.NotFound);
            await _repository.DeleteAsync(typeProperty);
            return new Response<int>(request.Id);
        }
    }
}
