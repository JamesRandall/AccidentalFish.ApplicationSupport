using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    /// <summary>
    /// Repository pattern for use with an ORM that supports asynchronous access
    /// </summary>
    /// <typeparam name="T">The type of the objects represented by the repository</typeparam>
    public interface IRepositoryAsync<T> where T : class
    {
        /// <summary>
        /// Get an IQueryable interface for the repository
        /// </summary>
        IQueryable<T> All { get; }
        /// <summary>
        /// Get an IQueryable interface for the repository including child object / collection references
        /// </summary>
        /// <param name="includeProperties">The child object / collection references to also fetch</param>
        /// <returns>An IQueryable interface</returns>
        IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
        /// <summary>
        /// Find the entity with the specified integer ID
        /// </summary>
        /// <param name="id">The ID to use to locate the entity</param>
        /// <returns>An entity reference or null if not found</returns>
        Task<T> FindAsync(int id);
        /// <summary>
        /// Insert or update the entity using idFunc to return the ID of the entity
        /// </summary>
        /// <param name="entity">The entity to insert or update</param>
        /// <param name="idFunc">Function that provides the ID of the entity</param>
        void InsertOrUpdate(T entity, Func<T, int> idFunc);
        /// <summary>
        /// Insert the given entity
        /// </summary>
        /// <param name="entity">The entity to insert</param>
        void Insert(T entity);
        /// <summary>
        /// Update the entity
        /// </summary>
        /// <param name="entity">The entity to update</param>
        void Update(T entity);
        /// <summary>
        /// Delete the entity
        /// </summary>
        /// <param name="entity">The entity to delete</param>
        void Delete(T entity);
        /// <summary>
        /// Delete the entity with the given ID
        /// </summary>
        /// <param name="id">The primary key of the entity to delete</param>
        Task DeleteAsync(int id);

    }
}
