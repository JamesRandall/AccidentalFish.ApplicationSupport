using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    internal class SqlExecuterAsync : ISqlExecuterAsync
    {
        private readonly DbContext _context;

        public SqlExecuterAsync(DbContext context)
        {
            _context = context;
        }

        public async Task ExecuteCommand(string command, params object[] parameters)
        {
            await _context.Database.ExecuteSqlCommandAsync(command, parameters);
        }

        public async Task<IEnumerable<T>> ExecuteQuery<T>(string command, params object[] parameters)
        {
            var query = _context.Database.SqlQuery<T>(command, parameters);
            return await query.ToListAsync();
        }
    }
}
