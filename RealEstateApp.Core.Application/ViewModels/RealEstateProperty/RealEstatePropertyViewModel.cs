using RealEstateApp.Core.Application.ViewModels.Common;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.Upgrade;

namespace RealEstateApp.Core.Application.ViewModels.RealEstateProperty
{
    public class RealEstatePropertyViewModel : BaseViewModel
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
        public TypeOfSaleViewModel TypeOfSale { get; set; }
        public int TypePropertyId { get; set; }
        public TypeOfPropertyViewModel TypeProperty { get; set; }
        public ICollection<UpgradeViewModel> Upgrades { get; set; }
        public ICollection<string> Images { get; set; }
    }
}
