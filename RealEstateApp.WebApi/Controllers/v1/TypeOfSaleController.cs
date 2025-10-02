using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApp.Core.Application.Dtos.Entities.TypeSale;
using RealEstateApp.Core.Application.Features.TypesOfSales.Commands.CreateTypeOfSale;
using RealEstateApp.Core.Application.Features.TypesOfSales.Commands.DeleteTypeOfSaleById;
using RealEstateApp.Core.Application.Features.TypesOfSales.Commands.UpdateTypeOfSale;
using RealEstateApp.Core.Application.Features.TypesOfSales.Queries.GetAllTypesOfSales;
using RealEstateApp.Core.Application.Features.TypesOfSales.Queries.GetTypeOfSaleId;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace RealEstateApp.WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    [SwaggerTag("Mantenimientos de los tipos de ventas")]
    public class TypeOfSaleController : BaseApiController
    {
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [SwaggerOperation(
            Summary = "Creación de un tipo de venta",
            Description = "Recibe los parametros necesarios para crear un nuevo tipo de venta"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Post(CreateTypeOfSaleCommand command)
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
               Summary = "Actualizacion de un tipo de venta",
               Description = "Recibe los parametros necesarios para modificar un tipo de venta existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeSaleRequest))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Put(int id, UpdateTypeOfSaleCommand command)
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
           Summary = "Listado de tipo de ventas",
           Description = "Obtiene todos los tipos de venta"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeSaleRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get()
        {
            return Ok(await Mediator.Send(new GetAllTypesOfSalesQuery()));
        }

        [Authorize(Roles = "Admin, Developer")]
        [HttpGet("{id}")]
        [SwaggerOperation(
            Summary = "Tipo de venta por id",
            Description = "Obtiene un tipo de venta utilizando el id"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TypeSaleRequest))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(await Mediator.Send(new GetTypeOfSaleByIdQuery { Id = id }));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Eliminar un tipo de venta",
            Description = "Recibe los parametros necesarios para eliminar un tipo de venta existente"
        )]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new DeleteTypeOfSaleByIdCommand { Id = id });
            return NoContent();
        }
    }
}
