using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Autofac.Builder;

namespace AccidentalFish.ApplicationSupport.Autofac
{
    public class ExplicitAutofacApplicationFrameworkDependencyResolver : AbstractAutofacApplicationFrameworkDependencyResolver
    {
        private IContainer _container;

        public ExplicitAutofacApplicationFrameworkDependencyResolver(ContainerBuilder builder) : base(builder)
        {
            
        }

        public override bool IsRegistered<T>()
        {
            if (_container != null)
            {
                return _container.IsRegistered<T>();
            }
            return RegisteredTypes.Contains(typeof(T));
        }

        public override T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public override T Resolve<T>(string name)
        {
            return _container.ResolveNamed<T>(name);
        }

        public IContainer Build(ContainerBuildOptions options = ContainerBuildOptions.None)
        {
            _container = Builder.Build(options);
            return _container;
        }

        public void SetContainer(IContainer container)
        {
            _container = container;
        }

        public bool IsBuilt => _container != null;
    }
}
