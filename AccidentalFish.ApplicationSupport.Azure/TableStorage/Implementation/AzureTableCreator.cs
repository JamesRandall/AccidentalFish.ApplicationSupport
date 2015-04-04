using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Policies;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage.Implementation
{
    internal class AzureTableCreator : IResourceCreator
    {
        private readonly CloudTable _table;

        public AzureTableCreator(CloudTable table)
        {
            _table = table;
        }

        public Task CreateIfNotExists()
        {
            return _table.CreateIfNotExistsAsync();
        }
    }
}
