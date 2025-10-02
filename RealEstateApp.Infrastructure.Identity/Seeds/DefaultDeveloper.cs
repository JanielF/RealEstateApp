using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Infrastructure.Identity.Models;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultDeveloper
    {
        public static async Task SeedAsync(UserManager<RealEstateUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RealEstateUser defaultUser = new();
            defaultUser.UserName = "defaultDeveloper";
            defaultUser.Email = "defaultdeveloper@gmail.com";
            defaultUser.FirstName = "Default";
            defaultUser.LastName = "Developer";
            defaultUser.DocumentId = "001-0257621-1";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.IsActive = true;
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, nameof(UserRoles.Developer));
                }
            }

        }
    }
}
