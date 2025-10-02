using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application;
using RealEstateApp.Core.Application.Interfaces.Repositories;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Application.Services;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence;
using RealEstateApp.Infrastructure.Persistence.Repositories;
using RealEstateApp.Tests.ApplicationTest.Collections;
using RealEstateApp.Tests.ApplicationTest.Fixtures;

namespace RealEstateApp.Tests.ApplicationTest.Services
{
    [Collection(nameof(TestCollectionDefinition))]
    public class RealEstatePropertyServiceTest : IClassFixture<ApplicationFixture>
    {
        private IRealEstatePropertyService _realEstatePropertyService;
        private IRealEstatePropertyRepository _realEstatePropertyRepository;
        private IPropertyImageRepository _propertyImageRepository;
        private IPropertyUpgradeRepository _propertyUpgradeRepository;
        private string path;
        private readonly ApplicationFixture _fixture;
        public RealEstatePropertyServiceTest(ApplicationFixture fixture)
        {
            _fixture = fixture;
            Setup();
        }
        [Fact]
        public async void RealEstateProperty_CreateAsync_ReturnSaveRealEstatePropertyViewModel()
        {

            SaveRealEstatePropertyViewModel vm = new SaveRealEstatePropertyViewModel()
            {
                Id = 444,
                AgentId = "1",
                NumberOfBathrooms = 1,
                NumberOfBedrooms = 2,
                Size = 3,
                Address = "La independencia",
                AgentName = "juan",
                Description = "Description",
                Price = 2000000,
                TypeOfSaleId = 2,
                TypePropertyId = 2,
                Images = new List<IFormFile>
                {
                      GetFormFile("../../../../RealEstateApp.Tests.ApplicationTest/TestImages/Screenshot 2023-03-17 065624.png"),
                      GetFormFile("../../../../RealEstateApp.Tests.ApplicationTest/TestImages/Screenshot 2024-02-08 194134.png"),
                      GetFormFile("../../../../RealEstateApp.Tests.ApplicationTest/TestImages/Screenshot_20221226_020447.png"),
                      GetFormFile("../../../../RealEstateApp.Tests.ApplicationTest/TestImages/Screenshot_20230225_054808.png"),

                      /*
                       * 
                       * C:\Users\oscar\Desktop\ITLA\c5\P3\Proyectos\RealEstateApp\RealEstateApp.Tests.ApplicationTest\bin\Debug\net7.0\TestImages\Screenshot 2023-03-17 065624.png'.'
                       */


                },
                Upgrades = new List<int>
                {
                    2,
                    3
                },


            };
            var result = await _realEstatePropertyService.CreateAsync(vm);
            var check = await _realEstatePropertyRepository.GetByIdAsync(result.Id.Value);
            result.Should().BeOfType<SaveRealEstatePropertyViewModel>();
            result.ImagesPath.Should().HaveCount(check.Images.Count());
            result.Upgrades.Should().HaveCount(check.Upgrades.Count());
            check.Should().NotBeNull();
            result.Id.Should().Be(check.Id);
        }
        [Fact]
        public async void RealEstatePropertyService_GetAllByFilter_returnListRealEstatePropertyViewModel()
        {
            RealEstatePropertyFilterViewModel filter = new RealEstatePropertyFilterViewModel()
            {
                MaxPrice = 406350,
                MinPrice = 100,
                NumberOfBathrooms = 1,
                NumberOfBedrooms = 2,
                TypeOfProperty = 1,
            };

            var result = await _realEstatePropertyService.GetAllByFilter(filter);
            result.Should().NotBeNullOrEmpty();

        }
        [Fact]
        public async void RealEstateProperty_DeleteAsync()
        {


            var all = await _realEstatePropertyRepository.GetAllAsync();

            int propertyId = 1;
            await _realEstatePropertyService.DeleteAsync(propertyId);
            var propertyResult = await _realEstatePropertyRepository.GetByIdAsync(propertyId);
            var imagesResult = await _propertyImageRepository.GetAllAsync();
            imagesResult = imagesResult.Where(x => x.PropertyId == propertyId).ToList();
            var upgradesResult = await _propertyUpgradeRepository.GetAllAsync();
            upgradesResult = upgradesResult.Where(x => x.PropertyId == propertyId).ToList();

            propertyResult.Should().BeNull();
            imagesResult.Should().BeNullOrEmpty();
            upgradesResult.Should().BeNullOrEmpty();

        }
        [Fact]
        public async void RealEstateProperty_UpdateAsync_ReturnSaveRealEstatePropertyViewModel()
        {
            var all = await _realEstatePropertyRepository.GetAllAsync();

            SaveRealEstatePropertyViewModel vm = new SaveRealEstatePropertyViewModel()
            {
                Id = 1,
                AgentId = "34",
                NumberOfBathrooms = 55,
                NumberOfBedrooms = 55,
                Address = "ya no es esquina",
                Size = 3,
                AgentName = "juan34",
                Description = "Description222222",
                Price = 1,
                TypeOfSaleId = 2,
                TypePropertyId = 2,
                Images = new List<IFormFile>
                {
                      GetFormFile("../../../../RealEstateApp.Tests.ApplicationTest/TestImages/Screenshot 2024-02-08 194134.png"),
                      GetFormFile("../../../../RealEstateApp.Tests.ApplicationTest/TestImages/Screenshot_20221226_020447.png"),
                      GetFormFile("../../../../RealEstateApp.Tests.ApplicationTest/TestImages/Screenshot_20230225_054808.png"),

                      /*
                       * 
                       * C:\Users\oscar\Desktop\ITLA\c5\P3\Proyectos\RealEstateApp\RealEstateApp.Tests.ApplicationTest\bin\Debug\net7.0\TestImages\Screenshot 2023-03-17 065624.png'.'
                       */


                },
                Upgrades = new List<int>
                {
                    1,
                    2
                },


            };
            var result = await _realEstatePropertyService.UpdateAsync(vm, vm.Id.Value);
            var check = await _realEstatePropertyRepository.GetByIdAsync(result.Id.Value);
            result.Should().BeOfType<SaveRealEstatePropertyViewModel>();
            result.ImagesPath.Should().HaveCount(check.Images.Count());
            result.Upgrades.Should().HaveCount(check.Upgrades.Count());
            result.Id.Should().Be(check.Id);
        }
        private async void Setup()
        {
            path = "./wwwroot";

            if (Directory.Exists(path))
            {
                DirectoryInfo directory = new(path);

                foreach (FileInfo file in directory.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo folder in directory.GetDirectories())
                {
                    folder.Delete(true);
                }

                Directory.Delete(path);
            }
            _propertyImageRepository = new PropertyImageRepository(_fixture.Context);
            _propertyUpgradeRepository = new PropertyUpgradeRepository(_fixture.Context);
            _realEstatePropertyRepository = new RealEstatePropertyRepository(_fixture.Context);

            _realEstatePropertyService = new RealEstatePropertyService(_realEstatePropertyRepository
                ,_fixture.Mapper
                , _propertyImageRepository
                , _propertyUpgradeRepository
                , new FavoritePropertyRepository(_fixture.Context));

            //await PropertySeeds();




        }
        private IFormFile GetFormFile(string filePath)
        {
            byte[] fileBytes = File.ReadAllBytes(filePath);

            MemoryStream memoryStream = new MemoryStream(fileBytes);

            IFormFile formFile = new FormFile(memoryStream, 0, memoryStream.Length, null, Path.GetFileName(filePath));

            return formFile;
        }

