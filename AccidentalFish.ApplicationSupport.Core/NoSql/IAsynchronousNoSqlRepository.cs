using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    public interface IAsynchronousNoSqlRepository<T> where T : NoSqlEntity, new()
    {
        Task InsertAsync(T item);
        Task InsertBatchAsync(IEnumerable<T> items);
        Task InsertOrUpdateAsync(T item);
        Task DeleteAsync(T item);
        Task UpdateAsync(T item);
        Task<T> GetAsync(string partitionKey, string rowKey);
        Task<IEnumerable<T>> GetAsync(string partitionKey);
        Task<IEnumerable<T>> GetAsync(string partitionKey, int take);
        Task<IEnumerable<T>> QueryAsync(string columnName, string value);
    }
}
