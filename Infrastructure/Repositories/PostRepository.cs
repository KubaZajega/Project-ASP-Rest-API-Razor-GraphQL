using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jakub_Zajega_14987.Application.Contracts.Persistence;
using Jakub_Zajega_14987.Domain.Entities;
using Jakub_Zajega_14987.Infrastructure.Persistence;

namespace Jakub_Zajega_14987.Infrastructure.Repositories;

public class PostRepository : GenericRepository<Post>, IPostRepository
{
    public PostRepository(AppDbContext context) : base(context)
    {
    }
}