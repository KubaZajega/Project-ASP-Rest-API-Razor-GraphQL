using Jakub_Zajega_14987.AdminUI.Services;
using Jakub_Zajega_14987.Application.DTOs.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Jakub_Zajega_14987.AdminUI.Pages
{
    [Authorize]
    public class CreatePostModel : PageModel
    {
        private readonly IApiClient _apiClient;

        [BindProperty]
        public CreatePostDto NewPost { get; set; } = new();

        public CreatePostModel(IApiClient apiClient)
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
            await _apiClient.CreatePostAsync(NewPost);
            return RedirectToPage("/Index");
        }
    }
}