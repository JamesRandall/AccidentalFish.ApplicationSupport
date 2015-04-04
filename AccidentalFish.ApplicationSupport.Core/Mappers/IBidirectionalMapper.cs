using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    public interface IBidirectionalMapper<T1, T2> : IMapper<T1, T2>
    {
        T1 Map(T2 @from);
        IEnumerable<T1> Map(IEnumerable<T2> @from);
    }
}