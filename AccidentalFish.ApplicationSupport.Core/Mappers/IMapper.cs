using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
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
