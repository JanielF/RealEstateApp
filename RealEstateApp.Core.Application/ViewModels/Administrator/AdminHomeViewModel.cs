namespace RealEstateApp.Core.Application.ViewModels.Administrator
{
    public class AdminHomeViewModel
    {
        public string Username { get; set; }
        public int Properties { get; set; }
        public int ActiveAgents { get; set; }
        public int InactiveAgents { get; set; }
        public int ActiveCustomers { get; set; }
        public int InactiveCustomers { get; set; }
        public int ActiveDevelopers { get; set; }
        public int InactiveDevelopers { get; set; }
    }
}
