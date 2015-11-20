using System;
using System.Collections.Generic;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using Autofac;

namespace AccidentalFish.ApplicationSupport.Autofac
{
    public abstract class AbstractAutofacApplicationFrameworkDependencyResolver : IDependencyResolver
    {
        private readonly ContainerBuilder _builder;
        private readonly HashSet<Type> _registeredTypes = new HashSet<Type>(); 

        protected AbstractAutofacApplicationFrameworkDependencyResolver(ContainerBuilder builder)
        {
            _builder = builder;
        }

        protected ContainerBuilder Builder => _builder;

        protected HashSet<Type> RegisteredTypes => _registeredTypes; 

        public IDependencyResolver Register<T1, T2>() where T2 : T1
        {
            _builder.RegisterType<T2>().As<T1>();
            _registeredTypes.Add(typeof (T1));
            return this;
        }

        public IDependencyResolver Register<T1>(Func<T1> creator)
        {
            _builder.Register(c => creator()).As<T1>();
            _registeredTypes.Add(typeof(T1));
            return this;
        }

        public IDependencyResolver Register<T1, T2>(string name) where T2 : T1
        {
            _builder.RegisterType<T2>().Named<T1>(name);
            _registeredTypes.Add(typeof(T1));
            return this;
        }

        public IDependencyResolver Register(Type type1, Type type2)
        {
            _builder.RegisterType(type2).As(type1);
            _registeredTypes.Add(type1);
            return this;
        }

        public IDependencyResolver RegisterInstance<T>(T instance)
        {
            _builder.Register<T>(c => instance);
            _registeredTypes.Add(typeof(T));
            return this;
        }

        public abstract bool IsRegistered<T>();

        public abstract T Resolve<T>();

        public abstract T Resolve<T>(string name);
    }
}
