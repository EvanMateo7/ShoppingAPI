using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingAPI.API.Identity
{
  public class AppUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
  {
    public AppUserClaimsPrincipalFactory(UserManager<AppUser> userManager,
                                         IOptions<IdentityOptions> optionsAccessor)
                                         : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
      var identity = await base.GenerateClaimsAsync(user);
      identity.AddClaim(new Claim("ClaimFromPrincipalFactory", "GenerateClaimsAsync"));
      return identity;
    }

    public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
    {
      var identity = await GenerateClaimsAsync(user);
      if (user != null)
      {
        identity.AddClaim(new Claim("ClaimFromPrincipalFactory", "CreateAsync"));
      }
      return new ClaimsPrincipal(identity);
    }
  }
}
