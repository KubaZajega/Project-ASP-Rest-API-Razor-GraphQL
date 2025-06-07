using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jakub_Zajega_14987.Domain.Entities;

namespace Jakub_Zajega_14987.Application.Contracts.Persistence;

public interface ICommentRepository : IGenericRepository<Comment>
{
    // W przyszłości możemy dodać tu specyficzne metody, np.:
    // Task<IReadOnlyList<Comment>> GetAllCommentsForPostAsync(int postId);
}
