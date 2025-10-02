using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Upgrades.Commands.UpdateUpgrade
{
    /// <summary>
    /// Parámetros para la actualización de una mejora
    /// </summary>
    public class UpdateUpgradeCommand : IRequest<Response<UpgradeUpdateResponse>>
    {
        [SwaggerParameter(Description = "El id de la mejora que desea actualizar")]
        public int Id { get; set; }
        /// <example> Terraza </example>
        [SwaggerParameter(Description = "El nuevo nombre de la mejora")]
        public string Name { get; set; }
        /// <example> Esta mejora indica que la propiedad incluye una terraza </example>
        [SwaggerParameter(Description = "La nueva descripcion de la mejora")]
        public string Description { get; set; }
    }

    public class UpdateUpgradeCommandHandler : IRequestHandler<UpdateUpgradeCommand, Response<UpgradeUpdateResponse>>
    {
        private readonly IUpgradeRepository _repository;
        private readonly IMapper _mapper;

        public UpdateUpgradeCommandHandler(IUpgradeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<UpgradeUpdateResponse>> Handle(UpdateUpgradeCommand command, CancellationToken cancellationToken)
        {
            var upgrade = await _repository.GetByIdAsync(command.Id);

            if (upgrade == null)
            {
                throw new ApiException($"Upgrade not found.", (int)HttpStatusCode.NotFound);
            }
            else
            {
                upgrade = _mapper.Map<Upgrade>(command);
                await _repository.UpdateAsync(upgrade, upgrade.Id);
                var upgradeVm = _mapper.Map<UpgradeUpdateResponse>(upgrade);
                return new Response<UpgradeUpdateResponse>(upgradeVm);
            }
        }
    }
}
