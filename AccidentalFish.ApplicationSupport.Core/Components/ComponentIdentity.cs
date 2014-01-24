using System;
using AccidentalFish.ApplicationSupport.Core.Naming;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public class ComponentIdentity : FullyQualifiedNameBase, IComponentIdentity
    {
        public ComponentIdentity(string fullyQualifiedName) : base(fullyQualifiedName)
        {
        }

        public ComponentIdentity(Type type) : base(type)
        {
        }
    }
}
