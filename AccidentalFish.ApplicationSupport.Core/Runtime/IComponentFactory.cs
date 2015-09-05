using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Runtime
{
    /// <summary>
    /// Creates hostable components
    /// </summary>
    internal interface IComponentFactory
    {
        /// <summary>
        /// Create a hostable component with the given identity
        /// </summary>
        /// <param name="componentIdentity">The identity of the component</param>
        /// <returns>A hosted component</returns>
        IHostableComponent Create(IComponentIdentity componentIdentity);
    }
}
