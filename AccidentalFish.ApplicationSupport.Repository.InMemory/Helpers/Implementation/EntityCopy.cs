using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Helpers.Implementation
{
    internal class EntityCopy : IEntityCopy
    {
        private readonly ICopyCompiler _copyCompiler;
        private readonly ConcurrentDictionary<Type, object> _compiledCopiers = new ConcurrentDictionary<Type, object>();

        public EntityCopy(ICopyCompiler copyCompiler)
        {
            _copyCompiler = copyCompiler;
        }

        private Func<T,T> GetEntityCopier<T>() where T : class
        {
            object copier = _compiledCopiers.GetOrAdd(typeof (T), t => _copyCompiler.Compile<T>());
            return (Func<T,T>) copier;
        }

        public T ShallowCopy<T>(T source) where T : class
        {
            return GetEntityCopier<T>()(source);
        }

        public IEnumerable<T> ShallowCopy<T>(IEnumerable<T> source) where T : class
        {
            throw new NotImplementedException();
        }

        public T ExtendedShallowCopy<T>(T source, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> ExtendedShallowCopy<T>(IEnumerable<T> source, params Expression<Func<T, object>>[] includeProperties)
        {
            throw new NotImplementedException();
        }
    }
}
