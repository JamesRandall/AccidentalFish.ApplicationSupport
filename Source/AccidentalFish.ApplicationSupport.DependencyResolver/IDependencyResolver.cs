using System;

namespace AccidentalFish.ApplicationSupport.DependencyResolver
{
    public interface IDependencyResolver
    {
        void Register<T1, T2>() where T2 : T1;
        void Register<T1, T2>(string name) where T2 : T1;
        void Register(Type type1, Type type2);
        void RegisterInstance<T>(T instance);
        bool IsRegistered<T>();

        T Resolve<T>();
        T Resolve<T>(string name);
    }
}
