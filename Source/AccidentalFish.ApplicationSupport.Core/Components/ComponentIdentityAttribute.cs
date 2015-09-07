using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    /// <summary>
    /// Attribute that can be attached to classes to describe their name
    /// </summary>
    public class ComponentIdentityAttribute : Attribute
    {
        private readonly string _fullyQualifiedName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fullyQualifiedName">Name of the component</param>
        public ComponentIdentityAttribute(string fullyQualifiedName)
        {
            _fullyQualifiedName = fullyQualifiedName;
        }

        /// <summary>
        /// Name of the component
        /// </summary>
        public string FullyQualifiedName => _fullyQualifiedName;
    }
}
