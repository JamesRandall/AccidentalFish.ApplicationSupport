using System;
using AccidentalFish.ApplicationSupport.Core.Components;

namespace AccidentalFish.ApplicationSupport.Core.Naming
{
    /// <summary>
    /// Fully qualified name implementation
    /// </summary>
    public class FullyQualifiedNameBase : IFullyQualifiedName
    {
        private readonly string _fullyQualifiedName;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="fullyQualifiedName">Name</param>
        public FullyQualifiedNameBase(string fullyQualifiedName)
        {
            _fullyQualifiedName = fullyQualifiedName;
        }

        /// <summary>
        /// Construct the name from the a type. The type must be decorated with ComponentIdentityAttribute. 
        /// </summary>
        /// <param name="type">The type to pull the name from</param>
        public FullyQualifiedNameBase(Type type)
        {
            object[] attributes = type.GetCustomAttributes(typeof (ComponentIdentityAttribute), true);
            _fullyQualifiedName = ((ComponentIdentityAttribute) attributes[0]).FullyQualifiedName;
        }

        /// <summary>
        /// Returns the fully qualified name
        /// </summary>
        public string FullyQualifiedName => _fullyQualifiedName;

        /// <summary>
        /// The string representation is the fully qualified name
        /// </summary>
        /// <returns>String representation</returns>
        public override string ToString()
        {
            return FullyQualifiedName;
        }

        /// <summary>
        /// Compare this with another object
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>If obj is an IComponentIdentity implementation and the FullyQualifiedNames equate then this returns true, otherwise false.</returns>
        public override bool Equals(object obj)
        {
            IComponentIdentity other = obj as IComponentIdentity;
            if (other != null)
            {
                return _fullyQualifiedName.Equals(other.FullyQualifiedName);
            }
            return false;
        }

        /// <summary>
        /// Compare this iwth another object of this type
        /// </summary>
        /// <param name="other">The object to compare to</param>
        /// <returns>FullyQualifiedNames equate then this returns true, otherwise false</returns>
        protected bool Equals(FullyQualifiedNameBase other)
        {
            return string.Equals(_fullyQualifiedName, other._fullyQualifiedName);
        }

        /// <summary>
        /// Generate a hash code from the FullyQualifiedName string
        /// </summary>
        /// <returns>A hashcode</returns>
        public override int GetHashCode()
        {
            return (_fullyQualifiedName != null ? _fullyQualifiedName.GetHashCode() : 0);
        }
    }
}
