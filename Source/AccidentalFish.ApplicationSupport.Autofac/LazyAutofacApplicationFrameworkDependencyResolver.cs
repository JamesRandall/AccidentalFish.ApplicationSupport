using System;
using System.Threading;
using Autofac;
using Autofac.Builder;

namespace AccidentalFish.ApplicationSupport.Autofac
{
    /// <summary>
    /// Autofac based dependency resolver.
    /// The container is built on the first attempt to resolve a dependency.
    /// </summary>
    public class LazyAutofacApplicationFrameworkDependencyResolver : AbstractAutofacApplicationFrameworkDependencyResolver
    {
        private readonly Lazy<IContainer> _container;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="builder">The Autofac builder to use</param>
        /// <param name="options">Container build options, defaults to ContainerBuildOptions.None</param>
        public LazyAutofacApplicationFrameworkDependencyResolver(ContainerBuilder builder, ContainerBuildOptions options = ContainerBuildOptions.None) : base(builder)
        {
            _container = new Lazy<IContainer>(() => Build(options), LazyThreadSafetyMode.PublicationOnly);
        }

        /// <summary>
        /// Is the type registered. Does not trigger building of the container.
        /// </summary>
        /// <typeparam name="T">Type to verify</typeparam>
        /// <returns>True if the type is registered, otherwise false</returns>
        public override bool IsRegistered<T>()
        {
            if (_container.IsValueCreated)
            {
                return _container.Value.IsRegistered<T>();
            }
            return RegisteredTypes.Contains(typeof (T));
        }

        /// <summary>
        /// Resolve the specified type. Triggers the building of the container if not already built.
        /// </summary>
        /// <typeparam name="T">Type to return an implementation for</typeparam>
        /// <returns>Instantiated dependency</returns>
        public override T Resolve<T>()
        {
            return _container.Value.Resolve<T>();
        }

        /// <summary>
        /// Resolve the specified type with the given name. Triggers the building of the container if not already built.
        /// </summary>
        /// <param name="name">Name of the dependency</param>
        /// <typeparam name="T">Type to return an implementation for</typeparam>
        /// <returns>Instantiated dependency</returns>
        public override T Resolve<T>(string name)
        {
            return _container.Value.ResolveNamed<T>(name);
        }

        public override object Resolve(Type type)
        {
            return _container.Value.Resolve(type);
        }

        public override object Resolve(Type type, string name)
        {
            return _container.Value.ResolveNamed(name, type);
        }

        private IContainer Build(ContainerBuildOptions options)
        {
            return Builder.Build(options);
        }

        /// <summary>
        /// Is the container built
        /// </summary>
        public bool IsBuilt => _container.IsValueCreated;
    }
}
