using System;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    /// <summary>
    /// Unit of work pattern for use with an ORM
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Retrieve a repository that acts within the unit of work
        /// </summary>
        /// <typeparam name="T">The type of the entities represented by the repository</typeparam>
        /// <returns>A repository</returns>
        IRepository<T> GetRepository<T>() where T : class;
        /// <summary>
        /// Save all changes made within the unit of work
        /// </summary>
        void Save();
    }
}
