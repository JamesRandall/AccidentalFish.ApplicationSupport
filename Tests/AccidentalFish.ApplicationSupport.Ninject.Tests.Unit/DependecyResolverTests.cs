using AccidentalFish.ApplicationSupport.DependencyResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;

namespace AccidentalFish.ApplicationSupport.Ninject.Tests.Unit
{
    [TestClass]
    public class DependecyResolverTests
    {
        private IKernel _kernel;
        private IDependencyResolver _resolver;

        [TestInitialize]
        public void Setup()
        {
            _kernel = new StandardKernel();
            _resolver = new NinjectApplicationFrameworkDependencyResolver(_kernel);
        }

        [TestMethod]
        public void RegisterAndResolveInstanceWithInferredType()
        {
            _resolver.RegisterInstance(new TestObject());
            TestObject result = _resolver.Resolve<TestObject>();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void RegisterAndResolveInstanceWithSpecifiedType()
        {
            _resolver.RegisterInstance<ITestObject>(new TestObject());
            ITestObject result = _resolver.Resolve<TestObject>();
            Assert.IsInstanceOfType(result, typeof(TestObject));
        }

        [TestMethod]
        public void GenericCanResolveRegisteredInterface()
        {
            _resolver.Register<ITestObject, TestObject>();
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(TestObject));
        }

        [TestMethod]
        public void GenericSecondRegistrationWins()
        {
            _resolver.Register<ITestObject, TestObject>();
            _resolver.Register<ITestObject, SecondTestObject>();
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }

        [TestMethod]
        public void TypeCanResolveRegisteredInterface()
        {
            _resolver.Register(typeof(ITestObject), typeof(TestObject));
            ITestObject result = _resolver.Resolve<ITestObject>();
            Assert.IsInstanceOfType(result, typeof(TestObject));
        }

        [TestMethod]
        public void TypeSecondRegistrationWins()
        {
            _resolver.Register(typeof(ITestObject), typeof(TestObject));
            _resolver.Register(typeof(ITestObject), typeof(SecondTestObject));
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
            ITestObject result = _resolver.Resolve<ITestObject>("something");
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }

        [TestMethod]
        public void NamedRegistrationReplaces()
        {
            _resolver.Register<ITestObject, TestObject>("something");
            _resolver.Register<ITestObject, SecondTestObject>("something");
            ITestObject result = _resolver.Resolve<ITestObject>("something");
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }

        [TestMethod]
        public void TwoNamedRegistrationsCoexist()
        {
            _resolver.Register<ITestObject, TestObject>("something.else");
            _resolver.Register<ITestObject, SecondTestObject>("something");
            ITestObject result = _resolver.Resolve<ITestObject>("something");
            ITestObject result2 = _resolver.Resolve<ITestObject>("something.else");
            Assert.IsInstanceOfType(result, typeof(SecondTestObject));
        }
    }
}
