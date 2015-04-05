using System;
using System.Data.Entity;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Repository;
using AccidentalFish.ApplicationSupport.Repository.EntityFramework.Policies;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal sealed class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        private readonly IDbConfiguration _dbConfiguration;
        private readonly DbContext _context;

        public EntityFrameworkUnitOfWork(IConfiguration configuration, IDbContextFactory dbContextFactory, IDbConfiguration dbConfiguration)
        {
            if (configuration == null) throw new ArgumentNullException("configuration");
            if (dbContextFactory == null) throw new ArgumentNullException("dbContextFactory");
            if (dbConfiguration == null) throw new ArgumentNullException("dbConfiguration");
            
            _context = dbContextFactory.CreateContext(configuration.SqlConnectionString);
            _dbConfiguration = dbConfiguration;
        }

        public EntityFrameworkUnitOfWork(Type contextType, string connectionString, IDbConfiguration dbConfiguration)
        {
            if (contextType == null) throw new ArgumentNullException("contextType");
            if (dbConfiguration == null) throw new ArgumentNullException("dbConfiguration");
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException("connectionString");

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