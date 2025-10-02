using RealEstateApp.Core.Application.Dtos.Entities.TypeProperty;
using RealEstateApp.Core.Application.Dtos.Entities.TypeSale;
using RealEstateApp.Core.Application.Dtos.Entities.Upgrade;

namespace RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty
{
    public class RealEstateRequest
    {
        public int Id { get; set; }
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
        public TypeSaleRequest TypeOfSale { get; set; }
        public int TypePropertyId { get; set; }
        public TypePropertyRequest TypeProperty { get; set; }
        public ICollection<UpgradeRequest> Upgrades { get; set; }
        public ICollection<string> Images { get; set; }
    }
}
