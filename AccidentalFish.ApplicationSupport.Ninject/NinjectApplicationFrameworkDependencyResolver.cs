using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Register<T1, T2>() where T2 : T1
        {
            _kernel.Rebind<T1>().To<T2>();
        }

        public void Register<T1, T2>(string name) where T2 : T1
        {
            _kernel.Bind<T1>().To<T2>().Named(name);
        }

        public void Register(Type type1, Type type2)
        {
            _kernel.Rebind(type1).To(type2);
        }

        public void RegisterInstance<T>(T instance)
        {
            _kernel.Rebind<T>().ToConstant(instance);
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
