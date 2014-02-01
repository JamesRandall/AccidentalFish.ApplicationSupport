using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using AccidentalFish.ApplicationSupport.Core.Policies;
using Microsoft.WindowsAzure.Storage;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    internal class LazyTableCreateAsynchronousNoSqlRepository<T> : IAsynchronousNoSqlRepository<T> where T : NoSqlEntity, new()
    {
        private const int HttpNotFound = 404;
        private readonly AsynchronousNoSqlRepository<T> _repository;

        public LazyTableCreateAsynchronousNoSqlRepository(AsynchronousNoSqlRepository<T> repository)
        {
            _repository = repository;
        }

        private Task Create()
        {
            IResourceCreator resourceCreator = _repository.GetResourceCreator();
            return resourceCreator.CreateIfNotExists();
        }


        public async Task InsertAsync(T item)
        {
            try
            {
                await _repository.InsertAsync(item);
                return;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.InsertAsync(item);
        }

        public async Task InsertBatchAsync(IEnumerable<T> items)
        {
            try
            {
                await _repository.InsertBatchAsync(items);
                return;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.InsertBatchAsync(items);
        }

        public async Task InsertOrReplaceBatchAsync(IEnumerable<T> items)
        {
            try
            {
                await _repository.InsertOrReplaceBatchAsync(items);
                return;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.InsertOrReplaceBatchAsync(items);
        }

        public async Task InsertOrUpdateAsync(T item)
        {
            try
            {
                await _repository.InsertOrUpdateAsync(item);
                return;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.InsertOrUpdateAsync(item);
        }

        public async Task InsertOrReplaceAsync(T item)
        {
            try
            {
                await _repository.InsertOrReplaceAsync(item);
                return;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.InsertOrReplaceAsync(item);
        }

        public async Task<bool> DeleteAsync(T item)
        {
            try
            {
                return await _repository.DeleteAsync(item);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            return await _repository.DeleteAsync(item);
        }

        public async Task UpdateAsync(T item)
        {
            try
            {
                await _repository.UpdateAsync(item);
                return;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.UpdateAsync(item);
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            try
            {
                T item = await _repository.GetAsync(partitionKey, rowKey);
                return item;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            return await _repository.GetAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<T>> GetAsync(string partitionKey)
        {
            try
            {
                IEnumerable<T> items = await _repository.GetAsync(partitionKey);
                return items;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            return await _repository.GetAsync(partitionKey);
        }

        public async Task<IEnumerable<T>> GetAsync(string partitionKey, int take)
        {
            try
            {
                IEnumerable<T> items = await _repository.GetAsync(partitionKey, take);
                return items;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            return await _repository.GetAsync(partitionKey, take);
        }

        public async Task<IEnumerable<T>> QueryAsync(string columnName, string value)
        {
            try
            {
                IEnumerable<T> items = await _repository.QueryAsync(columnName, value);
                return items;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            return await _repository.QueryAsync(columnName, value);
        }

        public async Task<IEnumerable<T>> QueryAsync(Dictionary<string, object> columnValues)
        {
            try
            {
                IEnumerable<T> items = await _repository.QueryAsync(columnValues);
                return items;
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            return await _repository.QueryAsync(columnValues);
        }

        public async Task QueryFuncAsync(string column, string value, Func<IEnumerable<T>, bool> func)
        {
            try
            {
                await _repository.QueryFuncAsync(column, value, func);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.QueryFuncAsync(column, value, func);
        }

        public async Task QueryActionAsync(string column, string value, Action<IEnumerable<T>> action)
        {
            try
            {
                await _repository.QueryActionAsync(column, value, action);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.QueryActionAsync(column, value, action);
        }

        public async Task AllActionAsync(Action<IEnumerable<T>> action)
        {
            try
            {
                await _repository.AllActionAsync(action);
            }
            catch (StorageException ex)
            {
                if (ex.RequestInformation.HttpStatusCode != HttpNotFound)
                {
                    throw;
                }
            }
            await Create();
            await _repository.AllActionAsync(action);
        }

        public IResourceCreator GetResourceCreator()
        {
            return _repository.GetResourceCreator();
        }
    }
}
