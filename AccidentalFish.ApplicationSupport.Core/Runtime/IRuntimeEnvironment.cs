using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    public interface IRuntimeEnvironment
    {
        string RoleName { get; }

        string RoleIdentifier { get; }
    }
}
