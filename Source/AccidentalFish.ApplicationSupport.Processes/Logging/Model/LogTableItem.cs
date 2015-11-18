using System;
using System.Linq;
using AccidentalFish.ApplicationSupport.Core.Logging;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.ApplicationSupport.Logging.QueueLogger.Model
{
    internal class LogTableItem : TableEntity
    {
        public void SetPartitionAndRowKeyForLogByDateDesc()
        {
            PartitionKey = $"{LoggedAt.Year:D4}{LoggedAt.Month:D2}{LoggedAt.Day:D2}";
            RowKey = $"{DateTimeOffset.MaxValue.Ticks - LoggedAt.Ticks:D19}{Guid.NewGuid()}";
        }

        public void SetPartitionAndRowKeyForLogByDate()
        {
            PartitionKey = $"{LoggedAt.Year:D4}{LoggedAt.Month:D2}{LoggedAt.Day:D2}";
            RowKey = $"{LoggedAt.Ticks:D19}{Guid.NewGuid()}";
        }

        public void SetPartitionAndRowKeyForLogBySeverity()
        {
            PartitionKey = ((int)(Enum.GetValues(typeof(LogLevelEnum)).Cast<LogLevelEnum>().Max() - Level)).ToString("D2");
            RowKey = $"{DateTimeOffset.MaxValue.Ticks - LoggedAt.Ticks:D19}{Guid.NewGuid()}";
        }

        public void SetPartitionAndRowKeyForLogBySource()
        {
            PartitionKey = Source;
            RowKey =
                $"{Enum.GetValues(typeof (LogLevelEnum)).Cast<LogLevelEnum>().Max() - Level}_{DateTimeOffset.MaxValue.Ticks - LoggedAt.Ticks:D19}{Guid.NewGuid()}";
        }

        public void SetPartitionAndRowKeyForLogByCorrelationId()
        {
            PartitionKey = CorrelationId;
            RowKey = $"{LoggedAt.Ticks:D19}{Guid.NewGuid()}";
        }

        public string Source { get; set; }

        public string Message { get; set; }

        public string ExceptionName { get; set; }

        public string StackTrace { get; set; }

        public string InnerExceptionName { get; set; }

        public string RoleIdentifier { get; set; }

        public string RoleName { get; set; }

        public int Level { get; set; }

        public DateTimeOffset LoggedAt { get; set; }

        public string CorrelationId { get; set; }
    }
}
