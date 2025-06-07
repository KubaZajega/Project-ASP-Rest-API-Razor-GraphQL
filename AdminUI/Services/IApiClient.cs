using Jakub_Zajega_14987.Application.DTOs.Auth;
using Jakub_Zajega_14987.Application.DTOs.GraphQL;
using Jakub_Zajega_14987.Application.DTOs.Post;
using Jakub_Zajega_14987.Domain.Entities;

namespace Jakub_Zajega_14987.AdminUI.Services;

public interface IApiClient
{
    Task<List<Post>> GetPostsAsync();
    Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest);
    Task CreatePostAsync(CreatePostDto newPost);
    Task<Post?> GetPostByIdAsync(int id);
    Task UpdatePostAsync(int id, UpdatePostDto postToUpdate);
    Task DeletePostAsync(int id);
    Task<List<GqlUserNode>> GetUsersFromGraphQLAsync();
}