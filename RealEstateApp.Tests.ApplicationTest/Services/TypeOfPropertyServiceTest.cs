using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using RealEstateApp.Tests.ApplicationTest.Collections;
using RealEstateApp.Tests.ApplicationTest.Fixtures;

namespace RealEstateApp.Tests.ApplicationTest.Services
{
    [Collection(nameof(TestCollectionDefinition))]
    public class TypeOfPropertyServiceTest : IClassFixture<ApplicationFixture>
    {
        private ITypeOfPropertyRepository _typeOfPropertyRepository;
        private ITypeOfPropertyService _typeOfPropertyService;
        private IRealEstatePropertyRepository _realEstatePropertyRepository;
        private readonly ApplicationFixture _fixture;
        public TypeOfPropertyServiceTest(ApplicationFixture fixture)
        {
            _fixture = fixture;
            Setup();
        }
        [Fact]
        public async void TypeOfPropertyService_CreateAsync_ReturnSaveTypeOfPropertyViewModel()
        {
            SaveTypeOfPropertyViewModel viewModel = new SaveTypeOfPropertyViewModel()
            {
                Name = "Apartment",
                Description = "Description",
            };


            var result = await _typeOfPropertyService.CreateAsync(viewModel);
            var check = await _typeOfPropertyService.GetByIdAsync(result.Id.Value);
            result.Should().BeOfType<SaveTypeOfPropertyViewModel>();
            check.Should().NotBeNull();
            result.Id.Should().Be(check.Id);

        }
        [Fact]
        public async void TypeOfPropertyService_DeleteAsync()
        {


            int typeOfPropertyId = 2;
            await _typeOfPropertyService.DeleteAsync(typeOfPropertyId);
            var typeOfPropertyResult = await _typeOfPropertyRepository.GetByIdAsync(typeOfPropertyId);
            var properties = await _realEstatePropertyRepository.GetAllAsync();
            properties = properties.Where(x => x.TypePropertyId == typeOfPropertyId).ToList();

            typeOfPropertyResult.Should().BeNull();
            properties.Should().BeNullOrEmpty();


        }
        [Fact]
        public async void TypeOfPropertyService_UpdateAsync_ReturnSaveTypeOfPropertyViewModel()
        {
            SaveTypeOfPropertyViewModel viewModel = new SaveTypeOfPropertyViewModel()
            {
                Id = 1,
                Name = "Apartment",
                Description = "Description",
            };


            var result = await _typeOfPropertyService.UpdateAsync(viewModel, viewModel.Id.Value);
            var check = await _typeOfPropertyService.GetByIdAsync(viewModel.Id.Value);
            result.Should().BeOfType<SaveTypeOfPropertyViewModel>();
            check.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);


        }
        private void Setup()
        {
            _typeOfPropertyRepository = new TypeOfPropertyRepository(_fixture.Context);
            _realEstatePropertyRepository = new RealEstatePropertyRepository(_fixture.Context);
            _typeOfPropertyService = new TypeOfPropertyService(_typeOfPropertyRepository, _fixture.Mapper);



        }

    }

}
