using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    /// <summary>
    /// Asynchronous access to a NoSql type data store
    /// </summary>
    /// <typeparam name="T">Type of the entity in the store</typeparam>
    public interface IAsynchronousNoSqlRepository<T> where T : NoSqlEntity, new()
    {
        /// <summary>
        /// Insert the entity into the store
        /// </summary>
        /// <param name="item">Item to insert</param>
        Task InsertAsync(T item);
        /// <summary>
        /// Insert a set of entities into the store
        /// </summary>
        /// <param name="items">Items to inser</param>
        /// <returns></returns>
        Task InsertBatchAsync(IEnumerable<T> items);
        /// <summary>
        /// Insert an entity into the store, update it if it already exists
        /// </summary>
        /// <param name="item">Item to insert or update</param>
        Task InsertOrUpdateAsync(T item);
        /// <summary>
        /// Delete the entity from the store
        /// </summary>
        /// <param name="item">Item to delete</param>
        Task DeleteAsync(T item);
        /// <summary>
        /// Update an entity in the store
        /// </summary>
        /// <param name="item">Item to update</param>
        /// <returns></returns>
        Task UpdateAsync(T item);
        Task<T> GetAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> GetAsync(string partitionKey);
        Task<IEnumerable<T>> GetAsync(string partitionKey, int take);
        Task<IEnumerable<T>> QueryAsync(string columnName, string value);
        Task QueryActionAsync(string column, string value, Action<IEnumerable<T>> action);
        Task AllActionAsync(Action<IEnumerable<T>> action);
    }
}
