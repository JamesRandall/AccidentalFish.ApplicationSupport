using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Helpers
{
    internal interface ICopyCompiler
    {
        Func<T, T> Compile<T>();
    }
}
