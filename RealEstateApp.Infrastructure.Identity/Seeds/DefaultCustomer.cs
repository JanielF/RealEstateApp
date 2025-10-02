using Microsoft.AspNetCore.Identity;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Infrastructure.Identity.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Infrastructure.Identity.Seeds
{
    public class DefaultCustomer
    {
        public static async Task SeedAsync(UserManager<RealEstateUser> userManager, RoleManager<IdentityRole> roleManager)
        {

            RealEstateUser defaultUser = new();
            defaultUser.UserName = "defaultCustomer";
            defaultUser.Email = "defaultcustomer@gmail.com";
            defaultUser.FirstName = "Default";
            defaultUser.LastName = "Customer";
            defaultUser.DocumentId = "001-7811235-2";
            defaultUser.PhoneNumber = "809-1905-634";
            defaultUser.UserImagePath = "https://images7.memedroid.com/images/UPLOADED322/634084e6e7085.jpeg";
            defaultUser.EmailConfirmed = true;
            defaultUser.PhoneNumberConfirmed = true;
            defaultUser.IsActive = true;
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word!");
                    await userManager.AddToRoleAsync(defaultUser, nameof(UserRoles.Customer));
                }
            }

        }
    }
}
