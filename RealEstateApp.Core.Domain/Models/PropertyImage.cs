namespace RealEstateApp.Core.Domain.Models
{
    public class PropertyImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int PropertyId { get; set; }
        public RealEstateProperty Property { get; set; }
    }
}
