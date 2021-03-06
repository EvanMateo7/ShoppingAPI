using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using ShoppingAPI.Domain.AggregateRoots.AppUserAggregate;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingAPI.API.Identity
{
  public class AppClaimsTransformation : IClaimsTransformation
  {
    private readonly UserManager<AppUser> _userManager;

    public AppClaimsTransformation(UserManager<AppUser> userManager)
    {
      _userManager = userManager;
    }

    public Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
    {
      // Clone current identity
      var clone = principal.Clone();
      List<Claim> claims = new List<Claim> { new Claim("ClaimFromClaimsTransformer", "TransformAsync") };
      var newIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
      clone.AddIdentity(newIdentity);

      return Task.FromResult(clone);
    }

  }
}
