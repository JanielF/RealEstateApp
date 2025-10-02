using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Models
{
    public class RealEstateProperty : BaseEntity
    {
        public string Guid { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Size { get; set; }
        public string Address { get; set; }
        public int NumberOfBedrooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public string AgentId { get; set; }
        public string AgentName { get; set; }
        public int TypeOfSaleId { get; set; }
        public TypeOfSale TypeOfSale { get; set; }
        public int TypePropertyId { get; set; }
        public TypeOfProperty TypeProperty { get; set; }
        public ICollection<PropertyUpgrade> Upgrades { get; set; }
        public ICollection<PropertyImage> Images { get; set; }
        public ICollection<FavoriteProperty> FavoriteProperties { get; set;}


    }

}
