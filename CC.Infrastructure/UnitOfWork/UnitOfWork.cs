using CC.Application.Interfaces;
using CC.Infrastructure.DatabaseContext;

namespace CC.Infrastructure.UnitOfWork;

/// <summary>
/// The UnitOfWork class provides a mechanism for managing transactions and handling 
/// database context disposal in an efficient and consistent manner.
/// It is designed to be used with a single database context within a scope of a unit of work.
/// </summary>
public class UnitOfWork : IUnitOfWork, IAsyncDisposable
{
    /// <summary>
    /// The database context that handles the interaction with the underlying database.
    /// </summary>
    private readonly AppDbContext _dbContext;

    /// <summary>
    /// Flag to indicate whether the object has already been disposed.
    /// </summary>
    private bool disposed = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
    /// </summary>
    /// <param name="dbContext">The database context that will be used to interact with the database.</param>
    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext), "Database context cannot be null.");
    }

    /// <summary>
    /// Commits all changes made to the database context within the current transaction.
    /// It ensures that all modifications are saved to the database in a single transaction.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> CommitAsync()
    {
        // Asynchronously commits changes to the database context.
        return await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Asynchronously disposes of the database context and releases all resources.
    /// Ensures that the database context is disposed of when the unit of work is no longer needed.
    /// </summary>
    /// <returns>A value task representing the asynchronous disposal operation.</returns>
    public async ValueTask DisposeAsync()
    {
        // Only dispose if not already disposed.
        if (!disposed)
        {
            if (_dbContext != null)
            {
                // Asynchronously dispose the database context.
                await _dbContext.DisposeAsync();
            }

            // Mark the object as disposed.
            disposed = true;
        }

        // Suppress finalization since DisposeAsync has been called.
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Releases resources synchronously.
    /// This is the legacy method used in the standard IDisposable pattern.
    /// </summary>
    /// <param name="disposing">A boolean flag indicating whether to release managed resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        // Only dispose if not already disposed.
        if (!disposed)
        {
            if (disposing)
            {
                // Dispose of managed resources (i.e., database context).
                _dbContext?.Dispose();
            }

            // Mark the object as disposed.
            disposed = true;
        }
    }

    /// <summary>
    /// Destructor (finalizer) to ensure that resources are disposed of if Dispose was not called.
    /// </summary>
    ~UnitOfWork()
    {
        // Ensure resources are disposed if not already done via DisposeAsync or Dispose.
        Dispose(false);
    }
}
