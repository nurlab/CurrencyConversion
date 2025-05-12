using CC.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CC.Application.Interfaces;

/// <summary>
/// Represents the application's database context abstraction.
/// </summary>
/// <remarks>
/// This interface is used to define the entity sets exposed by the data context,
/// primarily to facilitate dependency injection and unit testing.
/// </remarks>
public interface IAppDbContext
{
    /// <summary>
    /// Gets or sets the <see cref="DbSet{TEntity}"/> representing the users table.
    /// </summary>
    DbSet<User> Users { get; set; }
}
