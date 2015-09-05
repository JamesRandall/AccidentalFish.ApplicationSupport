using System;
using System.Threading;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWorkAsync : IDisposable
    {
        /// <summary>
        /// Retrieve a repository that acts within the unit of work
        /// </summary>
        /// <typeparam name="T">The type of the entities represented by the repository</typeparam>
        /// <returns>A repository</returns>
        [Obsolete("There is no reason for this method to utilise a task and therefore this will be removed in the next major version. Use GetRepository<T> instead.")]
        Task<IRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class;
        /// <summary>
        /// Retrieve a repository that acts within the unit of work
        /// </summary>
        /// <typeparam name="T">The type of the entities represented by the repository</typeparam>
        /// <returns>A repository</returns>
        IRepositoryAsync<T> GetRepository<T>() where T : class;
        /// <summary>
        /// Save the changes made within the unit of work
        /// </summary>
        /// <returns>The number of objects written to the underlying database</returns>
        Task<int> SaveAsync();
        /// <summary>
        /// Using an optimistic concurrency approach attempt to perform the supplied actions within the unit of work saving the changes.
        /// This will retry until it succeeds.
        /// (Concurrency issues are detected via the DbUpdateConcurrencyException exception).
        /// </summary>
        /// <param name="update">The actions to perform</param>
        /// <returns>An awaitable task</returns>
        Task OptimisticRepositoryWinsUpdateAsync(Action update);

        /// <summary>
        /// Using an optimistic concurrency approach attempt to perform the supplied actions within the unit of work saving the changes.
        /// This will retry maxRetries times.
        /// (Concurrency issues are detected via the DbUpdateConcurrencyException exception).
        /// </summary>
        /// <param name="update">The actions to perform</param>
        /// <param name="maxRetries">The number of times to retry</param>
        /// <returns>An awaitable task</returns>
        Task<bool> OptimisticRepositoryWinsUpdateAsync(Action update, int maxRetries);
    }
}
