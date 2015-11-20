using System;
using AccidentalFish.ApplicationSupport.DependencyResolver;
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

        public IDependencyResolver Register<T1, T2>() where T2 : T1
        {
            _container.RegisterType<T1, T2>();
            return this;
        }

        public IDependencyResolver Register<T1>(Func<T1> creator)
        {
            _container.RegisterType<T1>(new InjectionFactory(context => creator()));
            return this;
        }

        public IDependencyResolver Register<T1, T2>(string name) where T2 : T1
        {
            _container.RegisterType<T1, T2>(name);
            return this;
        }

        public IDependencyResolver Register(Type type1, Type type2)
        {
            _container.RegisterType(type1, type2);
            return this;
        }

        public IDependencyResolver RegisterInstance<T>(T instance)
        {
            _container.RegisterInstance(instance);
            return this;
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
