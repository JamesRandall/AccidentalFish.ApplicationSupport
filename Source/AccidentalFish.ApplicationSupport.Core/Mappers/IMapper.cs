using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    /// <summary>
    /// Interface for a uni-directional mapper
    /// </summary>
    /// <typeparam name="T1">Map from</typeparam>
    /// <typeparam name="T2">Map to</typeparam>
    public interface IMapper<in T1, out T2>
    {
        /// <summary>
        /// Implement to map from T1 to T2
        /// </summary>
        /// <param name="from">The object to map from</param>
        /// <returns>The mapped result</returns>
        T2 Map(T1 @from);
        /// <summary>
        /// Maps a set of T1 to a set of T2
        /// </summary>
        /// <param name="from">The object set to map from</param>
        /// <returns>The mapped result set</returns>
        IEnumerable<T2> Map(IEnumerable<T1> @from);
    }
}
