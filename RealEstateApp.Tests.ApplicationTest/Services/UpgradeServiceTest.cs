using FluentAssertions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.Upgrade;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using RealEstateApp.Tests.ApplicationTest.Collections;
using RealEstateApp.Tests.ApplicationTest.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Tests.ApplicationTest.Services
{
    [Collection(nameof(TestCollectionDefinition))]
    public class UpgradeServiceTest : IClassFixture<ApplicationFixture>
    {
        private IUpgradeRepository _upgradeRepository;
        private IUpgradeService _upgradeService;
        private IPropertyUpgradeRepository _propertyUpgradeRepository;
        private readonly ApplicationFixture _fixture;
        public UpgradeServiceTest(ApplicationFixture fixture)
        {
            _fixture = fixture;
            Setup();
        }
        [Fact]
        public async void TypeOfSaleService_CreateAsync_ReturnSaveUpgradeViewModel()
        {
            SaveUpgradeViewModel viewModel = new SaveUpgradeViewModel()
            {
                Name = "Sale",
                Description = "Description",
            };


            var result = await _upgradeService.CreateAsync(viewModel);
            var check = await _upgradeService.GetByIdAsync(result.Id.Value);
            result.Should().BeOfType<SaveUpgradeViewModel>();
            check.Should().NotBeNull();
            result.Id.Should().Be(check.Id);

        }
        [Fact]
        public async void TypeOfSaleService_DeleteAsync()
        {


            int typeOfPropertyId = 1;
            var a = await _propertyUpgradeRepository.GetAllAsync();
            await _upgradeService.DeleteAsync(typeOfPropertyId);
            var typeOfPropertyResult = await _upgradeRepository.GetByIdAsync(typeOfPropertyId);
            var properties = await _propertyUpgradeRepository.GetAllAsync();
            properties = properties.Where(x => x.UpgradeId == typeOfPropertyId).ToList();

            typeOfPropertyResult.Should().BeNull();
            properties.Should().BeNullOrEmpty();


        }
        [Fact]
        public async void TypeOfSaleService_UpdateAsync_ReturnSaveUpgradeViewModel()
        {

            SaveUpgradeViewModel viewModel = new SaveUpgradeViewModel()
            {
                Id = 1,
                Name = "Sale123",
                Description = "Description",
            };


            var result = await _upgradeService.UpdateAsync(viewModel, viewModel.Id.Value);
            var check = await _upgradeService.GetByIdAsync(viewModel.Id.Value);
            result.Should().BeOfType<SaveUpgradeViewModel>();
            check.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }
        private void Setup()
        {
            _upgradeRepository = new UpgradeRepository(_fixture.Context);
            _propertyUpgradeRepository = new PropertyUpgradeRepository(_fixture.Context);
            _upgradeService = new UpgradeService(_upgradeRepository, _fixture.Mapper, _propertyUpgradeRepository);



        }

    }
}
