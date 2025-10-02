using Microsoft.AspNetCore.Identity;

namespace RealEstateApp.Infrastructure.Identity.Models
{
    public class RealEstateUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentId { get; set; }
        public bool IsActive { get; set; }
        public string? UserImagePath { get; set; }
    }
}
