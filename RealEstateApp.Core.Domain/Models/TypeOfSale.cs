using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Models
{
    public class TypeOfSale : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RealEstateProperty>? Properties { get; set; }

    }
}
