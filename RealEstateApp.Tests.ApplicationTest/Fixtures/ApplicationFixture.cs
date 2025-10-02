using AutoMapper;
using FluentAssertions.Common;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.DependencyInjection;
using RealEstateApp.Core.Application;
using RealEstateApp.Core.Domain.Models;
using RealEstateApp.Infrastructure.Persistence;
using RealEstateApp.Infrastructure.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Tests.ApplicationTest.Fixtures
{
    public class ApplicationFixture : IDisposable
    {
        public ApplicationContext Context { get; set; }
        public IMapper Mapper { get; set; }
        public ApplicationFixture()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddPersistenceLayerTest();
            services.AddApplicationLayer();
            var serviceProvider = services.BuildServiceProvider();
            Context = serviceProvider.GetRequiredService<ApplicationContext>();
            Mapper = serviceProvider.GetRequiredService<IMapper>();
            Seeds();

        }

        public void Dispose()
        {
            var a = Context.Properties.ToList();
            Context.Database.EnsureDeleted();
            var e = Context.Properties.ToList();


        }

        private void Seeds()
        {
            List<Upgrade> upgrades = new List<Upgrade>()
            {
                new Upgrade()
                {
                    Id = 1,
                    Name = "yacuzzi",
                    Description = "this is a yacuzzi"
                },
                new Upgrade()
                {
                    Id = 2,
                    Name = "terrace",
                    Description = "this is a terrace"
                },
                new Upgrade()
                {
                    Id = 3,
                    Name = "barbecue",
                    Description = "this is a barbecue"
                },
                new Upgrade()
                {
                    Id = 4,
                    Name = "Basement",
                    Description = "this is a Basement"
                }

            };

            Context.Upgrades.AddRange(upgrades);

            List<TypeOfProperty> typeOfProperties = new List<TypeOfProperty>()
            {
                new TypeOfProperty()
                {
                    Id= 1,
                    Name = "apartment",
                    Description = "this is a apartment"
                },
                new TypeOfProperty()
                {
                    Id = 2,
                    Name = "house",
                    Description = "this is a house"
                },
                new TypeOfProperty()
                {
                    Id = 3,
                    Name = "penthouse",
                    Description = "this is a penthouse"
                },
                new TypeOfProperty()
                {

                    Id = 4,
                    Name = "building",
                    Description = "this is a building"
                }

            };

            Context.TypeOfProperties.AddRange(typeOfProperties);

            List<TypeOfSale> typeOfSales = new List<TypeOfSale>()
            {
                new TypeOfSale()
                {
                    Id = 1,
                    Name = "rent",
                    Description = "this is a rent"
                },
                new TypeOfSale()
                {
                    Id = 2,
                    Name = "Standard",
                    Description = "this is a Standard"
                },
                new TypeOfSale()
                {
                    Id = 3,
                    Name = "Short",
                    Description = "this is a Short"
                },
                new TypeOfSale()
                {
                    Id = 4,
                    Name = "REO",
                    Description = "this is a REO"
                }

            };


            Context.TypeOfSales.AddRange(typeOfSales);

            List<RealEstateProperty> realEstateProperties = new List<RealEstateProperty>()
            {
                new RealEstateProperty
                {
                    Id = 1,
                    AgentId = $"1",
                    AgentName = $"Juan",
                    Description = "A property",
                    Size = 10,
                    Address = "Esquina1",
                    Guid = $"000332",
                    NumberOfBathrooms = 4,
                    NumberOfBedrooms = 4,
                    TypeOfSaleId = 1,
                    TypePropertyId = 2,
                    Price = 20000 ,
                    Upgrades = new List<PropertyUpgrade>
                    {
                        new PropertyUpgrade
                        {
                            
                            PropertyId = 1,
                            UpgradeId = 1,

                        }
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Id = 1,
                            PropertyId = 1,
                            ImagePath = $"image1.jpeg"
                        }
                    }



                },
                new RealEstateProperty
                {
                    Id = 2,
                    AgentId = $"2",
                    AgentName = $"Juan2",
                    Description = "A property",
                    Size = 40,
                    Address = "Esquina2",
                    Guid = $"040312",
                    NumberOfBathrooms = 1,
                    NumberOfBedrooms = 2,
                    TypeOfSaleId = 3,
                    TypePropertyId = 1,
                    Price = 206350 ,
                    Upgrades = new List<PropertyUpgrade>
                    {
                        new PropertyUpgrade
                        {
                            
                            PropertyId = 2,
                            UpgradeId = 3,

                        },
                        new PropertyUpgrade
                        {
                            PropertyId = 2,
                            UpgradeId = 1,

                        }
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Id = 2,
                            PropertyId = 2,
                            ImagePath = $"image2.jpeg"
                        }
                    }



                },
                new RealEstateProperty
                {
                    Id = 3,
                    AgentId = $"3",
                    AgentName = $"Juan3",
                    Description = "A property",
                    Size = 100,
                    Address = "Esquina",
                    Guid = $"122332",
                    NumberOfBathrooms = 2,
                    NumberOfBedrooms = 6,
                    TypeOfSaleId = 4,
                    TypePropertyId = 3,
                    Price = 4000221 ,
                    Upgrades = new List<PropertyUpgrade>
                    {
                        new PropertyUpgrade
                        {
                            PropertyId = 3,
                            UpgradeId = 4,

                        }
                    },
                    Images = new List<PropertyImage>
                    {
                        new PropertyImage
                        {
                            Id = 3,
                            PropertyId = 3,
                            ImagePath = $"image3.jpeg"
                        }
                    }



                }
            };
            Context.Properties.AddRange(realEstateProperties);

            List<FavoriteProperty> favoriteProperties = new List<FavoriteProperty>()
            {
                new FavoriteProperty
                {
                    Id= 1,
                    PropertyId = 1,
                    UserId = "1",
                },
                new FavoriteProperty
                {
                    Id= 2,
                    PropertyId = 1,
                    UserId = "1",
                },
                new FavoriteProperty
                {
                    Id= 3,
                    PropertyId = 2,
                    UserId = "1",
                },
                new FavoriteProperty
                {
                    Id= 4,
                    PropertyId = 3,
                    UserId = "1",
                }

            };
            Context.FavoriteProperties.AddRange(favoriteProperties);
            try
            {

                Context.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

    }
    //[CollectionDefinition("ApplicationCollection")]
    //public class ApplicationCollection : ICollectionFixture<ApplicationFixture>
    //{
    //    // This class has no code, and is never created. Its purpose is simply
    //    // to be the place to apply [CollectionDefinition] and all the
    //    // ICollectionFixture<> interfaces.
    //}

}
