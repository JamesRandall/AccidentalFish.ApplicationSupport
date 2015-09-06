using System.Collections.Generic;
using System.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    /// <summary>
    /// Base implementation for a bi-directional mapper. Implementations get a "map IEnumerable" implementation for free.
    /// </summary>
    /// <typeparam name="T1">From/to type</typeparam>
    /// <typeparam name="T2">From/to type</typeparam>
    public abstract class AbstractBidirectionalMapper<T1, T2> : AbstractMapper<T1, T2>, IBidirectionalMapper<T1,T2>
    {
        /// <summary>
        /// Implement to map from T2 to T1
        /// </summary>
        /// <param name="from">From</param>
        /// <returns>Mapped result</returns>
        public abstract T1 Map(T2 from);

        /// <summary>
        /// Maps an IEnumerable of T2 to an IEnumerable of T1
        /// </summary>
        /// <param name="from"></param>
        /// <returns>Mapped results</returns>
        public virtual IEnumerable<T1> Map(IEnumerable<T2> from)
        {
            if (@from == null) return new T1[0];
            return @from.Select(Map).ToArray();
        }
    }
}
