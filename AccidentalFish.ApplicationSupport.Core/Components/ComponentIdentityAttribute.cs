using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public class ComponentIdentityAttribute : Attribute
    {
        private readonly string _fullyQualifiedName;

        public ComponentIdentityAttribute(string fullyQualifiedName)
        {
            _fullyQualifiedName = fullyQualifiedName;
        }

        public string FullyQualifiedName { get { return _fullyQualifiedName; }}
    }
}
