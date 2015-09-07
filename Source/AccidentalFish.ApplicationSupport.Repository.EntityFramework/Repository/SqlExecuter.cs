using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal class SqlExecuter : ISqlExecuter
    {
        private readonly DbContext _context;

        public SqlExecuter(DbContext context)
        {
            _context = context;
        }

        public void ExecuteCommand(string command, params object[] parameters)
        {
            _context.Database.ExecuteSqlCommand(command, parameters);
        }

        public IEnumerable<T> ExecuteQuery<T>(string command, params object[] parameters)
        {
            var query = _context.Database.SqlQuery<T>(command, parameters);
            return query.ToList();
        }
    }
}
