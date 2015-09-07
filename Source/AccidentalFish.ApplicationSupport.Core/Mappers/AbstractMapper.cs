using System.Collections.Generic;
using System.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    /// <summary>
    /// Maps items from type T1 to T2. Implementations get an IEnumerable set mapper for free.
    /// </summary>
    /// <typeparam name="T1">The from type</typeparam>
    /// <typeparam name="T2">The to type</typeparam>
    public abstract class AbstractMapper<T1,T2> : IMapper<T1, T2>
    {
        /// <summary>
        /// Implement to map from T1 to T2
        /// </summary>
        /// <param name="from">The object to map from</param>
        /// <returns>The mapped result</returns>
        public abstract T2 Map(T1 from);

        /// <summary>
        /// Maps a set of T1 to a set of T2
        /// </summary>
        /// <param name="from">The object set to map from</param>
        /// <returns>The mapped result set</returns>
        public virtual IEnumerable<T2> Map(IEnumerable<T1> from)
        {
            if (@from == null) return new T2[0];
            return @from.Select(Map).ToArray();
        }
    }
}
