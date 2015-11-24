using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.Storage.Table;

namespace TableStorage
{
    public class SampleEntity : TableEntity
    {
        public string Name { get; set; }
    }

    class Program
    {
        const string StorageAccountConnectionString = "UseDevelopmentStorage=true;";
        const string TableName = "MyTable";

        static void Main(string[] args)
        {
            SimpleSaveAndRetrieve().Wait();
            Task<string> task = LargeBatchInsert();
            task.Wait();
            GetAllInPartition(task.Result).Wait();
        }

        private static async Task SimpleSaveAndRetrieve()
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            resolver
                .UseCore(defaultTraceLoggerMinimumLogLevel:LogLevelEnum.Verbose)
                .UseAzure();

            var resourceManager = container.Resolve<IAzureResourceManager>();
            var tableStorageRepositoryFactory = container.Resolve<ITableStorageRepositoryFactory>();
            var table = tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<SampleEntity>(StorageAccountConnectionString, TableName);
            await resourceManager.CreateIfNotExistsAsync(table);
            string partitionKey = Guid.NewGuid().ToString();
            string rowKey = Guid.NewGuid().ToString();
            await table.InsertAsync(new SampleEntity
            {
                Name = "Someone New",
                PartitionKey = partitionKey,
                RowKey = rowKey
            });

            SampleEntity retrievedEntity = await table.GetAsync(partitionKey, rowKey);
            Console.WriteLine(retrievedEntity.Name);
        }

        private static async Task<string> LargeBatchInsert()
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            resolver
                .UseCore(defaultTraceLoggerMinimumLogLevel: LogLevelEnum.Verbose)
                .UseAzure();

            var resourceManager = container.Resolve<IAzureResourceManager>();
            var tableStorageRepositoryFactory = container.Resolve<ITableStorageRepositoryFactory>();
            var table = tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<SampleEntity>(StorageAccountConnectionString, TableName);
            await resourceManager.CreateIfNotExistsAsync(table);

            string partitionKey = Guid.NewGuid().ToString();
            List<SampleEntity> entities = new List<SampleEntity>();
            for (int item = 0; item < 725; item++)
            {
                entities.Add(new SampleEntity
                {
                    Name = $"Name {item}",
                    PartitionKey = partitionKey,
                    RowKey = Guid.NewGuid().ToString()
                });
            }

            await table.InsertBatchAsync(entities);
            Console.WriteLine($"Inserted 725 items into partition {partitionKey}");
            return partitionKey;            
        }

        private static async Task GetAllInPartition(string partitionKey)
        {
            IUnityContainer container = new UnityContainer();
            UnityApplicationFrameworkDependencyResolver resolver = new UnityApplicationFrameworkDependencyResolver(container);

            resolver
                .UseCore(defaultTraceLoggerMinimumLogLevel: LogLevelEnum.Verbose)
                .UseAzure();

            var resourceManager = container.Resolve<IAzureResourceManager>();
            var tableStorageRepositoryFactory = container.Resolve<ITableStorageRepositoryFactory>();
            var table = tableStorageRepositoryFactory.CreateAsynchronousNoSqlRepository<SampleEntity>(StorageAccountConnectionString, TableName);
            await resourceManager.CreateIfNotExistsAsync(table);

            IEnumerable<SampleEntity> entities = await table.GetAsync(partitionKey);
            Console.WriteLine($"Retrieved {entities.Count()} items");
        }
    }
}
