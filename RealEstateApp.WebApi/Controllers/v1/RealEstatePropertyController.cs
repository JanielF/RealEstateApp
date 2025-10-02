using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty;
using RealEstateApp.Core.Application.Features.Agents.Queries.GellAllAgents;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById;
using RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentProperty;
using RealEstateApp.Core.Application.Features.PropertiesRealEstates.Queries.GetAllProperties;
using RealEstateApp.Core.Application.Features.PropertiesRealEstates.Queries.GetPropertyByCode;
using RealEstateApp.Core.Application.Features.PropertiesRealEstates.Queries.GetPropertyById;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [SwaggerTag("Mantenimientos de propiedades")]
    public class RealEstatePropertyController : BaseApiController
    {
        [Authorize(Roles = "Admin, Developer")]
        [HttpGet]
        [SwaggerOperation(
            Summary = "Lista de propiedades",
            Description = "Obtiene todos las propiedades"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RealEstateRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllPropertiesQuery()));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Propiedades por id",
            Description = "Obtiene una propiedad utilizando el id"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RealEstateRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetPropertyByIdQuery { Id = id }));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("ByCode")]
        [SwaggerOperation(
            Summary = "Propiedades por el codigo",
            Description = "Obtienes las propiedades mediante el codigo de la propiedad"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(RealEstateRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(string guid)
        {
            return Ok(await Mediator.Send(new GetPropertyByCodeQuery { Guid = guid }));
        }
    }
}
