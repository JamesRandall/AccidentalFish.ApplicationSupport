using System;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Provides the name for a component
    /// </summary>
    public class ComponentIdentity : FullyQualifiedNameBase, IComponentIdentity
    {
        /// <summary>
        /// Constructor that builds the name from a string
        /// </summary>
        /// <param name="fullyQualifiedName">Name of the component</param>
        public ComponentIdentity(string fullyQualifiedName) : base(fullyQualifiedName)
        {
        }

        /// <summary>
        /// Constructor that builds the name from a type annotated with the ComponentIdentityAttribetu
        /// </summary>
        /// <param name="type"></param>
        public ComponentIdentity(Type type) : base(type)
        {
        }
    }
}
