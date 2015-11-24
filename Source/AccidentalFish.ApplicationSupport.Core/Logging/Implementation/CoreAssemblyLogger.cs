using System;

namespace AccidentalFish.ApplicationSupport.Core.Logging.Implementation
{
    internal class CoreAssemblyLogger : ICoreAssemblyLogger
    {
        private readonly ILogger _logger;

        public CoreAssemblyLogger(ILogger logger)
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
