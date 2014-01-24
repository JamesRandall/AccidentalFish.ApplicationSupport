using System;
using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Naming
{
    public class FullyQualifiedNameBase : IFullyQualifiedName
    {
        private readonly string _fullyQualifiedName;

        public FullyQualifiedNameBase(string fullyQualifiedName)
        {
            _fullyQualifiedName = fullyQualifiedName;
        }

        public FullyQualifiedNameBase(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof (ComponentIdentityAttribute), true);
            _fullyQualifiedName = ((ComponentIdentityAttribute) attributes[0]).FullyQualifiedName;
        }

        public string FullyQualifiedName { get { return _fullyQualifiedName; } }

        public override string ToString()
        {
            return FullyQualifiedName;
        }

        public override bool Equals(object obj)
        {
            IComponentIdentity other = obj as IComponentIdentity;
            if (other != null)
            {
                return _fullyQualifiedName.Equals(other.FullyQualifiedName);
            }
            return false;
        }

        protected bool Equals(FullyQualifiedNameBase other)
        {
            return string.Equals(_fullyQualifiedName, other._fullyQualifiedName);
        }

        public override int GetHashCode()
        {
            return (_fullyQualifiedName != null ? _fullyQualifiedName.GetHashCode() : 0);
        }
    }
}
