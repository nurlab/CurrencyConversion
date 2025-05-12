namespace CC.Application.Interfaces;

/// <summary>
/// Defines a contract for implementing the Unit of Work pattern.
/// </summary>
/// <remarks>
/// A Unit of Work handles transactions and coordinates the work of multiple repositories
/// by committing changes as a single unit to ensure data consistency.
/// </remarks>
public interface IUnitOfWork
{
    /// <summary>
    /// Commits all changes made during the current transaction scope asynchronously.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    Task<int> CommitAsync();

    /// <summary>
    /// Disposes the current unit of work asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous dispose operation.</returns>
    ValueTask DisposeAsync();
}
