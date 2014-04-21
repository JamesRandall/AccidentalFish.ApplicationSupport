using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Policies;
using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Core.Repository.Implementaton
{
    internal class EntityFrameworkUnitOfWorkAsync : IUnitOfWorkAsync
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly DbContext _context;

        public EntityFrameworkUnitOfWorkAsync(IConfiguration configuration, IDbContextFactory dbContextFactory, IDbConfiguration dbConfiguration)
        {
            Condition.Requires(configuration).IsNotNull();
            Condition.Requires(dbContextFactory).IsNotNull();
            Condition.Requires(dbConfiguration).IsNotNull();
            
            _context = dbContextFactory.CreateContext(configuration.SqlConnectionString);
            _dbConfiguration = dbConfiguration;
        }

        public EntityFrameworkUnitOfWorkAsync(Type contextType, string connectionString, IDbConfiguration dbConfiguration)
        {            
            Condition.Requires(contextType).IsNotNull();
            Condition.Requires(connectionString).IsNotNullOrWhiteSpace();
            Condition.Requires(dbConfiguration).IsNotNull();

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

        public bool SuspendExecutionPolicy
        {
            get { return _dbConfiguration.SuspendExecutionStrategy; }
            set { _dbConfiguration.SuspendExecutionStrategy = value; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
