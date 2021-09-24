using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingAPI.Pages
{
    public class OptionConfiguration
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;
        
        public OptionConfiguration(IConfiguration configuration, IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }

        public void ConfigureGoogleOptions(GoogleOptions options)
        {
            IConfigurationSection googleAuthNSection =
                _config.GetSection("Authentication:Google");

            options.ClientId = googleAuthNSection["ClientId"];
            options.ClientSecret = googleAuthNSection["ClientSecret"];
            options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
            options.ClaimActions.MapJsonKey("urn:google:locale", "locale", "string");
            options.SaveTokens = true;

            options.Events.OnCreatingTicket = context =>
            {
                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Role, "superadmin")
                    };
                var appIdentity = new ClaimsIdentity(claims);

                context.Principal.AddIdentity(appIdentity);

                return Task.CompletedTask;
            };
        }

        public void ConfigureIdentityOptions(IdentityOptions options)
        {
            // https://github.com/dotnet/AspNetCore.Docs/issues/13206
            if (_env.IsDevelopment())
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
            }
            else
            {
                options.SignIn.RequireConfirmedAccount = true;
            }
            options.User.RequireUniqueEmail = true;
        }
    }
}
