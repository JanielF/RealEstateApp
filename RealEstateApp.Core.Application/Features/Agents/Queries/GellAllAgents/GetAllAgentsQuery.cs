using MediatR;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Wrappers;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GellAllAgents
{
    public class GetAllAgentsQuery : IRequest<Response<IEnumerable<UserDTO>>>
    {

    }

    public class GetAllAgentsQueryHandler : IRequestHandler<GetAllAgentsQuery, Response<IEnumerable<UserDTO>>>
    {
        private readonly IAccountService _accountService;

        public GetAllAgentsQueryHandler(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<Response<IEnumerable<UserDTO>>> Handle(GetAllAgentsQuery request, CancellationToken cancellationToken)
        {
            string role = UserRoles.RealEstateAgent.ToString();
            var agents = await _accountService.GetAllByRoleDTO(role);
            if (agents == null || agents.Count is 0) throw new ApiException($"There are no agents.", (int)HttpStatusCode.NoContent);
            return new Response<IEnumerable<UserDTO>>(agents);
        }
    }
}
