using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jakub_Zajega_14987.Application.Contracts.Persistence;
using Jakub_Zajega_14987.Infrastructure.Persistence;

namespace Jakub_Zajega_14987.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private IPostRepository? _postRepository;
    private IUserRepository? _userRepository;
    private ICommentRepository? _commentRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public IPostRepository PostRepository => _postRepository ??= new PostRepository(_context);
    public IUserRepository UserRepository => _userRepository ??= new UserRepository(_context);
    public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_context);

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}