using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    public interface ISqlExecuterAsync
    {
        Task ExecuteCommand(string command, params object[] parameters);
        Task<IEnumerable<T>> ExecuteQuery<T>(string command, params object[] parameters);
    }
}
