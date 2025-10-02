using FluentAssertions;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using RealEstateApp.Tests.ApplicationTest.Collections;
using RealEstateApp.Tests.ApplicationTest.Fixtures;

namespace RealEstateApp.Tests.ApplicationTest.Services
{
    [Collection(nameof(TestCollectionDefinition))]

    public class TypeOfSaleServiceTest : IClassFixture<ApplicationFixture>
    {
        private ITypeOfSaleRepository _typeOfSaleRepository;
        private ITypeOfSaleService _typeOfSaleService;
        private IRealEstatePropertyRepository _realEstatePropertyRepository;
        private readonly ApplicationFixture _fixture;
        public TypeOfSaleServiceTest(ApplicationFixture fixture)
        {
            _fixture = fixture;
            Setup();
        }
        [Fact]
        public async void TypeOfSaleService_CreateAsync_ReturnSaveTypeOfSaleViewModel()
        {
            SaveTypeOfSaleViewModel viewModel = new SaveTypeOfSaleViewModel()
            {
                Name = "Sale",
                Description = "Description",
            };


            var result = await _typeOfSaleService.CreateAsync(viewModel);
            var check = await _typeOfSaleService.GetByIdAsync(result.Id.Value);
            result.Should().BeOfType<SaveTypeOfSaleViewModel>();
            check.Should().NotBeNull();
            result.Id.Should().Be(check.Id);

        }
        [Fact]
        public async void TypeOfSaleService_DeleteAsync()
        {


            int typeOfPropertyId = 2;
            await _typeOfSaleService.DeleteAsync(typeOfPropertyId);
            var typeOfPropertyResult = await _typeOfSaleRepository.GetByIdAsync(typeOfPropertyId);
            var properties = await _realEstatePropertyRepository.GetAllAsync();
            properties = properties.Where(x => x.TypeOfSaleId == typeOfPropertyId).ToList();

            typeOfPropertyResult.Should().BeNull();
            properties.Should().BeNullOrEmpty();


        }
        [Fact]
        public async void TypeOfSaleService_UpdateAsync_ReturnSaveTypeOfSaleViewModel()
        {

            SaveTypeOfSaleViewModel viewModel = new SaveTypeOfSaleViewModel()
            {
                Id = 1,
                Name = "Sale123",
                Description = "Description",
            };


            var result = await _typeOfSaleService.UpdateAsync(viewModel, viewModel.Id.Value);
            var check = await _typeOfSaleService.GetByIdAsync(viewModel.Id.Value);
            result.Should().BeOfType<SaveTypeOfSaleViewModel>();
            check.Should().NotBeNull();
            result.Should().BeEquivalentTo(viewModel);
        }
        private void Setup()
        {
            _typeOfSaleRepository = new TypeOfSaleRepository(_fixture.Context);
            _realEstatePropertyRepository = new RealEstatePropertyRepository(_fixture.Context);
            _typeOfSaleService = new TypeOfSaleService(_typeOfSaleRepository, _fixture.Mapper);



        }

    }

}
