using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    public interface IMapper<T1, T2>
    {
        T2 Map(T1 @from);
        IEnumerable<T2> Map(IEnumerable<T1> @from);
        T1 Map(T2 @from);
        IEnumerable<T1> Map(IEnumerable<T2> @from);
    }
}
