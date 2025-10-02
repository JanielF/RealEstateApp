using Microsoft.AspNetCore.Http;

namespace RealEstateApp.Tests.IdentityTests.HttpContexts
{

    public class TestHttpContextAccessor : IHttpContextAccessor
    {
        private HttpContext _context = new DefaultHttpContext();

        public HttpContext HttpContext
        {
            get { return _context; }
            set { _context = value; }
        }
    }
}
