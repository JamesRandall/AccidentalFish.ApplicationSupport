using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Repository
{
    public interface IUnitOfWorkFactoryProvider
    {
        IUnitOfWorkFactory Create(string contextType, string connectionString);
    }
}
