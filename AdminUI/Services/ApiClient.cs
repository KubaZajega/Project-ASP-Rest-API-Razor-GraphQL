using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Jakub_Zajega_14987.Application.DTOs.Auth;
using Jakub_Zajega_14987.Application.DTOs.GraphQL;
using Jakub_Zajega_14987.Application.DTOs.Post;
using Jakub_Zajega_14987.Domain.Entities;

namespace Jakub_Zajega_14987.AdminUI.Services
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public ApiClient(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }

        private void AddAuthorizationHeader()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;
            var token = _httpContextAccessor.HttpContext?.User.FindFirst("AuthToken")?.Value;
            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                var posts = await _httpClient.GetFromJsonAsync<List<Post>>("api/posts");
                return posts ?? new List<Post>();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Post>();
            }
        }

        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto loginRequest)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginRequest);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            }
            return null;
        }

        public async Task CreatePostAsync(CreatePostDto newPost)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.PostAsJsonAsync("api/posts", newPost);
            response.EnsureSuccessStatusCode();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            return await _httpClient.GetFromJsonAsync<Post?>($"api/posts/{id}");
        }

        public async Task UpdatePostAsync(int id, UpdatePostDto postToUpdate)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.PutAsJsonAsync($"api/posts/{id}", postToUpdate);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeletePostAsync(int id)
        {
            AddAuthorizationHeader();
            var response = await _httpClient.DeleteAsync($"api/posts/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<List<GqlUserNode>> GetUsersFromGraphQLAsync()
        {
            var graphQlEndpoint = _configuration["ApiClient:GraphQLAddress"];
            var query = new
            {
                query = @"query GetUsers { users { nodes { id username role } } }"
            };

            var response = await _httpClient.PostAsJsonAsync(graphQlEndpoint, query);
            response.EnsureSuccessStatusCode();

            var gqlResponse = await response.Content.ReadFromJsonAsync<GqlUsersResponse>();
            return gqlResponse?.Data?.Users?.Nodes ?? new List<GqlUserNode>();
        }
    }
}
