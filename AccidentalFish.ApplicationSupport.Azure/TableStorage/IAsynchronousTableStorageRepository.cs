using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Policies;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage
{
    public enum TableStorageQueryOperator
    {
        And,
        Or
    }
    /// <summary>
    /// Asynchronous access to a NoSql type data store
    /// </summary>
    /// <typeparam name="T">Type of the entity in the store</typeparam>
    public interface IAsynchronousTableStorageRepository<T> where T : ITableEntity, new()
    {
        /// <summary>
        /// Insert the entity into the store
        /// </summary>
        /// <param name="item">Item to insert</param>
        Task InsertAsync(T item);
        /// <summary>
        /// Insert a set of entities into the store
        /// </summary>
        /// <param name="items">Items to insert</param>
        /// <returns></returns>
        Task InsertBatchAsync(IEnumerable<T> items);
        /// <summary>
        /// Inserts or replaces a set of entities into the store
        /// </summary>
        /// <param name="items">Items to insert</param>
        /// <returns></returns>
        Task InsertOrReplaceBatchAsync(IEnumerable<T> items);
        /// <summary>
        /// Insert an entity into the store, update it if it already exists
        /// </summary>
        /// <param name="item">Item to insert or update</param>
        Task InsertOrUpdateAsync(T item);
        /// <summary>
        /// Insert an entity into the store, replace it if it already exists
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task InsertOrReplaceAsync(T item);
        /// <summary>
        /// Delete the entity from the store
        /// </summary>
        /// <param name="item">Item to delete</param>
        Task<bool> DeleteAsync(T item);
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
        Task<IEnumerable<T>> QueryAsync(Dictionary<string, object> columnValues);
        Task QueryFuncAsync(string column, string value, Func<IEnumerable<T>, bool> func);
        Task QueryFuncAsync(Dictionary<string, object> conditions, TableStorageQueryOperator op, Func<IEnumerable<T>, bool> func);
        Task QueryFuncAsync(string column, IEnumerable<object> values , TableStorageQueryOperator op, Func<IEnumerable<T>, bool> func);
        Task QueryActionAsync(string column, string value, Action<IEnumerable<T>> action);
        Task AllActionAsync(Action<IEnumerable<T>> action);

        Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, int pageSize);
        Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, int pageSize, string serializedContinuationToken);
        Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, TableStorageQueryOperator op, int pageSize);
        Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, TableStorageQueryOperator op, int pageSize, string serializedContinuationToken);
        Task<PagedResultSegment<T>> PagedQueryAsync(string filter, int pageSize, string serializedContinuationToken);

        IResourceCreator GetResourceCreator();
    }
}
