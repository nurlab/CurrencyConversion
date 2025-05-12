using CC.Domain.Entities;

namespace CC.Domain.Interfaces;

/// <summary>
/// Defines the contract for the user repository, which extends from the generic repository interface.
/// </summary>
public interface IUserRepository : IGRepository<User>
{
}
