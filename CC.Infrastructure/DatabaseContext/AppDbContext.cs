
using CC.Application.Interfaces;
using CC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CC.Infrastructure.DatabaseContext;

/// <summary>
/// Represents the application's database context, providing access to the application's data models.
/// </summary>
public class AppDbContext : DbContext, IAppDbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the <see cref="DbContext"/>.</param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the <see cref="DbSet{User}"/> representing the users in the database.
    /// </summary>
    public DbSet<User> Users { get; set; }
}
