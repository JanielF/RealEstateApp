using Microsoft.Extensions.Options;
using RealEstateApp.Core.Application.Dtos.Email;
using RealEstateApp.Core.Application.Interfaces.Services;
using RealEstateApp.Core.Domain.Settings;

namespace RealEstateApp.Tests.IdentityTests.FakeServices
{
    public class FakeEmalService : IEmailService
    {
        public MailSettings MailSettings { get; }

        public async Task SendAsync(EmailRequest request)
        {
        }
    }
}
