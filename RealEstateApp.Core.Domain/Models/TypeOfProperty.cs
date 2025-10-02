using RealEstateApp.Core.Domain.Common;

namespace RealEstateApp.Core.Domain.Models
{
    public class TypeOfProperty : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<RealEstateProperty>? Properties { get; set; }
    }
}
