using System;
using System.Data.Entity;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Policies;
using CuttingEdge.Conditions;

namespace AccidentalFish.ApplicationSupport.Core.Repository.Implementaton
{
    internal sealed class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly DbContext _context;

        public EntityFrameworkUnitOfWork(IConfiguration configuration, IDbContextFactory dbContextFactory, IDbConfiguration dbConfiguration)
        {
            Condition.Requires(configuration).IsNotNull();
            Condition.Requires(dbContextFactory).IsNotNull();
            Condition.Requires(dbConfiguration).IsNotNull();
            
            _context = dbContextFactory.CreateContext(configuration.SqlConnectionString);
            _dbConfiguration = dbConfiguration;
        }

        public EntityFrameworkUnitOfWork(Type contextType, string connectionString, IDbConfiguration dbConfiguration)
        {            
            Condition.Requires(contextType).IsNotNull();
            Condition.Requires(connectionString).IsNotNullOrWhiteSpace();
            Condition.Requires(dbConfiguration).IsNotNull();

            _context = (DbContext) Activator.CreateInstance(contextType, connectionString);
            _dbConfiguration = dbConfiguration;
        }

        public IRepository<T> GetRepository<T>() where T : class
        {
            return new EntityFrameworkRepository<T>(_context);
        }

        public void Save()
        {
            _context.SaveChanges();            
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
            set { _dbConfiguration.SuspendExecutionStrategy = value; }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}