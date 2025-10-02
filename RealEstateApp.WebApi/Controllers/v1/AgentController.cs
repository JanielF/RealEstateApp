using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatus;
using RealEstateApp.Core.Application.Features.Agents.Queries.GellAllAgents;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperty;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [SwaggerTag("Mantenimientos de agentes")]
    public class AgentController : BaseApiController
    {
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista de agentes",
            Description = "Obtiene todos los agentes"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllAgentsQuery()));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Agente por id",
            Description = "Obtiene un agente utilizando el id"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string id)
        {
            return Ok(await Mediator.Send(new GetAgentByIdQuery { Id = id }));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("GetProperties/{id}")]
        [SwaggerOperation(
            Summary = "Propiedades por el agente",
            Description = "Obtienes las propiedades mediante el id del agente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RealEstateRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAgentProperty(string id)
        {
            return Ok(await Mediator.Send(new GetAgentPropertyQuery { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpPatch("{id}")]
        [SwaggerOperation(
            Summary = "Cambiar el estado de un agente",
            Description = "Actualiza el estado de un usuario entre activo o inactivo"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDTO))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangeStatus(string id)
        {
            return Ok(await Mediator.Send(new ChangeStatusCommand { Id = id }));
        }
    }
}
