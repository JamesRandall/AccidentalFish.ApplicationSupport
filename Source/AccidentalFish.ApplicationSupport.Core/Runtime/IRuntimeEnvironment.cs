namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    /// <summary>
    /// Defines the current run time environment. When running in Azure this is the role name and identifier.
    /// Otherwise the role identifier is the server name and the role name is "Default"
    /// </summary>
    public interface IRuntimeEnvironment
    {
        /// <summary>
        /// Role name
        /// </summary>
        string RoleName { get; }

        /// <summary>
        /// Role identifier
        /// </summary>
        string RoleIdentifier { get; }
    }
}
