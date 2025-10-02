using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Entities.Upgrade;
using RealEstateApp.Core.Application.Features.Upgrades.Commands.CreateUpgrade;
using RealEstateApp.Core.Application.Features.Upgrades.Commands.DeleteUpgrade;
using RealEstateApp.Core.Application.Features.Upgrades.Commands.UpdateUpgrade;
using RealEstateApp.Core.Application.Features.Upgrades.Queries.GetAllUpgrades;
using RealEstateApp.Core.Application.Features.Upgrades.Queries.GetUpgradeById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [SwaggerTag("Mantenimientos de las mejoras")]
    public class UpgradeController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creación de una mejora",
            Description = "Recibe los parametros necesarios para crear una nueva mejora"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateUpgradeCommand command)
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
               Summary = "Actualizacion de una mejora",
               Description = "Recibe los parametros necesarios para modificar una mejora existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpgradeRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateUpgradeCommand command)
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
           Summary = "Listado de mejoras",
           Description = "Obtiene todos las mejoras"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpgradeRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllUpgradesQuery()));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Mejora por id",
            Description = "Obtiene una mejora utilizando el id"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UpgradeRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetUpgradeByIdQuery { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar una mejora",
            Description = "Recibe los parametros necesarios para eliminar una mejora existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteUpgradeByIdCommand { Id = id });
            return NoContent();
        }
    }
}
