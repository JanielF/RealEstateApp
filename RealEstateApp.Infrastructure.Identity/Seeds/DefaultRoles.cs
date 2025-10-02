using Microsoft.AspNetCore.Identity;
using RealEstateApp.Infrastructure.Identity.Models;
using RealEstateApp.Core.Application.Enums.Roles;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultRoles
    {
        public static async Task SeedAsync(UserManager<RealEstateUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Customer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Developer.ToString()));
            await roleManager.CreateAsync(new IdentityRole(UserRoles.RealEstateAgent.ToString()));
        }
    }
}
