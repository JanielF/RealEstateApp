namespace RealEstateApp.Core.Application.Dtos.Account
{
    public class UserRegisterResponse
    {
        public string Id { get; set; }
        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}
