using System;
using System.Collections.Generic;
using System.Linq;
using AccidentalFish.ApplicationSupport.Core.NoSql;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Azure.NoSql
{
    internal class TableStorageQueryBuilder : ITableStorageQueryBuilder
    {
        public TableQuery<T> TableQuery<T>(Dictionary<string, object> columnValues, NoSqlQueryOperator op) where T : NoSqlEntity
        {
            TableQuery<T> query = new TableQuery<T>();
            if (columnValues != null && columnValues.Any())
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
                string tableOp = op == NoSqlQueryOperator.And ? TableOperators.And : TableOperators.Or;
                for (int index = 1; index < tableQueries.Count; index++)
                {
                    string subQueryString = tableQueries[index];
                    queryString = Microsoft.WindowsAzure.Storage.Table.TableQuery.CombineFilters(queryString, tableOp, subQueryString);
                }
                query = query.Where(queryString);
            }
            
            return query;
        }
    }
}
