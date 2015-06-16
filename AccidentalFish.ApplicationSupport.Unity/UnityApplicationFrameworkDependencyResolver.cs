using System;
using AccidentalFish.ApplicationSupport.Injection;
using Microsoft.Practices.Unity;

namespace AccidentalFish.ApplicationSupport.Unity
{
    public class UnityApplicationFrameworkDependencyResolver : IDependencyResolver
    {
        private readonly IUnityContainer _container;

        public UnityApplicationFrameworkDependencyResolver(IUnityContainer container)
        {
            _container = container;
        }

        public void Register<T1, T2>() where T2 : T1
        {
            _container.RegisterType<T1, T2>();
        }

        public void Register<T1, T2>(string name) where T2 : T1
        {
            _container.RegisterType<T1, T2>(name);
        }

        public void Register(Type type1, Type type2)
        {
            _container.RegisterType(type1, type2);
        }

        public void RegisterInstance<T>(T instance)
        {
            _container.RegisterInstance(instance);
        }

        public bool IsRegistered<T>()
        {
            return _container.IsRegistered<T>();
        }

        public T Resolve<T>()
        {
            return _container.Resolve<T>();
        }

        public T Resolve<T>(string name)
        {
            return _container.Resolve<T>(name);
        }
    }
}
