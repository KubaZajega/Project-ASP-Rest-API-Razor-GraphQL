using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Jakub_Zajega_14987.Application.DTOs.Auth;
using Jakub_Zajega_14987.AdminUI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace Jakub_Zajega_14987.AdminUI.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IApiClient _apiClient;

        [BindProperty]
        public LoginRequestDto LoginRequest { get; set; } = new();

        public LoginModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public void OnGet() { }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var loginResponse = await _apiClient.LoginAsync(LoginRequest);

                if (string.IsNullOrEmpty(loginResponse?.Token))
                {
                    ModelState.AddModelError(string.Empty, "Logowanie nie powiod³o siê. SprawdŸ dane.");
                    return Page();
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(loginResponse.Token);

                var claims = jwtToken.Claims.ToList();
                claims.Add(new Claim("AuthToken", loginResponse.Token));

                var claimsIdentity = new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    "given_name",
                    "role"
                );

                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToPage("/Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Wyst¹pi³ b³¹d serwera.");
                return Page();
            }
        }
    }
}
