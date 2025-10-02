using RealEstateApp.Core.Application.ViewModels.Account;
using RealEstateApp.Core.Application.ViewModels.TypeOfProperty;
using RealEstateApp.Core.Application.ViewModels.TypeOfSale;
using RealEstateApp.Core.Application.ViewModels.Upgrade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstateApp.Core.Application.ViewModels.RealEstateProperty
{
    public class RealEstatePropertyDetailsViewModel
    {
        public RealEstatePropertyViewModel Property { get; set; }
        public UserViewModel Agent { get; set; }

    }
}
