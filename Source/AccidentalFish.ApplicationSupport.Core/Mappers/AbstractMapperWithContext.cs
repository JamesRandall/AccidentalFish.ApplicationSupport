using System.Collections.Generic;
using System.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    /// <summary>
    /// Abstract mapper that also takes a context object for more complex mapping logic
    /// </summary>
    /// <typeparam name="T1">The type to map from</typeparam>
    /// <typeparam name="T2">The type to map to</typeparam>
    /// <typeparam name="T3">The type of the context</typeparam>
    public abstract class AbstractMapperWithContext<T1, T2, T3> : IMapperWithContext<T1, T2, T3>
    {
        /// <summary>
        /// Implement to map the object using the supplied context
        /// </summary>
        /// <param name="from">Object to map from</param>
        /// <param name="context">Context to use</param>
        /// <returns>The mapped object</returns>
        public abstract T2 Map(T1 from, T3 context);

        /// <summary>
        /// Map a set of objects using a context
        /// </summary>
        /// <param name="from">Set to map from</param>
        /// <param name="context">Context object</param>
        /// <returns>Set of mapped objcets</returns>
        public IEnumerable<T2> Map(IEnumerable<T1> from, T3 context)
        {
            if (@from == null) return new T2[0];
            return @from.Select(x => Map(x, context)).ToArray();
        }

        /// <summary>
        /// Implement to map the object using the supplied context
        /// </summary>
        /// <param name="from">Object to map from</param>
        /// <param name="context">Context to use</param>
        /// <returns>The mapped object</returns>
        public abstract T1 Map(T2 @from, T3 context);

        /// <summary>
        /// Map a set of objects using a context
        /// </summary>
        /// <param name="from">Set to map from</param>
        /// <param name="context">Context object</param>
        /// <returns>Set of mapped objcets</returns>
        public IEnumerable<T1> Map(IEnumerable<T2> @from, T3 context)
        {
            if (@from == null) return new T1[0];
            return @from.Select(x => Map(x, context)).ToArray();
        }
    }
}
