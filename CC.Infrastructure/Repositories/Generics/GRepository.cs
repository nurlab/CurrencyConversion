using CC.Domain.Interfaces;
using CC.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using DbContext = Microsoft.EntityFrameworkCore.DbContext;

namespace CC.Infrastructure.Repositories.Generics;

/// <summary>
/// A generic repository that provides basic CRUD operations for entities of type <typeparamref name="T"/>.
/// </summary>
/// <remarks>
/// This repository class implements the <see cref="IGRepository{T}"/> interface and provides standard operations
/// such as adding, querying, and checking existence of entities in the database. It leverages Entity Framework Core's
/// <see cref="DbContext"/> for interacting with the underlying database.
/// </remarks>
/// <typeparam name="T">The entity type that this repository operates on. It must be a class.</typeparam>
public class GRepository<T> : IGRepository<T> where T : class
{
    #region Private Fields

    /// <summary>
    /// The database context used to interact with the database.
    /// </summary>
    protected readonly DbContext _dbContext;

    /// <summary>
    /// Flag to track whether the repository has been disposed.
    /// </summary>
    private bool _disposed = false;

    #endregion

    #region Constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="GRepository{T}"/> class with the specified <paramref name="dbContext"/>.
    /// </summary>
    /// <param name="dbContext">The <see cref="AppDbContext"/> instance to interact with the database.</param>
    public GRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Asynchronously adds a new entity to the database.
    /// </summary>
    /// <param name="t">The entity to add.</param>
    /// <returns>The added entity.</returns>
    public virtual async Task<T> AddAsync(T t)
    {
        // Adds the entity asynchronously and returns the added entity.
        var entity = await _dbContext.Set<T>().AddAsync(t);
        return entity.Entity;
    }

    /// <summary>
    /// Asynchronously checks if any record exists that matches the specified condition.
    /// </summary>
    /// <param name="predicate">The condition to test each element against.</param>
    /// <returns>A task that represents whether any elements satisfy the condition.</returns>
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
    {
        // Checks if any entity in the set matches the predicate.
        return await _dbContext.Set<T>().AnyAsync(predicate);
    }

    /// <summary>
    /// Retrieves the first record that matches the specified condition, without tracking the entity.
    /// </summary>
    /// <param name="where">The condition to find the record.</param>
    /// <returns>The first matching entity or null if no match is found.</returns>
    public T GetFirstOrDefault(Expression<Func<T, bool>> where)
    {
        // Retrieves the first matching entity, without tracking changes.
        return _dbContext.Set<T>().AsNoTracking().FirstOrDefault(where);
    }

    /// <summary>
    /// Asynchronously retrieves the first record that matches the specified condition, without tracking the entity.
    /// </summary>
    /// <param name="where">The condition to find the record.</param>
    /// <returns>A task representing the first matching entity or null if no match is found.</returns>
    public async Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where)
    {
        // Asynchronously retrieves the first matching entity, without tracking changes.
        return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(where);
    }

    #endregion
}
