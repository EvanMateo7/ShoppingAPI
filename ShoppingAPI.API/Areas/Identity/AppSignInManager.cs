using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingAPI.API.Areas.Identity
{
  public class AppSignInManager<TUser> : SignInManager<TUser> where TUser : class
  {
    public AppSignInManager(UserManager<TUser> userManager,
                            IHttpContextAccessor contextAccessor,
                            IUserClaimsPrincipalFactory<TUser> claimsFactory,
                            IOptions<IdentityOptions> optionsAccessor,
                            ILogger<SignInManager<TUser>> logger,
                            IAuthenticationSchemeProvider schemes,
                            IUserConfirmation<TUser> confirmation)
                            : base(userManager,
                                   contextAccessor,
                                   claimsFactory,
                                   optionsAccessor,
                                   logger,
                                   schemes,
                                   confirmation)
    {
    }

    /// <summary>
    ///  <para>Sign-in with external provider claims.</para>
    ///  <para>This copies the external claims to the cookie.</para>
    /// </summary>
    public async Task SignInAsyncWithExternalClaims(TUser user,
                                                    bool isPersistent,
                                                    ExternalLoginInfo externalLoginInfo)
    {
      var loginProvider = externalLoginInfo.LoginProvider;
      var authProps = new AuthenticationProperties();
      authProps.StoreTokens(externalLoginInfo.AuthenticationTokens);
      authProps.IsPersistent = isPersistent;

      if (loginProvider != null)
      {
        IList<Claim> externalClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.AuthenticationMethod, loginProvider)
                };

        // Add external claims
        var claims = externalLoginInfo
                        .Principal
                        .Claims
                        .Where(c => c.Type.StartsWith("urn:"));
        var allExternalClaims = externalClaims.Concat(claims);
        await SignInWithClaimsAsync(user, authProps, allExternalClaims);
      }
      else
      {
        await SignInAsync(user, authProps);
      }
    }
  }
}
