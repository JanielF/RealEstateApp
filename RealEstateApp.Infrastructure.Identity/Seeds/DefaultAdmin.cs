using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Infrastructure.Identity.Models;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public static class DefaultAdmin
    {
        public static async Task SeedAsync(UserManager<RealEstateUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RealEstateUser defaultUser = new();
            defaultUser.UserName = "defaultAdmin";
            defaultUser.Email = "defaultadmin@gmail.com";
            defaultUser.FirstName = "Default";
            defaultUser.LastName = "Admin";
            defaultUser.DocumentId = "001-0250011-1";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.IsActive = true;
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, nameof(UserRoles.Admin));
                }
            }

        }
    }
}
