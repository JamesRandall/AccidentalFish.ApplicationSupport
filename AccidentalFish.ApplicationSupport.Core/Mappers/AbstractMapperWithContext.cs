using System.Collections.Generic;
using System.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    public abstract class AbstractMapperWithContext<T1, T2, T3> : IMapperWithContext<T1, T2, T3>
    {
        public abstract T2 Map(T1 @from, T3 context);

        public IEnumerable<T2> Map(IEnumerable<T1> @from, T3 context)
        {
            if (@from == null) return new T2[0];
            return @from.Select(x => Map(x, context)).ToArray();
        }

        public abstract T1 Map(T2 @from, T3 context);

        public IEnumerable<T1> Map(IEnumerable<T2> @from, T3 context)
        {
            if (@from == null) return new T1[0];
            return @from.Select(x => Map(x, context)).ToArray();
        }
    }
}
