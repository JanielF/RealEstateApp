using Microsoft.AspNetCore.Mvc.Filters;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Core.Application.Helpers;
using RealEstateApp.WebApp.Controllers;

namespace RealEstateApp.WebApp.Middlewares
{
    public class LoginAuthorize : IAsyncActionFilter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginAuthorize(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            AuthenticationResponse userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

            if (userViewModel != null)
            {
                var controller = (UserController)context.Controller;

                if (userViewModel.Roles.Any(r => r == nameof(UserRoles.Admin)))
                {
                    context.Result = controller.RedirectToAction("adminhome", "administrator");
                }
                else if (userViewModel.Roles.Any(r => r == nameof(UserRoles.RealEstateAgent)))
                {
                    context.Result = controller.RedirectToAction("agenthome", "agent");
                }
                else if (userViewModel.Roles.Any(r => r == nameof(UserRoles.Customer)))
                {
                    context.Result = controller.RedirectToAction("customerhome", "home");
                }
                else
                {
                    context.Result = controller.RedirectToAction("index", "home");
                }
            }
            else
            {
                await next();
            }
        }
    }
}
