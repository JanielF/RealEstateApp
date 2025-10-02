using FluentAssertions;
using RealEstateApp.Core.Application.Dtos.Entities.FavoriteProperty;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using RealEstateApp.Tests.ApplicationTest.Collections;
using RealEstateApp.Tests.ApplicationTest.Fixtures;

namespace RealEstateApp.Tests.ApplicationTest.Services
{
    [Collection(nameof(TestCollectionDefinition))]

    public class FavoritePropertyServiceTest : IClassFixture<ApplicationFixture>
    {
        private IFavoritePropertyRepository _favoriteRepository;
        private IFavoritePropertyService _favoriteService;
        private readonly ApplicationFixture _fixture;
        public FavoritePropertyServiceTest(ApplicationFixture fixture)
        {
            _fixture = fixture;
            Setup();
        }
        [Fact]
        public async void FavoritePropertyService_CreateAsync_CreateFavoriteResponse()
        {
            CreateFavoritePropertyViewModel viewModel = new CreateFavoritePropertyViewModel()
            {
                PropertyId = 4,
                UserId = "43"
            };


            var result = await _favoriteService.CreateFavoriteAsync(viewModel);
            result.Should().BeOfType<CreateFavoriteResponse>();
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrWhiteSpace();



        }
        [Fact]
        public async void FavoritePropertyService_DeleteAsync_CreateFavoritePropertyViewModel()
        {
            int favoriteId = 2;

            var result = await _favoriteService.DeleteAsync(2);
            result.Should().BeOfType<DeleteFavoriteResponse>();
            result.HasError.Should().BeFalse();
            result.Error.Should().BeNullOrWhiteSpace();



        }

        private void Setup()
        {
            _favoriteRepository = new FavoritePropertyRepository(_fixture.Context);
            _favoriteService = new FavoritePropertyService(_favoriteRepository, _fixture.Mapper);
        }
    }
}
