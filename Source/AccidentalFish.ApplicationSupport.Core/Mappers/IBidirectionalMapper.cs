using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    /// <summary>
    /// Interface for a bi-directional mapper.
    /// </summary>
    /// <typeparam name="T1">From/to type</typeparam>
    /// <typeparam name="T2">From/to type</typeparam>
    public interface IBidirectionalMapper<T1, T2> : IMapper<T1, T2>
    {
        /// <summary>
        /// Implement to map from T2 to T1
        /// </summary>
        /// <param name="from">From</param>
        /// <returns>Mapped result</returns>
        T1 Map(T2 @from);
        /// <summary>
        /// Maps an IEnumerable of T2 to an IEnumerable of T1
        /// </summary>
        /// <param name="from"></param>
        /// <returns>Mapped results</returns>
        IEnumerable<T1> Map(IEnumerable<T2> @from);
    }
}