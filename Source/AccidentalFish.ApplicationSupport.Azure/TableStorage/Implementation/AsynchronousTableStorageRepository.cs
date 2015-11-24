using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.Extensions;
using AccidentalFish.ApplicationSupport.Azure.Logging;
using AccidentalFish.ApplicationSupport.Core.Extensions;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.RetryPolicies;
using Microsoft.WindowsAzure.Storage.Table;
using static System.String;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation
{
    internal class AsynchronousTableStorageRepository<T> : IAsynchronousTableStorageRepository<T> where T : ITableEntity, new()
    {
        private readonly string _tableName;
        private readonly ITableStorageQueryBuilder _tableStorageQueryBuilder;
        private readonly ITableContinuationTokenSerializer _tableContinuationTokenSerializer;
        private readonly IAzureAssemblyLogger _logger;
        private readonly CloudTable _table;

        private const int MaxBatchSize = 100;

        public AsynchronousTableStorageRepository(
            string connectionString,
            string tableName,
            ITableStorageQueryBuilder tableStorageQueryBuilder,
            ITableContinuationTokenSerializer tableContinuationTokenSerializer,
            IAzureAssemblyLogger logger)
        {
            _tableName = tableName;
            _tableStorageQueryBuilder = tableStorageQueryBuilder;
            _tableContinuationTokenSerializer = tableContinuationTokenSerializer;
            _logger = logger;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            tableClient.DefaultRequestOptions.RetryPolicy = new ExponentialRetry(TimeSpan.FromSeconds(120), 3);
            _table = tableClient.GetTableReference(tableName);

            _logger?.Verbose("AsynchronousTableStorageRepository: created for table {0}", tableName);
        }

        public async Task InsertAsync(T item)
        {
            TableOperation operation = TableOperation.Insert(item);
            try
            {
                _logger?.Verbose("AsynchronousTableStorageRepository: InsertAsync - inserting item for table {0}", _tableName);
                await _table.ExecuteAsync(operation);
            }
            catch (StorageException ex)
            {
                _logger?.Verbose("AsynchronousTableStorageRepository: InsertAsync - storage exception for table {0}", ex, _tableName);
                if (ex.RequestInformation.HttpStatusCode == 409)
                {
                    throw new UniqueKeyViolationException(item.PartitionKey, item.RowKey, ex);
                }
                throw;
            }
            
        }

        public async Task InsertBatchAsync(IEnumerable<T> items)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: InsertBatchAsync - inserting items for table {0}", _tableName);
            List<Task> tasks = new List<Task>();
            foreach (IEnumerable<T> page in items.Page(MaxBatchSize))
            {
                TableBatchOperation operation = new TableBatchOperation();
                foreach (T item in page)
                {
                    operation.Insert(item);
                }
                tasks.Add(_table.ExecuteBatchAsync(operation));
            }
            _logger?.Verbose("AsynchronousTableStorageRepository: InsertBatchAsync - broken batch into {0} pages for table {1}", tasks.Count, _tableName);

            await Task.WhenAll(tasks);
        }

        public async Task InsertOrReplaceBatchAsync(IEnumerable<T> items)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: InsertOrReplaceBatchAsync - insert or replacing items for table {0}", _tableName);
            List<Task> tasks = new List<Task>();
            foreach (IEnumerable<T> page in items.Page(MaxBatchSize))
            {
                TableBatchOperation operation = new TableBatchOperation();
                foreach (T item in page)
                {
                    operation.InsertOrReplace(item);
                }
                tasks.Add(_table.ExecuteBatchAsync(operation));
            }
            _logger?.Verbose("AsynchronousTableStorageRepository: InsertOrReplaceBatchAsync - broken batch into {0} pages for table {1}", tasks.Count, _tableName);

            await Task.WhenAll(tasks);
        }

        public async Task<T> GetAsync(string partitionKey, string rowKey)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: GetAsync - getting item with partition and row key for table {0}", _tableName);
            TableOperation operation = TableOperation.Retrieve<T>(partitionKey, rowKey);
            TableResult result = await _table.ExecuteAsync(operation);
            return (T)result.Result;
        }

        public async Task<IEnumerable<T>> GetAsync(string partitionKey)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: GetAsync - getting items for a partition for table {0}", _tableName);
            TableQuery<T> query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));
            IEnumerable<T> result = await _table.ExecuteQueryAsync(query);
            return result;
        }

        public async Task<IEnumerable<T>> GetAsync(string partitionKey, int take)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: GetAsync - getting {0} items for a partition for table {1}", take, _tableName);
            TableQuery<T> query = new TableQuery<T>()
                .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey))
                .Take(take);
            IEnumerable<T> result = await _table.ExecuteQueryAsync(query);
            return result;
        }

        public Task InsertOrUpdateAsync(T item)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: InsertOrUpdateAsync - for table {0}", _tableName);
            TableOperation operation = TableOperation.InsertOrMerge(item);
            return _table.ExecuteAsync(operation);
        }

        public Task InsertOrReplaceAsync(T item)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: InsertOrReplaceAsync - for table {0}", _tableName);
            TableOperation operation = TableOperation.InsertOrReplace(item);
            return _table.ExecuteAsync(operation);
        }

        public async Task<bool> DeleteAsync(T item)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: DeleteAsync - for table {0}", _tableName);
            TableOperation operation = TableOperation.Delete(item);
            TableResult result = await _table.ExecuteAsync(operation);
            return result.HttpStatusCode == 204;
        }

        public Task UpdateAsync(T item)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: UpdateAsync - for table {0}", _tableName);
            TableOperation operation = TableOperation.Replace(item);
            return _table.ExecuteAsync(operation);
        }

        #region Querying

        public async Task<IEnumerable<T>> QueryAsync(string column, string value)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryAsync - on column {0} for table {1}", column, _tableName);
            List<T> results = new List<T>();
            TableQuery<T> query = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(column, QueryComparisons.Equal, value));
            TableQuerySegment<T> querySegment = null;

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await _table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                results.AddRange(querySegment.Results);
            }
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryAsync - on column {0} for table {1} returned {2} results", column, _tableName, results.Count);
            return results;
        }

        public async Task<IEnumerable<T>> QueryAsync(Dictionary<string, object> columnValues)
        {
            if (columnValues != null && columnValues.Keys.Any())
            {
                _logger?.Verbose("AsynchronousTableStorageRepository: QueryAsync - on columns {0} for table {1}",
                    Join(",", columnValues.Keys), _tableName);
            }
            else
            {
                _logger?.Verbose("AsynchronousTableStorageRepository: QueryAsync - no column value pairs specified for table {0}", _tableName);
            }

            List<T> results = new List<T>();

            TableQuery<T> query = _tableStorageQueryBuilder.TableQuery<T>(columnValues, TableStorageQueryOperator.And);
            TableQuerySegment<T> querySegment = null;

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await _table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                results.AddRange(querySegment.Results);
            }
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryAsync - on columns for table {0} returned {1} rows",
                     _tableName, results.Count);
            return results;
        }

        public async Task QueryActionAsync(string column, string value, Func<IEnumerable<T>, Task> action)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryActionAsync - on column {0} for table {1}", column, _tableName);
            var query = !IsNullOrWhiteSpace(column) ? new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(column, QueryComparisons.Equal, value)) : new TableQuery<T>();
            
            TableQuerySegment<T> querySegment = null;
            int results = 0;

            while (querySegment == null || querySegment.ContinuationToken != null)
            {
                querySegment = await _table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                results += querySegment.Results.Count;
                await action(new List<T>(querySegment.Results));
            }
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryActionAsync - on column {0} for table {1} processed {2} rows", column, _tableName, results);
        }

        public async Task QueryFuncAsync(string column, string value, Func<IEnumerable<T>, Task<bool>> func)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryFuncAsync - on column {0} for table {1}", column, _tableName);

            var query = !IsNullOrWhiteSpace(column) ? new TableQuery<T>().Where(TableQuery.GenerateFilterCondition(column, QueryComparisons.Equal, value)) : new TableQuery<T>();

            TableQuerySegment<T> querySegment = null;
            bool doContinue = true;
            int results = 0;

            while ((querySegment == null || querySegment.ContinuationToken != null) && doContinue)
            {
                querySegment = await _table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                results += querySegment.Results.Count;
                doContinue = await func(new List<T>(querySegment.Results));
            }

            _logger?.Verbose("AsynchronousTableStorageRepository: QueryFuncAsync - on column {0} for table {1} processed {2} rows", column, _tableName, results);
        }

        public async Task QueryFuncAsync(string column, IEnumerable<object> values, TableStorageQueryOperator op,
            Func<IEnumerable<T>, Task<bool>> func)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryFuncAsync - on column {0} for table {1} with operator {2}", column, _tableName, op);

            TableQuery<T> query = _tableStorageQueryBuilder.TableQuery<T>(column, values, op);
            
            TableQuerySegment<T> querySegment = null;
            bool doContinue = true;
            int results = 0;

            while ((querySegment == null || querySegment.ContinuationToken != null) && doContinue)
            {
                querySegment = await _table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                results += querySegment.Results.Count;
                doContinue = await func(new List<T>(querySegment.Results));
            }
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryFuncAsync - on column {0} for table {1} with operator {2} processed {3} rows", column, _tableName, op, results);
        }

        public async Task QueryFuncAsync(Dictionary<string, object> conditions, TableStorageQueryOperator op, Func<IEnumerable<T>, Task<bool>> func)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryFuncAsync - on multipple columns for table {0} with operator {1}", _tableName, op);
            TableQuery<T> query =  _tableStorageQueryBuilder.TableQuery<T>(conditions, op);
            TableQuerySegment<T> querySegment = null;
            bool doContinue = true;
            int results = 0;

            while ((querySegment == null || querySegment.ContinuationToken != null) && doContinue)
            {
                querySegment = await _table.ExecuteQuerySegmentedAsync(query, querySegment?.ContinuationToken);
                results += querySegment.Results.Count;
                doContinue = await func(new List<T>(querySegment.Results));
            }
            _logger?.Verbose("AsynchronousTableStorageRepository: QueryFuncAsync - on multipple columns for table {0} with operator {1} processed {2} rows", _tableName, op, results);
        }

        public async Task AllActionAsync(Func<IEnumerable<T>, Task> action)
        {
            await QueryActionAsync(null, null, action);
        }

        public Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, int pageSize)
        {
            return PagedQueryAsync(columnValues, TableStorageQueryOperator.And, pageSize);
        }

        public Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, int pageSize, string serializedContinuationToken)
        {
            return PagedQueryAsync(columnValues, TableStorageQueryOperator.And, pageSize, serializedContinuationToken);
        }

        #endregion

        #region Paged queries

        public Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, TableStorageQueryOperator op, int pageSize)
        {
            return PagedQueryAsync(columnValues, pageSize, null);
        }

        public async Task<PagedResultSegment<T>> PagedQueryAsync(Dictionary<string, object> columnValues, TableStorageQueryOperator op, int pageSize, string serializedContinuationToken)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: PagedQueryAsync - for table {0} with operator {1} and page size {2}", _tableName, op, pageSize);
            TableQuery<T> query = _tableStorageQueryBuilder.TableQuery<T>(columnValues, op).Take(pageSize);

            TableContinuationToken continuationToken = _tableContinuationTokenSerializer.Deserialize(serializedContinuationToken);

            TableQuerySegment<T> querySegment = await _table.ExecuteQuerySegmentedAsync(query, continuationToken);

            _logger?.Verbose("AsynchronousTableStorageRepository: PagedQueryAsync - for table {0} with operator {1} and page size {2} returned {3}", _tableName, op, pageSize, querySegment.Results.Count);

            return new PagedResultSegment<T>
            {
                ContinuationToken = _tableContinuationTokenSerializer.Serialize(querySegment.ContinuationToken),
                Page = new List<T>(querySegment.Results)
            };
        }

        public async Task<PagedResultSegment<T>> PagedQueryAsync(string filter, int pageSize,
            string serializedContinuationToken)
        {
            _logger?.Verbose("AsynchronousTableStorageRepository: PagedQueryAsync - for table {0} with page size {1}", _tableName, pageSize);

            TableQuery<T> tableQuery = new TableQuery<T>();
            tableQuery.Where(filter);

            TableContinuationToken continuationToken = _tableContinuationTokenSerializer.Deserialize(serializedContinuationToken);

            TableQuerySegment<T> querySegment = await _table.ExecuteQuerySegmentedAsync(tableQuery, continuationToken);

            _logger?.Verbose("AsynchronousTableStorageRepository: PagedQueryAsync - for table {0} with page size {1} returned {2} rows", _tableName, pageSize, querySegment.Results.Count);

            return new PagedResultSegment<T>
            {
                ContinuationToken = _tableContinuationTokenSerializer.Serialize(querySegment.ContinuationToken),
                Page = new List<T>(querySegment.Results)
            };
        }

        #endregion

        internal CloudTable Table => _table;
    }
}
