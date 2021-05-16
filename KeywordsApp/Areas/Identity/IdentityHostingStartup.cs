using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(KeywordsApp.Areas.Identity.IdentityHostingStartup))]
namespace KeywordsApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}
