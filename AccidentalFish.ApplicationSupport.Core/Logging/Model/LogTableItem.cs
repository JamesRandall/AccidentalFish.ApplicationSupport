using System;
using System.Linq;
using AccidentalFish.ApplicationSupport.Core.NoSql;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Model
{
    public class LogTableItem : NoSqlEntity
    {
        public void SetPartitionAndRowKeyForLogByDate()
        {
            PartitionKey = String.Format("{0:D4}{1:D2}{2:D2}", LoggedAt.Year, LoggedAt.Month, LoggedAt.Day);
            RowKey = String.Format("{0:D19}{1}", DateTimeOffset.MaxValue.Ticks - LoggedAt.Ticks, Guid.NewGuid());
        }

        public void SetPartitionAndRowKeyForLogBySeverity()
        {
            PartitionKey = ((int)(Enum.GetValues(typeof(LogLevelEnum)).Cast<LogLevelEnum>().Max() - Level)).ToString("D2");
            RowKey = String.Format("{0:D19}{1}", DateTimeOffset.MaxValue.Ticks - LoggedAt.Ticks, Guid.NewGuid());
        }

        public void SetPartitionAndRowKeyForLogBySource()
        {
            PartitionKey = Source;
            RowKey = String.Format("{0}_{1:D19}{2}",
                Enum.GetValues(typeof(LogLevelEnum)).Cast<LogLevelEnum>().Max() - Level,
                DateTimeOffset.MaxValue.Ticks - LoggedAt.Ticks, Guid.NewGuid());
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
    }
}
