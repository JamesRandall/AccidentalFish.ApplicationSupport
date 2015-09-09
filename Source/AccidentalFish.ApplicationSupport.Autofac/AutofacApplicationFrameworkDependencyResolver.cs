using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using Autofac;
using Autofac.Builder;

namespace AccidentalFish.ApplicationSupport.Autofac
{
    public class AutofacApplicationFrameworkDependencyResolver : IDependencyResolver
    {
        private readonly ContainerBuilder _builder;
        private IContainer _container;

        public AutofacApplicationFrameworkDependencyResolver(ContainerBuilder builder)
        {
            _builder = builder;
        }

        public void Register<T1, T2>() where T2 : T1
        {
            _builder.RegisterType<T2>().As<T1>();
        }

        public void Register<T1, T2>(string name) where T2 : T1
        {
            _builder.RegisterType<T2>().Named<T1>(name);
        }

        public void Register(Type type1, Type type2)
        {
            _builder.RegisterType(type2).As(type1);
        }

        public void RegisterInstance<T>(T instance)
        {
            _builder.Register<T>(c => instance);
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
            return _container.ResolveNamed<T>(name);
        }

        public IContainer Build(ContainerBuildOptions options = ContainerBuildOptions.None)
        {
            _container = _builder.Build(options);
            return _container;
        }
    }
}
