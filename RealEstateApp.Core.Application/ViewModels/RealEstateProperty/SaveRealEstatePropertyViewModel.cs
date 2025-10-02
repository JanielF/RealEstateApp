using Microsoft.AspNetCore.Http;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.Upgrade;
using RealEstateApp.Core.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Core.Application.ViewModels.RealEstateProperty
{
    public class SaveRealEstatePropertyViewModel
    {
        public int? Id { get; set; }
        public string? Guid { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Size { get; set; }
        public string Address { get; set; }

        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string? AgentId { get; set; }
        public string? AgentName { get; set; }
        public int TypeOfSaleId { get; set; }
        public int TypePropertyId { get; set; }
        public List<int> Upgrades { get; set; }

        [DataType(DataType.Upload)]
        public List<IFormFile>? Images { get; set; }

        public List<string>? ImagesPath { get; set; }

        public List<UpgradeViewModel>? UpgradeList { get; set; }
        public List<TypeOfPropertyViewModel>? TypeOfPropertyList { get; set; }
        public List<TypeOfSaleViewModel>? TypeOfSaleList { get; set; }

        public string? Error {  get; set; }
        public bool HasError { get; set; }


    }
}
