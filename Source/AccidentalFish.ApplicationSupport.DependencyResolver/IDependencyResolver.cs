using System;

namespace AccidentalFish.ApplicationSupport.DependencyResolver
{
    public interface IDependencyResolver
    {
        IDependencyResolver Register<T1, T2>() where T2 : T1;
        IDependencyResolver Register<T1>(Func<T1> creator);
        IDependencyResolver Register<T1, T2>(string name) where T2 : T1;
        IDependencyResolver Register(Type type1, Type type2);
        IDependencyResolver RegisterInstance<T>(T instance);

        bool IsRegistered<T>();

        T Resolve<T>();
        T Resolve<T>(string name);
        object Resolve(Type type);
        object Resolve(Type type, string name);
    }
}
