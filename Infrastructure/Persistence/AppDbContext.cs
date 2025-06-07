using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jakub_Zajega_14987.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Jakub_Zajega_14987.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "TestUser",
                Role = "User",
                PasswordHash = "hash_usera"
            },
            new User
            {
                Id = 2,
                Username = "TestAdmin",
                Role = "Admin",
                PasswordHash = "hash_admina"
            }
        );

        modelBuilder.Entity<Post>().HasData(
            new Post
            {
                Id = 1,
                Title = "Pierwszy Post z Seedingu",
                Content = "To jest treść posta dodanego przy starcie aplikacji.",
                CreatedAt = DateTime.UtcNow,
                UserId = 1
            },
            new Post
            {
                Id = 2,
                Title = "Drugi Post od Admina",
                Content = "Administrator też może pisać posty!",
                CreatedAt = DateTime.UtcNow,
                UserId = 2
            }
        );
    }
}