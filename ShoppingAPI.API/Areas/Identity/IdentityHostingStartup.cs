using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ShoppingAPI.API.Areas.Identity.IdentityHostingStartup))]
namespace ShoppingAPI.API.Areas.Identity
{
  public class IdentityHostingStartup : IHostingStartup
  {
    public void Configure(IWebHostBuilder builder)
    {
      //builder.ConfigureServices((context, services) => {
      //    services.AddDbContext<AppIdentityContext>(options => options.UseSqlServer(context.Configuration.GetConnectionString("ShoppingAPIDB")));

      //    services.AddDefaultIdentity<AppUser>(options => options.SignIn.RequireConfirmedAccount = true)
      //        .AddEntityFrameworkStores<AppIdentityContext>();
      //});
    }
  }
}
