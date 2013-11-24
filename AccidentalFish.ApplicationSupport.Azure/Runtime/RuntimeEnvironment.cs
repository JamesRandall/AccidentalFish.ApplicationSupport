using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Runtime;
using Microsoft.WindowsAzure.ServiceRuntime;

namespace AccidentalFish.ApplicationSupport.Azure.Runtime
{
    internal class RuntimeEnvironment : IRuntimeEnvironment
    {
        public string RoleName
        {
            get
            {
                return IsEmulated ? "local" : RoleEnvironment.CurrentRoleInstance.Role.Name;
            }
        }

        public string RoleIdentifier
        {
            get
            {
                return IsEmulated ? "local" : RoleEnvironment.CurrentRoleInstance.Id;
            }
        }

        private bool IsEmulated
        {
            get
            {
                try
                {
                    return RoleEnvironment.IsEmulated;
                }
                catch (Exception)
                {
                    // unit tests will raise an exception as there is no role environment at all
                    return false;
                }
            }
        }
    }
}
