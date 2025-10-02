namespace RealEstateApp.Core.Domain.Models
{
    public class PropertyUpgrade
    {
        public int PropertyId { get; set; }
        public RealEstateProperty Property { get; set; }
        public int UpgradeId { get; set; }
        public Upgrade Upgrade { get; set; }
    }
}
