namespace RealEstateApp.Core.Domain.Models
{
    public class FavoriteProperty
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public RealEstateProperty Property { get; set; }
        public string UserId { get; set; }
    }
}
