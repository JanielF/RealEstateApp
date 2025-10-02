using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Models
{
    public class Upgrade : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<PropertyUpgrade>? Properties { get; set; }

    }
}
