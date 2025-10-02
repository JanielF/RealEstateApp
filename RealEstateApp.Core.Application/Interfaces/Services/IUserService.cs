using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.ViewModels.Account;

namespace RealEstateApp.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserViewModel> GetByIdAsync(string id);
        Task<List<UserViewModel>> GetAllByRoleViewModel(string Role);
        Task<List<UserDTO>> GetAllByRoleDTO(string Role);

        Task<SaveUserViewModel> GetByIdSaveViewModelAsync(string id);
        Task<UserViewModel> GetByUsernameAsync(string username);
        Task<AuthenticationResponse> AuthenticateAsync(LoginViewModel request);
        Task<string> ConfirmAccountAsync(string userId, string token);
        //Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin);
        //Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request);
        Task<UserRegisterResponse> RegisterUserAsync(SaveUserViewModel request, string origin);
        Task<UserEditResponse> EditUserAsync(SaveUserViewModel request, string origin);
        Task<List<UserViewModel>> GetAll();
        Task SignOutAsync();
        Task<int> GetActiveUsers(string? role = null);
        Task<int> GetInactiveUsers(string? role = null);
        Task DeactivateUser(string id);
        Task ActivateUser(string id);
        Task<List<UserViewModel>> GetAgentByNameAsync(string nameInput);
        Task<UserDeleteResponse> DeleteUser(string id);
    }
}
