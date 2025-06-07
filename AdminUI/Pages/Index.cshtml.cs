using Jakub_Zajega_14987.AdminUI.Services;
using Jakub_Zajega_14987.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Jakub_Zajega_14987.AdminUI.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IApiClient _apiClient;
        public List<Post> Posts { get; set; } = new();
        public List<Claim> UserClaims { get; set; } = new();

        public IndexModel(IApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task OnGetAsync()
        {
            Posts = await _apiClient.GetPostsAsync();

            if (User.Identity?.IsAuthenticated == true)
            {
                UserClaims = User.Claims.ToList();
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            if (!User.IsInRole("Admin"))
            {
                return Forbid();
            }
            await _apiClient.DeletePostAsync(id);
            return RedirectToPage();
        }
    }
}