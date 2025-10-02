using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeProperty
{
    /// <summary>
    /// Parámetros para crear un tipo de propiedad
    /// </summary>
    public class CreateTypeOfPropertyCommand : IRequest<Response<int>>
    {
        /// <example> Residencial </example>
        [SwaggerParameter(Description = "El nuevo nombre del tipo de propiedad")]
        public string Name { get; set; }
        /// <example> Esta propiedad tiene grandes vistas en la colina Sur </example>
        [SwaggerParameter(Description = "La nueva descripcion del tipo de propiedad")]
        public string Description { get; set; }
    }

    public class CreateTypeOfPropertyCommandHandler : IRequestHandler<CreateTypeOfPropertyCommand, Response<int>>
    {
        private readonly ITypeOfPropertyRepository _repository;
        private readonly IMapper _mapper;

        public CreateTypeOfPropertyCommandHandler(ITypeOfPropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<int>> Handle(CreateTypeOfPropertyCommand request, CancellationToken cancellationToken)
        {
            var typeProperty = _mapper.Map<TypeOfProperty>(request);
            await _repository.CreateAsync(typeProperty);
            return new Response<int>(typeProperty.Id);
        }
    }
}
