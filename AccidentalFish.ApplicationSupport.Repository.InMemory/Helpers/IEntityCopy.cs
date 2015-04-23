using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Helpers
{
    internal interface IEntityCopy
    {
        /// <summary>
        /// Does a shallow copy of the entity. Any mutable reference properties
        /// are set to null at the end.
        /// </summary>
        T ShallowCopy<T>(T source) where T : class;

        /// <summary>
        /// Does a shallow copy of each entity in the collection. Any mutable reference properties
        /// are set to null at the end.
        /// </summary>
        IEnumerable<T> ShallowCopy<T>(IEnumerable<T> source) where T : class;

        /// <summary>
        /// Does the same core action as ShallowCopy but deep copies the specified reference properties including collections.
        /// Uses the same syntax as Entity Framework for accessing references within referenced collections.
        /// </summary>
        T ExtendedShallowCopy<T>(T source, params Expression<Func<T, object>>[] includeProperties);

        /// <summary>
        /// Does the same core action as ShallowCopy but deep copies the specified reference properties including collections.
        /// Uses the same syntax as Entity Framework for accessing references within referenced collections.
        /// </summary>
        IEnumerable<T> ExtendedShallowCopy<T>(IEnumerable<T> source, params Expression<Func<T, object>>[] includeProperties);
    }
}
