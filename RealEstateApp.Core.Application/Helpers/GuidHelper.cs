using System.Text;

namespace RealEstateApp.Core.Application.Helpers
{
    public static class GuidHelper
    {
        public static string Guid()
        {
            string guid = "";
            
            StringBuilder sb = new StringBuilder(guid);
            Random rnd = new Random();
            sb.Append(rnd.Next(100000, 999999).ToString());

            return sb.ToString();
        }

    }
}
