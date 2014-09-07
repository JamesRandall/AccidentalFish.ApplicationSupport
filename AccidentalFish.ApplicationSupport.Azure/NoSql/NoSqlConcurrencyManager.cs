using System;
using System.Linq;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using Microsoft.WindowsAzure.Storage;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    internal class NoSqlConcurrencyManager : INoSqlConcurrencyManager
    {
        public Task<T> Update<T>(IAsynchronousNoSqlRepository<T> table, string paritionKey, Action<T> update) where T : NoSqlEntity, new()
        {
            return Update(table, paritionKey, null, update);
        }

        public Task<T> InsertOrUpdate<T>(IAsynchronousNoSqlRepository<T> table, string partitionKey, Action<T> update,
            Func<T> insert) where T : NoSqlEntity, new()
        {
            return InsertOrUpdate(table, partitionKey, null, update, insert);
        }

        public async Task<T> Update<T>(IAsynchronousNoSqlRepository<T> table, string paritionKey, string rowKey, Action<T> update) where T : NoSqlEntity, new()
        {
            bool concurrencyException = true;
            T entity = null;
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
                }
            }
            return entity;
        }

        public async Task<T> InsertOrUpdate<T>(IAsynchronousNoSqlRepository<T> table, string partitionKey, string rowKey, Action<T> update, Func<T> insert) where T : NoSqlEntity, new()
        {
            bool concurrencyException = true;
            T entity = null;
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
                    catch (UniqueKeyViolation)
                    {
                        // someone has inserted in the meantime so we now need to update
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
                    }
                }
                
            }
            return entity;
        }
    }
}
