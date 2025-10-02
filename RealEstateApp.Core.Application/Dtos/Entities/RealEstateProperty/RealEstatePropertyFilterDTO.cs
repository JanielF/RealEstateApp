namespace RealEstateApp.Core.Application.Dtos.Entities.RealEstateProperty
{
    public class RealEstatePropertyFilterDTO
    {
        public int TypeOfProperty { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int NumberOfBedrooms { get; set; }
    }
}
