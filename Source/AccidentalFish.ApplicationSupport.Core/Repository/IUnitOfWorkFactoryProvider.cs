using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    /// <summary>
    /// Provides unit of work factories that are configured with a given database context and connection string
    /// </summary>
    public interface IUnitOfWorkFactoryProvider
    {
        /// <summary>
        /// Creates a unit of work factory
        /// </summary>
        /// <param name="contextType">The database context type</param>
        /// <param name="connectionString">The database connection string</param>
        /// <returns>A unit of work factory</returns>
        IUnitOfWorkFactory Create(string contextType, string connectionString);
    }
}
