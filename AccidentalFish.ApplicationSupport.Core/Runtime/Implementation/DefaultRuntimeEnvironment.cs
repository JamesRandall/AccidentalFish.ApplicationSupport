using System;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    internal class DefaultRuntimeEnvironment : IRuntimeEnvironment
    {
        public string RoleName
        {
            get { return "Default"; }
        }

        public string RoleIdentifier
        {
            get { return Environment.MachineName; }
        }
    }
}
