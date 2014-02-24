using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    public class PagedResultSegment<T> where T : NoSqlEntity
    {
        public string ContinuationToken { get; set; }

        public IEnumerable<T> Page { get; set; }
    }
}
