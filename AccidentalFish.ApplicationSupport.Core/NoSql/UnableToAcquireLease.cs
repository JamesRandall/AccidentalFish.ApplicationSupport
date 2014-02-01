using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.NoSql
{
    public class UnableToAcquireLease : Exception
    {
        public UnableToAcquireLease(string message, Exception innerException)
            : base(message, innerException)
        {
            
        }
    }
}
