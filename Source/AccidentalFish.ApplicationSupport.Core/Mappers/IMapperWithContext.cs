using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    /// <summary>
    /// Abstract mapper that also takes a context object for more complex mapping logic
    /// </summary>
    /// <typeparam name="T1">The type to map from</typeparam>
    /// <typeparam name="T2">The type to map to</typeparam>
    /// <typeparam name="T3">The type of the context</typeparam>
    public interface IMapperWithContext<T1, T2, in T3>
    {
        /// <summary>
        /// Implement to map the object using the supplied context
        /// </summary>
        /// <param name="from">Object to map from</param>
        /// <param name="context">Context to use</param>
        /// <returns>The mapped object</returns>
        T2 Map(T1 @from, T3 context);
        /// <summary>
        /// Map a set of objects using a context
        /// </summary>
        /// <param name="from">Set to map from</param>
        /// <param name="context">Context object</param>
        /// <returns>Set of mapped objcets</returns>
        IEnumerable<T2> Map(IEnumerable<T1> @from, T3 context);
        /// <summary>
        /// Implement to map the object using the supplied context
        /// </summary>
        /// <param name="from">Object to map from</param>
        /// <param name="context">Context to use</param>
        /// <returns>The mapped object</returns>
        T1 Map(T2 @from, T3 context);
        /// <summary>
        /// Map a set of objects using a context
        /// </summary>
        /// <param name="from">Set to map from</param>
        /// <param name="context">Context object</param>
        /// <returns>Set of mapped objcets</returns>
        IEnumerable<T1> Map(IEnumerable<T2> @from, T3 context);
    }
}
