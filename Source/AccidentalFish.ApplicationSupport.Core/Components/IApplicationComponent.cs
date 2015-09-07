namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Interface for a self naming component
    /// </summary>
    public interface IApplicationComponent
    {
        /// <summary>
        /// Identity of the component
        /// </summary>
        IComponentIdentity ComponentIdentity { get; }
    }
}
