using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Logging.Implementation
{
    internal class EntityFrameworkRepositoryLogger : IEntityFrameworkRepositoryLogger
    {
        private readonly ILogger _logger;

        public EntityFrameworkRepositoryLogger(ILogger logger)
        {
            _logger = logger;
        }

        public void Verbose(string message, params object[] additionalData)
        {
            try
            {
                _logger.Verbose(string.Format(message, additionalData));
            }
            catch (Exception)
            {
                _logger.Verbose($"{message}. Log format contained param error.");
            }
        }

        public void Verbose(string message, Exception exception, params object[] additionalData)
        {
            try
            {
                _logger.Verbose(string.Format(message, additionalData), exception);
            }
            catch (Exception)
            {
                _logger.Verbose($"{message}. Log format contained param error.", exception);
            }
        }
    }
}
