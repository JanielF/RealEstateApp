using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.Upgrades.Commands.CreateUpgrade
{
    /// <summary>
    /// Parámetros para crear una mejora
    /// </summary>
    public class CreateUpgradeCommand : IRequest<Response<int>>
    {
        /// <example> Terraza </example>
        [SwaggerParameter(Description = "El nombre de la mejora")]
        public string Name { get; set; }
        /// <example> Esta mejora indica que la propiedad incluye una terraza </example>
        [SwaggerParameter(Description = "La descripcion de la mejora")]
        public string Description { get; set; }
    }

    public class CreateUpgradeCommandHandler : IRequestHandler<CreateUpgradeCommand, Response<int>>
    {
        private readonly IUpgradeRepository _repository;
        private readonly IMapper _mapper;

        public CreateUpgradeCommandHandler(IUpgradeRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateUpgradeCommand request, CancellationToken cancellationToken)
        {
            var upgrade = _mapper.Map<Upgrade>(request);
            await _repository.CreateAsync(upgrade);
            return new Response<int>(upgrade.Id);
        }
    }
}
