using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Entities.TypeProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.CreateTypeProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.DeleteTypeProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Commands.UpdateTypeProperty;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetAllTypeOfProperties;
using RealEstateApp.Core.Application.Features.TypeOfProperties.Queries.GetTypeOfPropertyById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [SwaggerTag("Mantenimientos de los tipos de propiedades")]
    public class TypeOfPropertyController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creación de un tipo de propiedad",
            Description = "Recibe los parametros necesarios para crear un nuevo tipo de propiedad"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateTypeOfPropertyCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        [SwaggerOperation(
               Summary = "Actualización de un tipo de propiedad",
               Description = "Recibe los parametros necesarios para modificar un tipo de propiedad existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypePropertyRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateTypeOfPropertyCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (id != command.Id)
            {
                return BadRequest();
            }

            return Ok(await Mediator.Send(command));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet]
        [SwaggerOperation(
           Summary = "Listado de tipo de propiedades",
           Description = "Obtiene todos los tipos de propiedades"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypePropertyRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllTypesOfPropertiesQuery()));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Tipo de propiedad por id",
            Description = "Obtiene un tipo de propiedad utilizando el id"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypePropertyRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTypeOfPropertyByIdQuery { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar un tipo de propiedad",
            Description = "Recibe los parametros necesarios para eliminar un tipo de propiedad existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTypeOfPropertyByIdCommand { Id = id });
            return NoContent();
        }
    }
}
