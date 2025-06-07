using Jakub_Zajega_14987.AdminUI.Services;
using Jakub_Zajega_14987.Application.DTOs.GraphQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Jakub_Zajega_14987.AdminUI.Pages;

[Authorize]
public class UsersGraphQLModel : PageModel
{
    private readonly IApiClient _apiClient;
    public List<GqlUserNode> Users { get; set; } = new();

    public UsersGraphQLModel(IApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task OnGetAsync()
    {
        Users = await _apiClient.GetUsersFromGraphQLAsync();
    }
}