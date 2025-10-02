using RealEstateApp.Core.Application.ViewModels.Common;
using RealEstateApp.Core.Application.ViewModels.RealEstateProperty;

namespace RealEstateApp.Core.Application.ViewModels.TypeOfProperty
{
    public class TypeOfPropertyViewModel : BaseViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int QuantityOfProperties { get; set; }
        public ICollection<RealEstatePropertyViewModel> Properties { get; set; }
    }
}
