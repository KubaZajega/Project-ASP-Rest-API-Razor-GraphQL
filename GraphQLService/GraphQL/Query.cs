using Jakub_Zajega_14987.Domain.Entities;
using Jakub_Zajega_14987.Infrastructure.Persistence;

namespace Jakub_Zajega_14987.GraphQLService.GraphQL
{
    public class Query
    {
        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Post> GetPosts(AppDbContext context)
        {
            return context.Posts;
        }

        [UsePaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<User> GetUsers(AppDbContext context)
        {
            return context.Users;
        }
    }
}