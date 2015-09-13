using System;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AccidentalFish.ApplicationSupport.Autofac.Tests.Unit
{
    [TestClass]
    public class ExplicitAutofacApplicationFrameworkDependencyResolverTests
    {
        private ContainerBuilder _container;
        private ExplicitAutofacApplicationFrameworkDependencyResolver _resolver;

        [TestInitialize]
        public void Setup()
        {
            _container = new ContainerBuilder();
            _resolver = new ExplicitAutofacApplicationFrameworkDependencyResolver(_container);
        }

        [TestMethod]
        public void RegisterAndResolveInstanceWithInferredType()
        {
            _resolver.RegisterInstance(new TestObject());
            _resolver.Build();
            TestObject result = _resolver.Resolve<TestObject>();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegisterAndResolveInstanceWithSpecifiedType()
        {
            _resolver.RegisterInstance<ITestObject>(new TestObject());
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(TestObject));
        }

        [TestMethod]
        public void GenericCanResolveRegisteredInterface()
        {
            _resolver.Register<ITestObject, TestObject>();
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(TestObject));
        }

        [TestMethod]
        public void GenericSecondRegistrationWins()
        {
            _resolver.Register<ITestObject, TestObject>();
            _resolver.Register<ITestObject, SecondTestObject>();
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }

        [TestMethod]
        public void TypeCanResolveRegisteredInterface()
        {
            _resolver.Register(typeof(ITestObject), typeof(TestObject));
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(TestObject));
        }

        [TestMethod]
        public void TypeSecondRegistrationWins()
        {
            _resolver.Register(typeof(ITestObject), typeof(TestObject));
            _resolver.Register(typeof(ITestObject), typeof(SecondTestObject));
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }

        [TestMethod]
        public void IsRegisteredReturnsTrue()
        {
            _resolver.Register<ITestObject, TestObject>();
            bool result = _resolver.IsRegistered<ITestObject>();
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void IsRegisteredReturnsFalse()
        {
            bool result = _resolver.IsRegistered<ITestObject>();
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void NamedRegistrationResovles()
        {
            _resolver.Register<ITestObject, SecondTestObject>("something");
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>("something");
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }

        [TestMethod]
        public void NamedRegistrationReplaces()
        {
            _resolver.Register<ITestObject, TestObject>("something");
            _resolver.Register<ITestObject, SecondTestObject>("something");
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>("something");
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }

        [TestMethod]
        public void TwoNamedRegistrationsCoexist()
        {
            _resolver.Register<ITestObject, TestObject>("something.else");
            _resolver.Register<ITestObject, SecondTestObject>("something");
            _resolver.Build();
            ITestObject result = _resolver.Resolve<ITestObject>("something");
            ITestObject result2 = _resolver.Resolve<ITestObject>("something.else");
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
            Assert.IsInstanceOfType(result2, typeof(TestObject));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ResolveBeforeBuildErrors()
        {
            _resolver.Register<ITestObject, TestObject>();
            _resolver.Resolve<ITestObject>();
        }
    }
}
