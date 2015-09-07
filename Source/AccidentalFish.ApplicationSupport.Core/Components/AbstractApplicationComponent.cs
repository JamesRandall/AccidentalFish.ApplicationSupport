using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Abstract class for describing a component. Implementations of this class should be annotated with the
    /// ComponentIdentityAttribute.
    /// </summary>
    public abstract class AbstractApplicationComponent : IApplicationComponent
    {
        private readonly IComponentIdentity _componentIdentity;

        /// <summary>
        /// Constructor
        /// </summary>
        protected AbstractApplicationComponent()
        {
            try
            {
                _componentIdentity = new ComponentIdentity(GetType());
            }
            catch (IndexOutOfRangeException)
            {
                throw new MissingComponentIdentityException();
            }
            
        }

        /// <summary>
        /// The name of the component
        /// </summary>
        public IComponentIdentity ComponentIdentity => _componentIdentity;
    }
}
