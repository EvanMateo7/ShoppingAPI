using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingAPI.API.Areas.Identity
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
      identity.AddClaim(new Claim("ClaimFromPrincipalFactory", "123"));
      return identity;
    }

    public async override Task<ClaimsPrincipal> CreateAsync(AppUser user)
    {
      var id = await GenerateClaimsAsync(user);
      if (user != null)
      {
        id.AddClaim(new Claim("ClaimFromPrincipalFactory", "123"));
      }
      return new ClaimsPrincipal(id);
    }
  }
}
