using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application.Dtos.Account;
using RealEstateApp.Core.Application.Enums.Roles;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.ViewModels.Account;
using RealEstateApp.Infrastructure.Identity;
using RealEstateApp.Infrastructure.Identity.Models;
using RealEstateApp.Tests.IdentityTests.FakeServices;
using RealEstateApp.Tests.IdentityTests.HttpContexts;

namespace RealEstateApp.Tests.IdentityTests.Services
{
    public class AccountServiceTest
    {
      
        private IServiceCollection _services;
        private UserManager<RealEstateUser> _userManager;
        private RoleManager<IdentityRole> _roleManager;
        private IAccountService _accountService;
        private HttpContextAccessor _contextAccessor;
        public AccountServiceTest()
        {
            _services = new ServiceCollection();

            Setup();
            //SUT

        }



        [Fact]
        public async void AccountService_GetAll_ReturnDevelopersDto()
        {

            //arrange

            //Act

            var result = await _accountService.GetAllByRoleDTO(nameof(UserRoles.Developer));
            //Assert
            result.Should().BeOfType(typeof(List<UserDTO>));
            result.Should().OnlyHaveUniqueItems();
            result.All(x => x.Roles.Contains(nameof(UserRoles.Developer))).Should().BeTrue();


        }

        [Fact]
        public async void AccountService_GetAgentByNameAsync_ReturnListAgentViewModel()
        {

            //arrange
            string firstName = "Agent";
            string firstNameAndLastName = "Agent User";
            //Act

            var firstNameResult = await _accountService.GetAgentByNameAsync(firstName);
            var firstNameAndLastNameResult = await _accountService.GetAgentByNameAsync(firstNameAndLastName);

            //Assert
            firstNameResult.Should().BeOfType(typeof(List<UserViewModel>));
            firstNameAndLastNameResult.Should().BeOfType(typeof(List<UserViewModel>));

            firstNameResult.Should().OnlyHaveUniqueItems();
            firstNameAndLastNameResult.Should().OnlyHaveUniqueItems();

            firstNameResult.All(x => x.Roles.Contains(nameof(UserRoles.RealEstateAgent))).Should().BeTrue();
            firstNameAndLastNameResult.All(x => x.Roles.Contains(nameof(UserRoles.RealEstateAgent))).Should().BeTrue();


        }
        [Fact]
        public async void AccountService_GetAll_ReturnCustomersDto()
        {

            //arrange

            //Act

            var result = await _accountService.GetAllByRoleDTO(nameof(UserRoles.Customer));
            //Assert
            result.Should().BeOfType(typeof(List<UserDTO>));
            result.Should().OnlyHaveUniqueItems();
            result.All(x => x.Roles.Contains(nameof(UserRoles.Customer))).Should().BeTrue();


        }
        [Fact]
        public async void AccountService_GetAll_ReturnAdminsDto()
        {

            //arrange

            //Act

            var result = await _accountService.GetAllByRoleDTO(nameof(UserRoles.Admin));
            //Assert
            result.Should().BeOfType(typeof(List<UserDTO>));
            result.Should().OnlyHaveUniqueItems();
            result.All(x => x.Roles.Contains(nameof(UserRoles.Admin))).Should().BeTrue();


        }
        [Fact]
        public async void AccountService_GetAll_ReturnAgentsDto()
        {

            //arrange

            //Act

            var result = await _accountService.GetAllByRoleDTO(nameof(UserRoles.RealEstateAgent));
            //Assert
            result.Should().BeOfType(typeof(List<UserDTO>));
            result.Should().OnlyHaveUniqueItems();
            result.All(x => x.Roles.Contains(nameof(UserRoles.RealEstateAgent))).Should().BeTrue();


        }

        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnValidAdminUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123Pa$$word!",
                Username = "AdminFake",
                DocumentId = $"1111111",
                Email = $"adminfake@email.com",
                PhoneNumber = $"3455923832349966",
                FirstName = "Admin",
                LastName = "Fake",
                Role = nameof(UserRoles.Admin),

            };
            string origin = "https://origin";


