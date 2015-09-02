namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Interface for a self naming component
    /// </summary>
    public interface IApplicationComponent
    {
        IComponentIdentity ComponentIdentity { get; }
    }
}
