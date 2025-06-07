using Jakub_Zajega_14987.Application.Contracts.Persistence;
using Jakub_Zajega_14987.Application.DTOs.Post;
using Jakub_Zajega_14987.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Jakub_Zajega_14987.WebAPI.Controllers
{
    public class PostsController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _unitOfWork.PostRepository.GetAllAsync();
            return Ok(posts);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createPostDto)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out var userId))
            {
                return Unauthorized("Invalid token.");
            }

            var newPost = new Post
            {
                Title = createPostDto.Title,
                Content = createPostDto.Content,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.PostRepository.AddAsync(newPost);
            await _unitOfWork.SaveChangesAsync();

            return Created($"api/posts/{newPost.Id}", newPost);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPostById(int id)
        {
            var post = await _unitOfWork.PostRepository.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto updatePostDto)
        {
            var postToUpdate = await _unitOfWork.PostRepository.GetByIdAsync(id);
            if (postToUpdate == null)
            {
                return NotFound();
            }
            postToUpdate.Title = updatePostDto.Title;
            postToUpdate.Content = updatePostDto.Content;
            _unitOfWork.PostRepository.Update(postToUpdate);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var postToDelete = await _unitOfWork.PostRepository.GetByIdAsync(id);
            if (postToDelete == null)
            {
                return NotFound();
            }
            _unitOfWork.PostRepository.Delete(postToDelete);
            await _unitOfWork.SaveChangesAsync();
            return NoContent();
        }
    }
}