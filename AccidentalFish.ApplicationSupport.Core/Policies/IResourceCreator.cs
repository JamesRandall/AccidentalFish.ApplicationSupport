using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Policies
{
    /// <summary>
    /// An interface that can be attached to implementations to support resource creation (such as Azure tables)
    /// </summary>
    public interface IResourceCreator
    {
        /// <summary>
        /// Create the resource if it doesn't exist
        /// </summary>
        /// <returns>An awaitable task</returns>
        Task CreateIfNotExistsAsync();
    }
}
