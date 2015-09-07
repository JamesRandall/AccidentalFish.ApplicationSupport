using System.Collections.Generic;

namespace AccidentalFish.ApplicationSupport.Repository.EntityFramework.Repository
{
    public interface ISqlExecuter
    {
        void ExecuteCommand(string command, params object[] parameters);
        IEnumerable<T> ExecuteQuery<T>(string command, params object[] parameters);
    }
}
