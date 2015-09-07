using System;

namespace AccidentalFish.ApplicationSupport.Core.Runtime.Implementation
{
    internal class DefaultRuntimeEnvironment : IRuntimeEnvironment
    {
        public string RoleName => "Default";

        public string RoleIdentifier => Environment.MachineName;
    }
}
