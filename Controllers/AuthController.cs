using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using ShoppingAPI.Areas.Identity;
using ShoppingAPI.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ShoppingAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly AppSignInManager<AppUser> _signInManager;
        public AuthController(UserManager<AppUser> userManager, 
                                  AppSignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet("signin/google")]
        public IActionResult AuthSignInGoogle()
        {
            var provider = GoogleDefaults.AuthenticationScheme;
            var redirectUrl = Url.Action(nameof(AuthSignInResponse));
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        [HttpGet("signin/response")]
        public async Task<IActionResult> AuthSignInResponse()
        {
            var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
            var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {
                await _signInManager.SignInAsyncWithExternalClaims(user, true, externalLoginInfo);
                return LocalRedirect(Url.Action(nameof(AuthUserClaims)));
            }
            else
            {
                user = new AppUser() { UserName = email, Email = email };
                var result = await _userManager.CreateAsync(user);

                if (result.Succeeded || result.Errors.Any(e => e.Code == nameof(IdentityErrorDescriber.DuplicateUserName)))
                {
                    await _signInManager.SignInAsyncWithExternalClaims(user, true, externalLoginInfo);
                    return LocalRedirect(Url.Action(nameof(AuthUserClaims)));
                }
            }

            return Problem();
        }

        [HttpGet("signout")]
        public async Task<IActionResult> AuthSignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(AuthUserClaims));
        }

        [HttpGet("user/claims")]
        public ActionResult AuthUserClaims()
        {
            var claims = User.Claims.Select(c => new { c.Type, c.Issuer, c.Value }).ToList();
            return Ok(new
            {
                signed_in = User.Identity.IsAuthenticated,
                claims = claims
            });
        }
    }
}
