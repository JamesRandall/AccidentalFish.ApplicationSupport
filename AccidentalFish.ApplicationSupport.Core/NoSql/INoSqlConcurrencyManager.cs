using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    public interface INoSqlConcurrencyManager
    {
        Task<T> Update<T>(IAsynchronousNoSqlRepository<T> table, string paritionKey, Action<T> update) where T : NoSqlEntity, new();

        Task<T> Update<T>(IAsynchronousNoSqlRepository<T> table, string paritionKey, string rowKey, Action<T> update) where T : NoSqlEntity, new();
    }
}
