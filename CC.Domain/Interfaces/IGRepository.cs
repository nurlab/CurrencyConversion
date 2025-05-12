using System.Linq.Expressions;

namespace CC.Domain.Interfaces
{
    /// <summary>
    /// Defines a contract for generic repository operations.
    /// </summary>
    /// <typeparam name="T">The type of entity this repository will operate on.</typeparam>
    public interface IGRepository<T> where T : class
    {
        /// <summary>
        /// Adds a new entity asynchronously to the repository.
        /// </summary>
        /// <param name="t">The entity to add.</param>
        /// <returns>The added entity.</returns>
        Task<T> AddAsync(T t);

        /// <summary>
        /// Checks if any entity in the repository matches the given predicate.
        /// </summary>
        /// <param name="predicate">The condition to evaluate for entity matching.</param>
        /// <returns><c>true</c> if any entity matches the predicate; otherwise, <c>false</c>.</returns>
        Task<bool> AnyAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the first entity that matches the specified condition or returns <c>null</c> if no match is found.
        /// </summary>
        /// <param name="where">The condition to filter entities.</param>
        /// <returns>The first entity that matches the condition, or <c>null</c> if no match is found.</returns>
        T GetFirstOrDefault(Expression<Func<T, bool>> where);

        /// <summary>
        /// Asynchronously gets the first entity that matches the specified condition or returns <c>null</c> if no match is found.
        /// </summary>
        /// <param name="where">The condition to filter entities.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the first entity that matches the condition, or <c>null</c> if no match is found.</returns>
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> where);
    }
}
