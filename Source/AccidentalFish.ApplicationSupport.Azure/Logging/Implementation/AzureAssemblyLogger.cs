using System;
using AccidentalFish.ApplicationSupport.Core.Logging;

namespace AccidentalFish.ApplicationSupport.Azure.Logging.Implementation
{
    internal class AzureAssemblyLogger : IAzureAssemblyLogger
    {
        private readonly ILogger _logger;

        public AzureAssemblyLogger(ILogger logger)
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
