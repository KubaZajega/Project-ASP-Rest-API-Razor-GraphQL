using FluentAssertions;
using Jakub_Zajega_14987.Application.DTOs.Post;
using Jakub_Zajega_14987.Domain.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Jakub_Zajega_14987.WebAPI.Tests
{
    public class PostsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PostsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetPosts_ReturnsOkResult_WithListOfPosts()
        {
            var response = await _client.GetAsync("/api/posts");
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var posts = await response.Content.ReadFromJsonAsync<List<Post>>();
            posts.Should().NotBeNull();
        }

        [Fact]
        public async Task GetPostById_WithNonExistentId_ReturnsNotFound()
        {
            var nonExistentId = 99999;
            var response = await _client.GetAsync($"/api/posts/{nonExistentId}");
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreatePost_WithoutToken_ReturnsUnauthorized()
        {
            var newPost = new CreatePostDto { Title = "Test Title", Content = "Test Content" };
            var response = await _client.PostAsJsonAsync("/api/posts", newPost);
            response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task CreatePost_WithValidToken_ReturnsCreated()
        {
            var user = new User { Id = 100, Username = "TestUserForToken", Role = "User" };
            var token = JwtTokenGeneratorUtil.GenerateTokenForUser(user);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var newPost = new CreatePostDto { Title = "Authenticated Post", Content = "This content was created by an authenticated user." };
            var response = await _client.PostAsJsonAsync("/api/posts", newPost);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            response.Headers.Location.Should().NotBeNull();
        }
    }
}
