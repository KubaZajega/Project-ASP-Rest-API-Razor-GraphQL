using Jakub_Zajega_14987.AdminUI.Services;
using Jakub_Zajega_14987.Application.DTOs.Post;
using Jakub_Zajega_14987.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Jakub_Zajega_14987.AdminUI.Pages;

[Authorize]
public class EditPostModel : PageModel
{
    private readonly IApiClient _apiClient;

    [BindProperty]
    public UpdatePostDto PostToUpdate { get; set; } = new();

    [BindProperty(SupportsGet = true)]
    public int Id { get; set; }

    public EditPostModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        var post = await _apiClient.GetPostByIdAsync(Id);
        if (post == null)
        {
            return NotFound();
        }

        PostToUpdate.Title = post.Title;
        PostToUpdate.Content = post.Content;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        await _apiClient.UpdatePostAsync(Id, PostToUpdate);
        return RedirectToPage("/Index");
    }
}