using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Logging;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Logging;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal class EntityFrameworkUnitOfWorkAsync : IUnitOfWorkAsync
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly DbContext _context;
        private readonly IEntityFrameworkRepositoryLogger _logger;

        public EntityFrameworkUnitOfWorkAsync(
            IConfiguration configuration,
            IDbContextFactory dbContextFactory,
            IDbConfiguration dbConfiguration,
            IEntityFrameworkRepositoryLogger logger) : this(logger)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (dbContextFactory == null) throw new ArgumentNullException(nameof(dbContextFactory));
            if (dbConfiguration == null) throw new ArgumentNullException(nameof(dbConfiguration));

            _context = dbContextFactory.CreateContext(configuration.SqlConnectionString);
            _dbConfiguration = dbConfiguration;

            _logger?.Verbose("EntityFrameworkUnitOfWorkAsync: Created unit of work for {0}", _context.Database.Connection.ConnectionString);
        }

        public EntityFrameworkUnitOfWorkAsync(
            Type contextType,
            string connectionString,
            IDbConfiguration dbConfiguration,
            IEntityFrameworkRepositoryLogger logger) : this(logger)
        {
            if (contextType == null) throw new ArgumentNullException(nameof(contextType));
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(nameof(connectionString));
            if (dbConfiguration == null) throw new ArgumentNullException(nameof(dbConfiguration));

            _context = (DbContext) Activator.CreateInstance(contextType, connectionString);
            _dbConfiguration = dbConfiguration;

            _logger?.Verbose("EntityFrameworkUnitOfWorkAsync: Created unit of work for {0}", _context.Database.Connection.ConnectionString);
        }

        private EntityFrameworkUnitOfWorkAsync(IEntityFrameworkRepositoryLogger logger)
        {
            _logger = logger;
        }

        public IRepositoryAsync<T> GetRepository<T>() where T : class
        {
            return new EntityFrameworkRepositoryAsync<T>(_context);
        }

        public async Task<int> SaveAsync()
        {
            Stopwatch sw = Stopwatch.StartNew();
            int result = await _context.SaveChangesAsync();
            sw.Stop();
            _logger?.Verbose("EntityFrameworkUnitOfWorkAsync: Committed changes in {0}ms", sw.ElapsedMilliseconds);
            return result;
        }

        public Task ExecuteAsync(Func<Task> func)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            return _dbConfiguration.ExecutionStrategy.ExecuteAsync(func, tokenSource.Token);
        }

        public Task ExecuteAsync(Func<Task> func, CancellationToken token)
        {
            return _dbConfiguration.ExecutionStrategy.ExecuteAsync(func, token);
        }

        public Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            return _dbConfiguration.ExecutionStrategy.ExecuteAsync(func, tokenSource.Token);
        }

        public Task<T2> ExecuteAsync<T2>(Func<Task<T2>> func, CancellationToken token)
        {
            return _dbConfiguration.ExecutionStrategy.ExecuteAsync(func, token);
        }

        public Task OptimisticRepositoryWinsUpdateAsync(Action update)
        {
            return OptimisticRepositoryWinsUpdateAsync(update, int.MaxValue);
        }

        public async Task<bool> OptimisticRepositoryWinsUpdateAsync(Action update, int maxRetries)
        {
            bool saveFailed;
            int retries = 0;
            do
            {
                saveFailed = false;
                try
                {
                    _logger?.Verbose("EntityFrameworkUnitOfWorkAsync: OptimisticRepositoryWinsUpdateAsync - attempting update retry {0}", retries);
                    update();
                    await SaveAsync();
                    _logger?.Verbose("EntityFrameworkUnitOfWorkAsync: OptimisticRepositoryWinsUpdateAsync - update succeeded on retry {0}", retries);
                }
                catch (DbUpdateConcurrencyException concurrencyException)
                {
                    _logger?.Verbose("EntityFrameworkUnitOfWorkAsync: OptimisticRepositoryWinsUpdateAsync - update failed on retry {0}", retries);
                    retries++;
                    foreach(DbEntityEntry entity in concurrencyException.Entries) entity.Reload();
                    saveFailed = true;
                }
            } while (saveFailed && retries < maxRetries);
            if (saveFailed)
            {
                _logger?.Verbose("EntityFrameworkUnitOfWorkAsync: OptimisticRepositoryWinsUpdateAsync - updated failed after max retried of {0}", maxRetries);
            }
            return !saveFailed;
        }

        public bool SuspendExecutionPolicy
        {
            get { return _dbConfiguration.SuspendExecutionStrategy; }
            set
            {
                _logger?.Verbose(value
                    ? "EntityFrameworkUnitOfWorkAsync: Suspending execution policy"
                    : "EntityFrameworkUnitOfWorkAsync: Resuming execution policy");
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
