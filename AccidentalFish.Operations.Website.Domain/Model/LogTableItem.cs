using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage.Table;

namespace AccidentalFish.Operations.Website.Domain.Model
{
    public class LogTableItem : TableEntity
    {
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
