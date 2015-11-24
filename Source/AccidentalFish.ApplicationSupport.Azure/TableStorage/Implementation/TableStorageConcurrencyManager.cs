using System;
using System.Linq;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation
{
    internal class TableStorageConcurrencyManager : ITableStorageConcurrencyManager
    {
        private readonly IAzureAssemblyLogger _logger;

        public TableStorageConcurrencyManager(IAzureAssemblyLogger logger)
        {
            _logger = logger;
            _logger?.Verbose("TableStorageConcurrencyManager: created");
        }

        public Task<T> Update<T>(IAsynchronousTableStorageRepository<T> table, string paritionKey, Action<T> update) where T : ITableEntity, new()
        {
            return Update(table, paritionKey, null, update);
        }

        public Task<T> InsertOrUpdate<T>(IAsynchronousTableStorageRepository<T> table, string partitionKey, Action<T> update,
            Func<T> insert) where T : ITableEntity, new()
        {
            return InsertOrUpdate(table, partitionKey, null, update, insert);
        }

        public async Task<T> Update<T>(IAsynchronousTableStorageRepository<T> table, string paritionKey, string rowKey, Action<T> update) where T : ITableEntity, new()
        {
            _logger?.Verbose("TableStorageConcurrencyManager: Update - commencing concurrency checked update operation");
            bool concurrencyException = true;
            T entity = default(T);
            while (concurrencyException)
            {
                entity = rowKey == null ? (await table.GetAsync(paritionKey)).Single() : await table.GetAsync(paritionKey, rowKey);
                update(entity);
                try
                {
                    await table.UpdateAsync(entity);
                    concurrencyException = false;
                }
                catch (StorageException storageException)
                {
                    if (storageException.RequestInformation.HttpStatusCode != 412)
                    {
                        throw;
                    }
                    _logger?.Verbose("TableStorageConcurrencyManager: Update - retrying concurrency checked update operation due to concurrency failure");
                }
            }
            _logger?.Verbose("TableStorageConcurrencyManager: Update - completed concurrent update");
            return entity;
        }

        public async Task<T> InsertOrUpdate<T>(IAsynchronousTableStorageRepository<T> table, string partitionKey, string rowKey, Action<T> update, Func<T> insert) where T : ITableEntity, new()
        {
            _logger?.Verbose("TableStorageConcurrencyManager: InsertOrUpdate - commencing concurrency checked update operation");
            bool concurrencyException = true;
            T entity = default(T);
            while (concurrencyException)
            {
                entity = rowKey == null ? (await table.GetAsync(partitionKey)).SingleOrDefault() : await table.GetAsync(partitionKey, rowKey);
                if (entity == null)
                {
                    try
                    {
                        entity = insert();
                        await table.InsertAsync(entity);
                        concurrencyException = false;
                    }
                    catch (UniqueKeyViolationException)
                    {
                        // someone has inserted in the meantime so we now need to update
                        _logger?.Verbose("TableStorageConcurrencyManager: InsertOrUpdate - failed as insert has occurred in the interim, now attempting update");
                    }
                }
                else
                {
                    update(entity);
                    try
                    {
                        await table.UpdateAsync(entity);
                        concurrencyException = false;
                    }
                    catch (StorageException storageException)
                    {
                        if (storageException.RequestInformation.HttpStatusCode != 412)
                        {
                            throw;
                        }
                        _logger?.Verbose("TableStorageConcurrencyManager: InsertOrUpdate - retrying concurrency checked update operation due to concurrency failure");
                    }
                }
                
            }
            return entity;
        }
    }
}
