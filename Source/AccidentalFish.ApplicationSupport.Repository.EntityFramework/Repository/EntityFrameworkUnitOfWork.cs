using System;
using System.Data.Entity;
using System.Diagnostics;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal sealed class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly DbContext _context;
        private readonly ILogger _logger;

        private EntityFrameworkUnitOfWork(ILogger logger)
        {
            _logger = logger;
        }

        public EntityFrameworkUnitOfWork(
            IConfiguration configuration,
            IDbContextFactory dbContextFactory,
            IDbConfiguration dbConfiguration,
            ILogger logger) : this(logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (dbContextFactory == null) throw new ArgumentNullException(nameof(dbContextFactory));
            if (dbConfiguration == null) throw new ArgumentNullException(nameof(dbConfiguration));
            
            _context = dbContextFactory.CreateContext(configuration.SqlConnectionString);
            _dbConfiguration = dbConfiguration;

            _logger?.Verbose("EntityFrameworkUnitOfWork: Created unit of work for {0}", _context.Database.Connection.ConnectionString);
        }

        public EntityFrameworkUnitOfWork(
            Type contextType,
            string connectionString,
            IDbConfiguration dbConfiguration,
            ILogger logger) : this(logger)
        {
            if (contextType == null) throw new ArgumentNullException(nameof(contextType));
            if (dbConfiguration == null) throw new ArgumentNullException(nameof(dbConfiguration));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));

            _context = (DbContext) Activator.CreateInstance(contextType, connectionString);
            _dbConfiguration = dbConfiguration;

            _logger?.Verbose("EntityFrameworkUnitOfWork: Created unit of work for {0}", _context.Database.Connection.ConnectionString);
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            var result = new EntityFrameworkRepository<T>(_context);
            _logger?.Verbose("EntityFrameworkUnitOfWork: created repository for type {0}", typeof(T).FullName);
            return result;
        }

        public void Save()
        {
            Stopwatch sw = Stopwatch.StartNew();
            _context.SaveChanges();
            sw.Stop();
            _logger?.Verbose("EntityFrameworkUnitOfWork: Committed changes in {0}ms", sw.ElapsedMilliseconds);
        }

        public void Execute(Action action)
        {
            _dbConfiguration.ExecutionStrategy.Execute(action);
        }

        public T2 Execute<T2>(Func<T2> func)
        {
            T2 result = default(T2);

            _dbConfiguration.ExecutionStrategy.Execute(() => result = func());

            return result;
        }

        public bool SuspendExecutionPolicy
        { 
            get { return _dbConfiguration.SuspendExecutionStrategy; }
            set
            {
                _logger?.Verbose(value
                    ? "EntityFrameworkUnitOfWork: Suspending execution policy"
                    : "EntityFrameworkUnitOfWork: Resuming execution policy");
                _dbConfiguration.SuspendExecutionStrategy = value;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        internal DbContext Context => _context;
    }
}