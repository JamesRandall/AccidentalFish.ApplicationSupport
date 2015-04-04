using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Azure.TableStorage;
using AccidentalFish.ApplicationSupport.Core.Components;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.Components
{
    public interface IAzureApplicationResourceFactory : IApplicationResourceFactory
    {
        IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(IComponentIdentity componentIdentity) where T : ITableEntity, new();
        IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity) where T : ITableEntity, new();
        IAsynchronousTableStorageRepository<T> GetTableStorageRepository<T>(string tablename, IComponentIdentity componentIdentity, bool lazyCreateTable) where T : ITableEntity, new();
    }
}