            //Act
            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnInvalidAdminUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123",
                Username = "AdminUser0",
                DocumentId = $"111102220",
                Email = $"admin0@email.com",
                PhoneNumber = $"3455099660",
                FirstName = "Admin",
                LastName = "Fake",
                Role = nameof(UserRoles.Admin),



            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }
        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnValidDeveloperUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123Pa$$word!",
                Username = "DeveloperFake",
                DocumentId = $"345454245",
                Email = $"developerfake@email.com",
                PhoneNumber = $"895438945",
                FirstName = "Developer",
                LastName = "Fake",
                Role = nameof(UserRoles.Developer),

            };
            string origin = "https://origin";


            //Act
            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnInvalidDeveloperUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123",
                Username = "DeveloperUser0",
                DocumentId = $"333304440",
                Email = $"developer0@email.com",
                PhoneNumber = $"3455099660",
                FirstName = "Developer",
                LastName = "Fake",
                Role = nameof(UserRoles.Developer),



            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }



        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnValidCustomerUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123Pa$$word!",
                Username = "CustomerFake",
                DocumentId = $"393434245",
                Email = $"customerfake@email.com",
                PhoneNumber = $"3455923832349966",
                FirstName = "Customer",
                LastName = "Fake",
                Role = nameof(UserRoles.Customer),
                UserImagePath = "image.jpg"

            };
            string origin = "https://origin";


            //Act
            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnInvalidCustomerUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123",
                Username = "CustomerUser0",
                DocumentId = $"22222033330",
                Email = $"customer0@email.com",
                PhoneNumber = $"3455099660",
                FirstName = "Customer",
                LastName = "Fake",
                Role = nameof(UserRoles.Customer),
                UserImagePath = "image.jpg"

            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert

            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }



        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnValidAgentUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123Pa$$word!",
                Username = "AgentFake",
                DocumentId = $"214223232",
                Email = $"agentfake@email.com",
                PhoneNumber = $"3455923832349966",
                FirstName = "Agent",
                LastName = "Fake",
                Role = nameof(UserRoles.RealEstateAgent),
                UserImagePath = "image.jpg"

            };
            string origin = "https://origin";


            //Act
            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_RegisterUserAsync_ReturnInvalidAgentUserRegisterResponse()
        {

            //arrange
            UserRegisterRequest request = new()
            {

                Password = "123",
                Username = "AgentUser0",
                DocumentId = $"4444055550",
                Email = $"agent0@email.com",
                PhoneNumber = $"3455099660",
                FirstName = "Agent",
                LastName = "Fake",
                Role = nameof(UserRoles.RealEstateAgent),
                UserImagePath = "image.jpg"

            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.RegisterUserAsync(request, origin);
            //Assert

            result.Should().BeOfType(typeof(UserRegisterResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }


        [Fact]
        public async void AccountService_EditUserAsync_ReturnValidAdminUserEditResponse()
        {

            //arrange

            UserEditRequest request = new()
            {
                Id = "11111110",
                DocumentId = $"34124313344234",
                FirstName = "Admin12123132",
                LastName = "FakeAdmin12123",
                Email = "fakeadmin1234@email.com",
                Username = "FakeEditAdminUsername",
                Password = "123Pa$$word!123",
                Role = nameof(UserRoles.Admin)

            };
            string origin = "https://origin";




            //Act
            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_EditUserAsync_ReturnInvalidAdminUserEditResponse()
        {

            //arrange
            UserEditRequest request = new()
            {
                Id = "834589458",
                DocumentId = $"111102220",
                FirstName = "Admin12123132",
                LastName = "FakeAdmin12123",
                Email = "admin0@email.com",
                Username = "AdminUser0",
                Password = "123",
                PhoneNumber = $"3455099660",
                Role = nameof(UserRoles.Admin)
            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_EditUserAsync_ReturnValidDevoloperUserEditResponse()
        {

            //arrange

            UserEditRequest request = new()
            {
                Id = "4444440",
                DocumentId = $"1341234134",
                FirstName = "Developer12123132",
                LastName = "FakeDeveloper12123",
                Email = "fakedeveloper1234@email.com",
                Username = "FakeEditDeveloperUsername",
                Password = "123Pa$$word!123",
                Role = nameof(UserRoles.Developer)

            };
            string origin = "https://origin";




            //Act
            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_EditUserAsync_ReturnInvalidDevoloperUserEditResponse()
        {

            //arrange
            UserEditRequest request = new()
            {
                Id = "834589458",
                DocumentId = $"3333444",
                FirstName = "Admin12123132",
                LastName = "FakeDeveloper12123",
                Email = "developer0@email.com",
                Username = "DeveloperUser0",
                Password = "123",
                PhoneNumber = $"3455099660",
                Role = nameof(UserRoles.Developer)
            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_EditUserAsync_ReturnValidCustomerUserEditResponse()
        {

            //arrange

            UserEditRequest request = new()
            {
                Id = "2222220",
                FirstName = "Customer12123132",
                LastName = "FakeCustomer12123",
                PhoneNumber = "12123123123",
                UserImagePath = "fake.jpg",
                Role = nameof(UserRoles.Customer)

            };
            string origin = "https://origin";



            //Act
            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_EditUserAsync_ReturnInvalidCustomerUserEditResponse()
        {

            //arrange
            UserEditRequest request = new()
            {
                Id = "1212123123",
                FirstName = "Customer12123132",
                LastName = "FakeCustomer12123",
                PhoneNumber = "12123123123",
                UserImagePath = "fake123.jpg",
                Role = nameof(UserRoles.Customer)
            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_EditUserAsync_ReturnValidAgentUserEditResponse()
        {

            //arrange

            UserEditRequest request = new()
            {
                Id = "3333330",
                FirstName = "Agent12123132",
                LastName = "FakeAgent12123",
                PhoneNumber = "12312341233454",
                UserImagePath = "fakeagent.jpg",
                Role = nameof(UserRoles.RealEstateAgent)

            };
            string origin = "https://origin";



            //Act
            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrEmpty();



        }

        [Fact]
        public async void AccountService_EditUserAsync_ReturnInvalidAgentUserEditResponse()
        {

            //arrange
            UserEditRequest request = new()
            {
                Id = "0",
                FirstName = "Agent12123132",
                LastName = "FakeAgent12123",
                PhoneNumber = "12312341233454",
                UserImagePath = "fakeagent.jpg",
                Role = nameof(UserRoles.RealEstateAgent)
            };
            string origin = "https://origin";


            //Act

            var result = await _accountService.EditUserAsync(request, origin);
            //Assert
            result.Should().BeOfType(typeof(UserEditResponse));
            result.HasError.Should().BeTrue();
            result.Error.Should().NotBeNullOrEmpty();



        }
        #region Private methods

        private void Setup()
        {
            // Build service colection to create identity UserManager and RoleManager.           
            _services.AddLogging();
            _services.AddDistributedMemoryCache();
            _services.AddSession();

            // Add ASP.NET Core Identity database in memory.

            _services.AddIdentityInfrastructureTesting();
            _services.AddTransient<IEmailService, FakeEmalService>();
            _services.AddSingleton<IHttpContextAccessor, TestHttpContextAccessor>();

            var serviceProvider = _services.BuildServiceProvider();
            _accountService = serviceProvider.GetRequiredService<IAccountService>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            _userManager = serviceProvider.GetRequiredService<UserManager<RealEstateUser>>();




            SeedDatabaseWithRoles();
            SeedDatabaseWithUsers();

        }

        private async Task SeedDatabaseWithRoles()
        {
            if (_roleManager.Roles == null || _roleManager.Roles.Count() == 0)
            {


                string[] roles = { 
                    UserRoles.Admin.ToString(),
                    UserRoles.Developer.ToString(),
                    UserRoles.Customer.ToString(),
                    UserRoles.RealEstateAgent.ToString()
                };
                try
                {

                    foreach (string role in roles)
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
                catch (Exception ex)
                {

                }
                var rolesTest = _roleManager.Roles.ToList();
            }
        }
        private async Task SeedDatabaseWithUsers()
        {
            if (_userManager.Users == null || _userManager.Users.Count() == 0)
            {

                try
                {

                    for (int i = 0; i < 5; i++)
                    {
                        RealEstateUser user;
                        user = new RealEstateUser
                        {
                            Id = $"1111111{i}",
                            DocumentId = $"1111{i}222{i}",
                            UserName = $"AdminUser{i}",
                            Email = $"admin{i}@email.com",
                            EmailConfirmed = true,
                            IsActive = true,
                            PhoneNumber = $"3333{i}9966{i}",
                            PhoneNumberConfirmed = true,
                            FirstName = "Admin",
                            LastName = "User",




                        };
                        await _userManager.CreateAsync(user, "123Pa$$word!");
                        await _userManager.AddToRoleAsync(user, nameof(UserRoles.Admin));
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        RealEstateUser user;
                        user = new RealEstateUser
                        {
                            Id = $"222222{i}",
                            DocumentId = $"22222{i}3333{i}",
                            UserName = $"CustomerUser{i}",
                            Email = $"customer{i}@email.com",
                            EmailConfirmed = true,
                            IsActive = true,
                            PhoneNumber = $"1111{i}9966{i}",
                            PhoneNumberConfirmed = true,
                            FirstName = "Customer",
                            LastName = "User",
                            UserImagePath = $"customerimage{i}.jpg"



                        };
                        await _userManager.CreateAsync(user, "123Pa$$word!");
                        await _userManager.AddToRoleAsync(user, nameof(UserRoles.Customer));
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        RealEstateUser user;
                        user = new RealEstateUser
                        {
                            Id = $"444444{i}",
                            DocumentId = $"3333{i}444{i}",
                            UserName = $"DeveloperUser{i}",
                            Email = $"developer{i}@email.com",
                            EmailConfirmed = true,
                            IsActive = true,
                            PhoneNumber = $"2222{i}9966{i}",
                            PhoneNumberConfirmed = true,
                            FirstName = "Developer",
                            LastName = "User",




                        };
                        await _userManager.CreateAsync(user, "123Pa$$word!");
                        await _userManager.AddToRoleAsync(user, nameof(UserRoles.Developer));
                    }
                    for (int i = 0; i < 5; i++)
                    {
                        RealEstateUser user;
                        user = new RealEstateUser
                        {
                            Id = $"333333{i}",
                            DocumentId = $"4444{i}5555{i}",
                            UserName = $"AgentUser{i}",
                            Email = $"agent{i}@email.com",
                            EmailConfirmed = true,
                            IsActive = true,
                            PhoneNumber = $"4444{i}9966{i}",
                            PhoneNumberConfirmed = true,
                            FirstName = $"Agent{i}",
                            LastName = $"User{i}",
                            UserImagePath = $"agentimage{i}.jpg"




                        };
                        await _userManager.CreateAsync(user, "123Pa$$word!");
                        await _userManager.AddToRoleAsync(user, nameof(UserRoles.RealEstateAgent));
                    }

                  
                    var users = _userManager.Users.ToList();
                }
                catch (Exception ex)
                {

                }
            }
        }

        #endregion
    }
}
