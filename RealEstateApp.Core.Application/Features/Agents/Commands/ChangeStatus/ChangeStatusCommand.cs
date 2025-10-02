using MediatR;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Agents.Commands.ChangeStatus
{
    /// <summary>
    /// Parámetro para la cambiar el estado de un agente
    /// </summary>
    public class ChangeStatusCommand : IRequest<Response<string>>
    {
        [SwaggerParameter(Description = "El id del agente al que deseas cambiarle el estado")]
        public string Id { get; set; }
    }

    public class ChangeStatusCommandHandler : IRequestHandler<ChangeStatusCommand, Response<string>>
    {
        private readonly IAccountService _accountService;

        public ChangeStatusCommandHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<string>> Handle(ChangeStatusCommand request, CancellationToken cancellationToken)
        {
            var user = await _accountService.GetByIdAsync(request.Id);

            if (user != null && user.Roles.Contains(UserRoles.RealEstateAgent.ToString()))
            {
                if (user.IsActive)
                {
                    await _accountService.DeactivateUser(request.Id);
                    return new Response<string>(request.Id);
                }
                else
                {
                    await _accountService.ActivateUser(request.Id);
                    return new Response<string>(request.Id);
                }
            }
            else
            {
                throw new ApiException($"Agent not found.", (int)HttpStatusCode.NotFound);
            }
        }
    }
}
