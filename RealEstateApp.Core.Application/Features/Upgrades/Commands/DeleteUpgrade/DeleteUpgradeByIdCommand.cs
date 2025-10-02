using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Upgrades.Commands.DeleteUpgrade
{
    /// <summary>
    /// Parámetros para la eliminación de una mejora
    /// </summary>
    public class DeleteUpgradeByIdCommand : IRequest<Response<int>>
    {
        [SwaggerParameter(Description = "El id de la mejora que se desea eliminar")]
        public int Id { get; set; }
    }

    public class DeleteUpgradeByIdCommandHandler : IRequestHandler<DeleteUpgradeByIdCommand, Response<int>>
    {
        private readonly IUpgradeRepository _repository;

        public DeleteUpgradeByIdCommandHandler(IUpgradeRepository repository)
        {
            _repository = repository;
        }

        public async Task<Response<int>> Handle(DeleteUpgradeByIdCommand request, CancellationToken cancellationToken)
        {
            var upgrade = await _repository.GetByIdAsync(request.Id);
            if (upgrade == null) throw new ApiException($"Upgrade not found.", (int)HttpStatusCode.NotFound);
            await _repository.DeleteAsync(upgrade);
            return new Response<int>(request.Id);
        }
    }
}
