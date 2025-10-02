using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Infrastructure.Identity.Models;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public class DefaultAgent
    {
        public static async Task SeedAsync(UserManager<RealEstateUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            RealEstateUser defaultUser = new();
            defaultUser.UserName = "defaultAgent";
            defaultUser.Email = "defaultagent@gmail.com";
            defaultUser.FirstName = "Default";
            defaultUser.LastName = "Agent";
            defaultUser.DocumentId = "001-0153089-1";
            defaultUser.PhoneNumber = "809-1235-234";
            defaultUser.UserImagePath = "https://www.elsoldetijuana.com.mx/incoming/7h4hk6-kesikesiperrito.jpg/ALTERNATES/LANDSCAPE_768/Kesikesiperrito.jpg";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.IsActive = true;
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, nameof(UserRoles.RealEstateAgent));
                }
            }

        }
    }
}