        //private async Task PropertySeeds()
        //{
        //    var properties = await _realEstatePropertyRepository.GetAllAsync();
        //    if (properties == null || properties.Count() == 0)
        //    {

        //        for (int i = 1; i <= 10; i++)
        //        {
        //            var property = new RealEstateProperty
        //            {
        //                Id = i,
        //                AgentId = $"{i}",
        //                AgentName = $"Juan{i}",
        //                Description = "A property",
        //                Size = 10 + i,
        //                Guid = $"00033{i}",
        //                NumberOfBathrooms = i,
        //                NumberOfBedrooms = i,
        //                TypeOfSale = new TypeOfSale
        //                {
        //                    Id = i,
        //                    Name = "type of sale",
        //                    Description = "a type of sale"
        //                },
        //                TypeOfSaleId = i,
        //                TypeProperty = new TypeOfProperty
        //                {
        //                    Id = i,
        //                    Name = "type of property",
        //                    Description = "a type of property"
        //                },
        //                TypePropertyId = i,
        //                Price = 20000 * i,
        //                Upgrades = new List<PropertyUpgrade>
        //            {
        //                new PropertyUpgrade
        //                {
        //                    PropertyId = i,
        //                    UpgradeId = i,
        //                    Upgrade = new Upgrade
        //                    {
        //                        Id = i,
        //                        Description = "upgrade description",
        //                        Name = "upgrade"
        //                    }
        //                }
        //            },
        //                Images = new List<PropertyImage>
        //            {
        //                new PropertyImage
        //                {
        //                    Id = i,
        //                    PropertyId = i,
        //                    ImagePath = $"image{i}.jpeg"
        //                }
        //            },



        //            };

        //            await _realEstatePropertyRepository.CreateAsync(property);

        //        }

        //    }

        //}

    }
}
