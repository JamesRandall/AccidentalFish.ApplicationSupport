using System;
using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    internal class TableStorageQueryBuilder : ITableStorageQueryBuilder
    {
        public TableQuery<T> TableQuery<T>(Dictionary<string, object> columnValues) where T : NoSqlEntity
        {
            List<string> tableQueries = new List<string>();
            foreach (KeyValuePair<string, object> kvp in columnValues)
            {
                if (kvp.Value is string)
                {
                    tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterCondition(kvp.Key, QueryComparisons.Equal, (string)kvp.Value));
                }
                else if (kvp.Value is Guid)
                {
                    tableQueries.Add(Microsoft.WindowsAzure.Storage.Table.TableQuery.GenerateFilterConditionForGuid(kvp.Key, QueryComparisons.Equal, (Guid)kvp.Value));
                }
            }
            string queryString = tableQueries[0];
            for (int index = 1; index < tableQueries.Count; index++)
            {
                string subQueryString = tableQueries[index];
                queryString = Microsoft.WindowsAzure.Storage.Table.TableQuery.CombineFilters(queryString, TableOperators.And, subQueryString);
            }

            TableQuery<T> query = new TableQuery<T>().Where(queryString);
            return query;
        }
    }
}
