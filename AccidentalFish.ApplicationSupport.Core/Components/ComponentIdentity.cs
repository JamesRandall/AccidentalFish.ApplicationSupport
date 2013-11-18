using System;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public class ComponentIdentity : IComponentIdentity
    {
        private readonly string _fullyQualifiedName;

        public ComponentIdentity(string fullyQualifiedName)
        {
            _fullyQualifiedName = fullyQualifiedName;
        }

        public ComponentIdentity(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof (ComponentIdentityAttribute), true);
            _fullyQualifiedName = ((ComponentIdentityAttribute) attributes[0]).FullyQualifiedName;
        }

        public string FullyQualifiedName { get { return _fullyQualifiedName; } }

        public override bool Equals(object obj)
        {
            IComponentIdentity other = obj as IComponentIdentity;
            if (other != null)
            {
                return _fullyQualifiedName.Equals(other.FullyQualifiedName);
            }
            return false;
        }

        protected bool Equals(ComponentIdentity other)
        {
            return string.Equals(_fullyQualifiedName, other._fullyQualifiedName);
        }

        public override int GetHashCode()
        {
            return (_fullyQualifiedName != null ? _fullyQualifiedName.GetHashCode() : 0);
        }
    }
}
