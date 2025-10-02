using AutoMapper;
using MediatR;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Core.Application.Exceptions;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Wrappers;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;

namespace RealEstateApp.Core.Application.Features.Agents.Queries.GetAgentById
{
    /// <summary>
    /// Parámetros para obtener un agente por su id
    /// </summary>
    public class GetAgentByIdQuery : IRequest<Response<UserDTO>>
    {
        [SwaggerParameter(Description = "El id del agente que quiere encontrar")]
        public string Id { get; set; }
    }

    public class GetAgentByIdQueryHandler : IRequestHandler<GetAgentByIdQuery, Response<UserDTO>>
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;

        public GetAgentByIdQueryHandler(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        public async Task<Response<UserDTO>> Handle(GetAgentByIdQuery request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<UserDTO>(await _accountService.GetByIdAsyncDTO(request.Id));
            if (user == null && user.Roles.Contains(UserRoles.RealEstateAgent.ToString())) throw new ApiException($"Agent not found.", (int)HttpStatusCode.NoContent);
            return new Response<UserDTO>(user);
        }
    }
}
