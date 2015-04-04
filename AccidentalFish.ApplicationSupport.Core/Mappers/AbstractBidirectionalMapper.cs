using System.Collections.Generic;
using System.Linq;

namespace AccidentalFish.ApplicationSupport.Core.Mappers
{
    public abstract class AbstractBidirectionalMapper<T1, T2> : AbstractMapper<T1, T2>, IBidirectionalMapper<T1,T2>
    {
        public abstract T1 Map(T2 @from);

        public virtual IEnumerable<T1> Map(IEnumerable<T2> @from)
        {
            if (@from == null) return new T1[0];
            return @from.Select(Map).ToArray();
        }
    }
}
