using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Account;
using RealEstateApp.Core.Domain.Settings;
using RealEstateApp.Infrastructure.Identity.Models;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using System.Data;
using RealEstateApp.Core.Application.Enums.Upload;
using RealEstateApp.Core.Application.Helpers;

namespace RealEstateApp.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<RealEstateUser> _userManager;
        private readonly SignInManager<RealEstateUser> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly JWTSettings _jwtSettings;

        public AccountService(UserManager<RealEstateUser> userManager, SignInManager<RealEstateUser> signInManager, IEmailService emailService, IMapper mapper, IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _mapper = mapper;
            _jwtSettings = jwtSettings.Value;
        }

        #region Activate and Deactivate
        public async Task ActivateUser(string id)
        {
            RealEstateUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = true;
                await _userManager.UpdateAsync(user);
            }
        }

        public async Task DeactivateUser(string id)
        {
            RealEstateUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsActive = false;
                await _userManager.UpdateAsync(user);
            }
        }
        #endregion

        #region Login

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();
            RealEstateUser user;
            user = request.Input.Contains("@") ? await _userManager.FindByEmailAsync(request.Input) : await _userManager.FindByNameAsync(request.Input);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Input}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"Invalid credentials for {request.Input}";
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.HasError = true;
                response.Error = $"Account no confirmed for {request.Input}";
                return response;
            }
            if (!user.IsActive)
            {
                response.HasError = true;
                response.Error = $"Account no actived for {request.Input}";
                return response;
            }

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

            response.Id = user.Id;
            response.Email = user.Email;
            response.Username = user.UserName;
            response.UserImagePath = user.UserImagePath;

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();
            response.IsVerified = user.EmailConfirmed;
            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            var refreshToken = GenerateRefreshToken();
            response.RefreshToken = refreshToken.Token;

            return response;
        }

        #endregion

        #region Logout

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        #region Registers

        #region General

        public async Task<UserRegisterResponse> RegisterUserAsync(UserRegisterRequest request, string origin)
        {
            UserRegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"username '{request.Username}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }
            var userWithSameDocumentId = await _userManager.Users.FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId);
            if (userWithSameDocumentId != null)
            {
                response.HasError = true;
                response.Error = $"The document id '{request.DocumentId}' is already registered.";
                return response;
            }

            if (!Enum.IsDefined(typeof(UserRoles), request.Role))
            {

                response.HasError = true;
                response.Error = $"The role {request.Role} do not exist";
                return response;
            }


            var user = new RealEstateUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Username,
                DocumentId = request.DocumentId,
                PhoneNumber = request.PhoneNumber,
                PhoneNumberConfirmed = true,
                IsActive = request.Role != nameof(UserRoles.RealEstateAgent),
                EmailConfirmed = request.Role != nameof(UserRoles.Customer)


            };

            var result = await _userManager.CreateAsync(user, request.Password);
            var createdUser = await _userManager.FindByNameAsync(user.UserName);
            if (result.Succeeded && createdUser != null)
            {
                if (request.UserImage != null)
                {
                    createdUser.UserImagePath = UploadHelper.UploadFile(request.UserImage, createdUser.Id, nameof(UploadEntities.User));
                    await _userManager.UpdateAsync(createdUser);

                }
                await _userManager.AddToRoleAsync(user, request.Role);

                if (request.Role == nameof(UserRoles.Customer))
                {

                    var verificationUri = await SendVerificationEmailUri(user, origin);

                    await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
                    {
                        To = user.Email,
                        Body = $"Please confirm your account visiting this URL {verificationUri}",
                        Subject = "Confirm registration"
                    });
                }
            }
            else
            {
                response.HasError = true;
                response.Error = $"An error occurred trying to register the user.";
                return response;
            }
            response.Id = createdUser.Id;
            return response;
        }

        #endregion

        #endregion

        #region Edit User

        public async Task<UserEditResponse> EditUserAsync(UserEditRequest request, string origin)
        {
            UserEditResponse response = new()
            {
                HasError = false
            };

            if (!Enum.IsDefined(typeof(UserRoles), request.Role))
            {

                response.HasError = true;
                response.Error = $"The role {request.Role} do not exist";
                return response;
            }

            if (request.Role == nameof(UserRoles.Admin) || request.Role == nameof(UserRoles.Developer))
            {
                response = await EditInternalUsersValidations(request);
                if (response.HasError)
                    return response;
            }


            RealEstateUser user = await _userManager.FindByIdAsync(request.Id);
            if (user == null)
            {
                response.HasError = true;
                response.Error = $"We couldn't be able to find your user.";
                return response;

            }
            user.FirstName = request.FirstName ?? user.FirstName;
            user.LastName = request.LastName ?? user.LastName;
            user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
            user.DocumentId = request.DocumentId ?? user.DocumentId;
            user.UserName = request.Username ?? user.UserName;
            user.Email = request.Email ?? user.Email;
            if (request.UserImage != null)
            {
                user.UserImagePath = UploadHelper.UploadFile(request.UserImage, user.Id, nameof(UploadEntities.User), true, user.UserImagePath);

            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {

                response.HasError = true;
                response.Error = $"An error occurred trying to edit the user.";
                return response;
            }

            if (request.Password != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _userManager.ResetPasswordAsync(user, token, request.Password);
            }

            return response;
        }

        #endregion

        #region Confirm User

        public async Task<string> ConfirmAccountAsync(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return $"No accounts registered with this user";
            }

            token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return $"Account confirmed for {user.Email}. You can now use the app";
            }
            else
            {
                return $"An error occurred while confirming {user.Email}.";
            }
        }

        #endregion

        #region Gets

        #region GetsDTO

        public async Task<UserDTO> GetByIdAsyncDTO(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userVm = _mapper.Map<UserDTO>(user);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userVm.Roles = roles.ToList();
            }
            return userVm;
        }

        public async Task<UserDTO> GetByIdRoleAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userVm = _mapper.Map<UserDTO>(user);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userVm.Roles = roles.ToList();
            }
            return userVm;
        }

        public async Task<List<UserDTO>> GetAllByRoleDTO(string role)
        {
            var users = await _userManager.Users.ToListAsync();
            var usersDtos = _mapper.Map<List<UserDTO>>(users);
            if (users != null && users.Count > 0)
            {
                foreach (UserDTO user in usersDtos)
                {
                    var roles = await _userManager.GetRolesAsync(users.FirstOrDefault(y => y.Id == user.Id));
                    user.Roles = roles.ToList();
                }
            }
            usersDtos = usersDtos.Where(x => x.Roles.Contains(role)).ToList();
            return usersDtos;
        }

        #endregion

        #region GetsViewModels

        public async Task<SaveUserViewModel> GetByIdSaveViewModelAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userVm = _mapper.Map<SaveUserViewModel>(user);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userVm.Role = roles.First();
            }
            return userVm;
        }

        public async Task<UserViewModel> GetByUsernameAsync(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            var userVm = _mapper.Map<UserViewModel>(user);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userVm.Roles = roles.ToList();
            }
            return userVm;
        }

        public async Task<List<UserViewModel>> GetAll()
        {
            var users = await _userManager.Users.ToListAsync();
            var usersVm = _mapper.Map<List<UserViewModel>>(users);
            if (users != null && users.Count > 0)
            {
                foreach (UserViewModel user in usersVm)
                {
                    var roles = await _userManager.GetRolesAsync(users.FirstOrDefault(y => y.Id == user.Id));
                    user.Roles = roles.ToList();
                }
            }
            return usersVm;
        }

        public async Task<UserViewModel> GetByIdAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var userVm = _mapper.Map<UserViewModel>(user);
            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userVm.Roles = roles.ToList();
            }
            return userVm;
        }

        public async Task<List<UserViewModel>> GetAgentByNameAsync(string nameInput)
        {
            var names = nameInput.Split(' ');
            List<UserViewModel> users = new();
            if (names.Length == 1)
            {
                var result = await _userManager.Users.Where(x => x.FirstName.Contains(names[0]) || x.LastName.Contains(names[0])).ToListAsync();
                result = result.Where(x => _userManager.GetRolesAsync(x).Result.Contains(nameof(UserRoles.RealEstateAgent))).ToList();
                users = _mapper.Map<List<UserViewModel>>(result);
                foreach (UserViewModel user in users)
                {
                    var roles = await _userManager.GetRolesAsync(result.FirstOrDefault(y => y.Id == user.Id));
                    user.Roles = roles.ToList();
                }
            }
            else if (names.Length == 2)
            {
                var result = await _userManager.Users.Where(x => (x.FirstName.Contains(names[0]) && x.LastName.Contains(names[1])) || (x.FirstName.Contains(names[0]) && x.LastName.Contains(names[1]))).ToListAsync();
                result = result.Where(x => _userManager.GetRolesAsync(x).Result.Contains(nameof(UserRoles.RealEstateAgent))).ToList();
                users = _mapper.Map<List<UserViewModel>>(result);
                foreach (UserViewModel user in users)
                {
                    var roles = await _userManager.GetRolesAsync(result.FirstOrDefault(y => y.Id == user.Id));
                    user.Roles = roles.ToList();
                }
            }

            return users;
        }

        public async Task<List<UserViewModel>> GetAllByRoleViewModel(string role)
        {
            var users = await _userManager.Users.ToListAsync();
            var usersVm = _mapper.Map<List<UserViewModel>>(users);
            if (users != null && users.Count > 0)
            {
                foreach (UserViewModel user in usersVm)
                {
                    var roles = await _userManager.GetRolesAsync(users.FirstOrDefault(y => y.Id == user.Id));
                    user.Roles = roles.ToList();
                }
            }
            usersVm = usersVm.Where(x => x.Roles.Contains(role)).ToList();
            return usersVm;
        }

        #endregion

        public async Task<int> GetActiveUsers(string? role = null)
        {
            var users = await _userManager.Users.Where(x => x.IsActive).ToListAsync();
            users = users.Where(x => role != null ? _userManager.GetRolesAsync(x).Result.Contains(role) : true).ToList();
            return users.Count();
        }

        public async Task<int> GetInactiveUsers(string? role = null)
        {
            var users = await _userManager.Users.Where(x => !x.IsActive).ToListAsync();
            users = users.Where(x => role != null ? _userManager.GetRolesAsync(x).Result.Contains(role) : true).ToList();
            return users.Count();
        }

        

        #endregion

        #region Password

        public async Task<ForgotPasswordResponse> ForgotPasswordAsync(ForgotPasswordRequest request, string origin)
        {
            ForgotPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Username}";
                return response;
            }

            var verificationUri = await SendForgotPasswordUri(user, origin);

            await _emailService.SendAsync(new Core.Application.Dtos.Email.EmailRequest()
            {
                To = user.Email,
                Body = $"Please reset your account visiting this URL {verificationUri}",
                Subject = "reset password"
            });


            return response;
        }

        public async Task<ResetPasswordResponse> ResetPasswordAsync(ResetPasswordRequest request)
        {
            ResetPasswordResponse response = new()
            {
                HasError = false
            };

            var user = await _userManager.FindByNameAsync(request.Username);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Accounts registered with {request.Username}";
                return response;
            }

            request.Token = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = $"An error occurred while reset password";
                return response;
            }

            return response;
        }

        #endregion

        #region Private methods

        private async Task<JwtSecurityToken> GenerateJWToken(RealEstateUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            foreach (var role in roles)
            {
                roleClaims.Add(new Claim("roles", role));
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmectricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredetials = new SigningCredentials(symmectricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredetials);

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = RandomTokenString(),
                Expires = DateTime.UtcNow.AddDays(7),
                Created = DateTime.UtcNow
            };
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var ramdomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(ramdomBytes);

            return BitConverter.ToString(ramdomBytes).Replace("-", "");
        }

        private async Task<UserEditResponse> EditInternalUsersValidations(UserEditRequest request)
        {
            UserEditResponse response = new()
            {
                HasError = false
            };
            var userWithSameUserName = await _userManager.FindByNameAsync(request.Username);
            if (userWithSameUserName != null && userWithSameUserName.Id != request.Id)
            {
                response.HasError = true;
                response.Error = $"username '{request.Username}' is already taken.";
                return response;
            }

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != request.Id)
            {
                response.HasError = true;
                response.Error = $"Email '{request.Email}' is already registered.";
                return response;
            }
            var userWithSameDocumentId = await _userManager.Users.FirstOrDefaultAsync(x => x.DocumentId == request.DocumentId);
            if (userWithSameDocumentId != null && userWithSameDocumentId.Id != request.Id)
            {
                response.HasError = true;
                response.Error = $"The document id '{request.DocumentId}' is already registered.";
                return response;
            }

            return response;
        }

        private async Task<string> SendVerificationEmailUri(RealEstateUser user, string origin)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ConfirmEmail";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "userId", user.Id.ToString());
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "token", code);
            return verificationUri;
        }

        private async Task<string> SendForgotPasswordUri(RealEstateUser user, string origin)
        {
            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var route = "User/ResetPassword";
            var Uri = new Uri(string.Concat($"{origin}/", route));
            var verificationUri = QueryHelpers.AddQueryString(Uri.ToString(), "token", code);
            verificationUri = QueryHelpers.AddQueryString(verificationUri, "username", user.UserName);
            return verificationUri;
        }

        #endregion

        #region Delete User

        public async Task<UserDeleteResponse> DeleteAsync(string id)
        {
            UserDeleteResponse response = new();
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "We couldn't delete the user";
                return response;
            }
            return response;
        }

        #endregion

    }
}
