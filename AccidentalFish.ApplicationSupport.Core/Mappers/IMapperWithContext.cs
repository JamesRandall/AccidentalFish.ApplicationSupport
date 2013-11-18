using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    public interface IMapperWithContext<T1, T2, in T3>
    {
        T2 Map(T1 @from, T3 context);
        IEnumerable<T2> Map(IEnumerable<T1> @from, T3 context);
        T1 Map(T2 @from, T3 context);
        IEnumerable<T1> Map(IEnumerable<T2> @from, T3 context);
    }
}
