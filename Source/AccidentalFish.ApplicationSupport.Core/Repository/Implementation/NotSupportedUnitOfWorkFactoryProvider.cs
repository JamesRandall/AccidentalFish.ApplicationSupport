using System;

namespace AccidentalFish.ApplicationSupport.Core.Repository.Implementation
{
    /// <summary>
    /// Simply throws an exception if called. The desired repository pattern provider
    /// should be registered after core to give database access.
    /// </summary>
    internal class NotSupportedUnitOfWorkFactoryProvider : IUnitOfWorkFactoryProvider
    {
        public IUnitOfWorkFactory Create(string contextType, string connectionString)
        {
            throw new NotSupportedException("Register dependencies in a bootstrapper of a repository pattern provider after the Core library to provide database support");
        }
    }
}
