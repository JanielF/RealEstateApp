using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Wrappers;
using RealEstateApp.Core.Domain.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeProperty
{
    /// <summary>
    /// Parámetros para la actualización de un tipo de propiedad
    /// </summary>
    public class UpdateTypeOfPropertyCommand : IRequest<Response<TypeOfPropertyUpdateResponse>>
    {
        [SwaggerParameter(Description = "El id del tipo de propiedad que desea actualizar")]
        public int Id { get; set; }
        /// <example> Residencial </example>
        [SwaggerParameter(Description = "El nuevo nombre del tipo de propiedad")]
        public string Name { get; set; }
        /// <example> Esta propiedad tiene grandes vistas en la colina Sur </example>
        [SwaggerParameter(Description = "La nueva descripcion del tipo de propiedad")]
        public string Description { get; set; }
    }

    public class UpdateTypeOfPropertyCommandHandler : IRequestHandler<UpdateTypeOfPropertyCommand, Response<TypeOfPropertyUpdateResponse>>
    {
        private readonly ITypeOfPropertyRepository _repository;
        private readonly IMapper _mapper;

        public UpdateTypeOfPropertyCommandHandler(ITypeOfPropertyRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Response<TypeOfPropertyUpdateResponse>> Handle(UpdateTypeOfPropertyCommand command, CancellationToken cancellationToken)
        {
            var typeProperty = await _repository.GetByIdAsync(command.Id);

            if (typeProperty == null)
            {
                throw new ApiException($"Type of Property not found.", (int)HttpStatusCode.NotFound);
            }
            else
            {
                typeProperty = _mapper.Map<TypeOfProperty>(command);
                await _repository.UpdateAsync(typeProperty, typeProperty.Id);
                var typePropertyVm = _mapper.Map<TypeOfPropertyUpdateResponse>(typeProperty);
                return new Response<TypeOfPropertyUpdateResponse>(typePropertyVm);
            }
        }
    }
}
