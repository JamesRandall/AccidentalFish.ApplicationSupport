using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal class EntityFrameworkUnitOfWorkAsync : IUnitOfWorkAsync
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly DbContext _context;

        public EntityFrameworkUnitOfWorkAsync(IConfiguration configuration, IDbContextFactory dbContextFactory, IDbConfiguration dbConfiguration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (dbContextFactory == null) throw new ArgumentNullException("dbContextFactory");
            if (dbConfiguration == null) throw new ArgumentNullException("dbConfiguration");


            _context = dbContextFactory.CreateContext(configuration.SqlConnectionString);
            _dbConfiguration = dbConfiguration;
        }

        public EntityFrameworkUnitOfWorkAsync(Type contextType, string connectionString, IDbConfiguration dbConfiguration)
        {
            if (contextType == null) throw new ArgumentNullException("contextType");
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("connectionString");
            if (dbConfiguration == null) throw new ArgumentNullException("dbConfiguration");

            _context = (DbContext) Activator.CreateInstance(contextType, connectionString);
            _dbConfiguration = dbConfiguration;
        }

        public Task<IRepositoryAsync<T>> GetRepositoryAsync<T>() where T : class
        {
            return Task.FromResult<IRepositoryAsync<T>>(new EntityFrameworkRepositoryAsync<T>(_context));
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
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

        public Task<bool> OptimisticRepositoryWinsUpdate(Action update)
        {
            return OptimisticRepositoryWinsUpdate(update, int.MaxValue);
        }

        public async Task<bool> OptimisticRepositoryWinsUpdate(Action update, int maxRetries)
        {
            bool saveFailed;
            int retries = 0;
            do
            {
                saveFailed = false;
                try
                {
                    update();
                    await SaveAsync();
                }
                catch (DbUpdateConcurrencyException concurrencyException)
                {
                    retries++;
                    foreach(DbEntityEntry entity in concurrencyException.Entries) entity.Reload();
                    saveFailed = true;
                }
            } while (saveFailed && retries < maxRetries);
            return !saveFailed;
        }

        public bool SuspendExecutionPolicy
        {
            get { return _dbConfiguration.SuspendExecutionStrategy; }
            set { _dbConfiguration.SuspendExecutionStrategy = value; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        internal DbContext Context => _context;
    }
}
