using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jakub_Zajega_14987.Application.Contracts.Persistence;
public interface IUnitOfWork : IDisposable
{
    IPostRepository PostRepository { get; }
    IUserRepository UserRepository { get; }
    ICommentRepository CommentRepository { get; }
    Task<int> SaveChangesAsync();
}