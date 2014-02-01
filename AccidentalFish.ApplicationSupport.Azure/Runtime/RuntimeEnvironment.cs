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
                try
                {
                    if (RoleEnvironment.IsAvailable)
                    {
                        return RoleEnvironment.CurrentRoleInstance.Role.Name;
                    }
                    return "local";
                }
                catch (Exception)
                {
                    return "local";
                }
            }
        }

        public string RoleIdentifier
        {
            get
            {
                try
                {
                    if (RoleEnvironment.IsAvailable)
                    {
                        return RoleEnvironment.CurrentRoleInstance.Id;
                    }
                    return "local";
                }
                catch (Exception)
                {
                    return "local";
                }
            }
        }
    }
}
