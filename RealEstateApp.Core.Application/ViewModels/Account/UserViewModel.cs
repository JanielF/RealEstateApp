namespace RealEstateApp.Core.Application.ViewModels.Account
{
    public class UserViewModel
    {
        public string? Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DocumentId { get; set; }
        public string PhoneNumber { get; set; }
        public int PropertyCount { get; set; }
        public string? UserImagePath { get; set; }
        public List<string> Roles { get; set; }
        public bool IsActive { get; set; }

    }
}
