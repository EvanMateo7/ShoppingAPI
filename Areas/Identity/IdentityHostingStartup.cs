using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShoppingAPI.Data;
using ShoppingAPI.Domain;

[assembly: HostingStartup(typeof(ShoppingAPI.Areas.Identity.IdentityHostingStartup))]
namespace ShoppingAPI.Areas.Identity
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