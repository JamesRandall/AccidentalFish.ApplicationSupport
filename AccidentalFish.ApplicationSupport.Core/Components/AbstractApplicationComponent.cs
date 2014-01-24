using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Core.Components
{
    public abstract class AbstractApplicationComponent : IApplicationComponent
    {
        private readonly IComponentIdentity _componentIdentity;

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

        public IComponentIdentity ComponentIdentity { get { return _componentIdentity; }}
    }
}
