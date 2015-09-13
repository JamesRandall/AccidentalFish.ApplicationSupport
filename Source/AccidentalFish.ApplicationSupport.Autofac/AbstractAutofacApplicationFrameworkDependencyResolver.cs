using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void Register<T1, T2>() where T2 : T1
        {
            _builder.RegisterType<T2>().As<T1>();
            _registeredTypes.Add(typeof (T1));
        }

        public void Register<T1, T2>(string name) where T2 : T1
        {
            _builder.RegisterType<T2>().Named<T1>(name);
            _registeredTypes.Add(typeof(T1));
        }

        public void Register(Type type1, Type type2)
        {
            _builder.RegisterType(type2).As(type1);
            _registeredTypes.Add(type1);
        }

        public void RegisterInstance<T>(T instance)
        {
            _builder.Register<T>(c => instance);
            _registeredTypes.Add(typeof(T));
        }

        public abstract bool IsRegistered<T>();

        public abstract T Resolve<T>();

        public abstract T Resolve<T>(string name);
    }
}
