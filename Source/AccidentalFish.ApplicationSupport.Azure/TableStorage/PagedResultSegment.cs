using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.TableStorage
{
    public class PagedResultSegment<T> where T : ITableEntity
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<T> Page { get; set; }
    }
}
