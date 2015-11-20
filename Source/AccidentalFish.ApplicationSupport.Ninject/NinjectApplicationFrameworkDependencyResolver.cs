using System;
using System.Linq;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using Ninject;

namespace AccidentalFish.ApplicationSupport.Ninject
{
    public class NinjectApplicationFrameworkDependencyResolver : IDependencyResolver
    {
        private readonly IKernel _kernel;
        
        public NinjectApplicationFrameworkDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
        }

        public IDependencyResolver Register<T1, T2>() where T2 : T1
        {
            _kernel.Rebind<T1>().To<T2>();
            return this;
        }

        public IDependencyResolver Register<T1>(Func<T1> creator)
        {
            _kernel.Rebind<T1>().ToMethod(context => creator());
            return this;
        }

        public IDependencyResolver Register<T1, T2>(string name) where T2 : T1
        {
            var bindings = _kernel.GetBindings(typeof (T1));
            var binding = bindings.SingleOrDefault(x => x.Metadata.Name == name);
            if (binding != null)
            {
                _kernel.RemoveBinding(binding);
            }
            _kernel.Bind<T1>().To<T2>().Named(name);
            return this;
        }

        public IDependencyResolver Register(Type type1, Type type2)
        {
            _kernel.Rebind(type1).To(type2);
            return this;
        }

        public IDependencyResolver RegisterInstance<T>(T instance)
        {
            _kernel.Rebind<T>().ToConstant(instance);
            return this;
        }

        public bool IsRegistered<T>()
        {
            return _kernel.GetBindings(typeof (T)).Any();
        }

        public T Resolve<T>()
        {
            return _kernel.Get<T>();
        }

        public T Resolve<T>(string name)
        {
            return _kernel.Get<T>(name);
        }
    }
}
