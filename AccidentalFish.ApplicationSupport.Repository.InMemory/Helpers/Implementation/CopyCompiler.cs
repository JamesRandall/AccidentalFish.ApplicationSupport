using System;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Helpers.Implementation
{
    internal class CopyCompiler : ICopyCompiler
    {
        public Func<T, T> Compile<T>()
        {
            throw new NotImplementedException();
        }
    }
}
