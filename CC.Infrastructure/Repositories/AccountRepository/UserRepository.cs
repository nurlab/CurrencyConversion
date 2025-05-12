using CC.Domain.Entities;
using CC.Domain.Interfaces;
using CC.Infrastructure.DatabaseContext;
using CC.Infrastructure.Repositories.Generics;

namespace CC.Infrastructure.Repositories.AccountRepository;

/// <summary>
/// Provides repository functionality for managing user data.
/// </summary>
/// <remarks>
/// The <see cref="UserRepository"/> class inherits from the generic repository base class <see cref="GRepository{User}"/>.
/// It implements the <see cref="IUserRepository"/> interface, providing data access operations specific to the <see cref="User"/> entity.
/// This class encapsulates interactions with the <see cref="AppDbContext"/> to perform CRUD operations for the <see cref="User"/> entity.
/// </remarks>
class UserRepository : GRepository<User>, IUserRepository
{
    /// <summary>
    /// Initializes a new instance of the <see cref="UserRepository"/> class.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext"/> instance used to interact with the database.</param>
    /// <remarks>
    /// The constructor passes the <paramref name="dbContext"/> to the base class <see cref="GRepository{User}"/>
    /// to initialize the repository with the necessary database context for performing operations.
    /// </remarks>
    public UserRepository(AppDbContext dbContext) : base(dbContext)
    {
    }
}
