namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    /// <summary>
    /// Creates unit of work implementations
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Create a synchronous unit of work
        /// </summary>
        /// <returns>The unit of work</returns>
        IUnitOfWork Create();
        /// <summary>
        /// Create an asynchronous unit of work
        /// </summary>
        /// <returns></returns>
        IUnitOfWorkAsync CreateAsync();
    }
}
