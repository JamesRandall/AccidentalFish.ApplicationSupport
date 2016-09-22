using AccidentalFish.ApplicationSupport.Azure.Blobs;
using AccidentalFish.ApplicationSupport.Azure.Policies;
using AccidentalFish.ApplicationSupport.Core;
using AccidentalFish.ApplicationSupport.Core.Blobs;
using AccidentalFish.ApplicationSupport.Core.Configuration;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Core.Queues;
using AccidentalFish.ApplicationSupport.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
// ReSharper disable InconsistentNaming

namespace AccidentalFish.ApplicationSupport.Azure.Tests.Unit
{
    [TestClass]
    public class ReplacementRegistrationTests
    {
        private IUnityContainer _container;
        private UnityApplicationFrameworkDependencyResolver _resolver;

        [TestInitialize]
        public void Setup()
        {
            _container = new UnityContainer();
            _resolver = new UnityApplicationFrameworkDependencyResolver(_container);
            _resolver.UseCore().UseAzure();
        }

        [TestMethod]
        public void IConfigurationIsReplaced()
        {
            // Act
            IConfiguration configuration = _resolver.Resolve<IConfiguration>();

            // Assert
            Assert.IsInstanceOfType(configuration, typeof(Azure.Configuration.Configuration));
        }

        [TestMethod]
        public void IQueueFactoryIsReplaced()
        {
            // Act
            IQueueFactory dependency = _resolver.Resolve<IQueueFactory>();

            // Assert
            Assert.IsInstanceOfType(dependency, typeof(Queues.QueueFactory));
        }

        [TestMethod]
        public void IBlobRepositoryFactoryIsReplaced()
        {
            // Act
            IBlobRepositoryFactory dependency = _resolver.Resolve<BlobRepositoryFactory>();

            // Assert
            Assert.IsInstanceOfType(dependency, typeof(BlobRepositoryFactory));
        }

        [TestMethod]
        public void ILeaseManagerFactoryIsReplaced()
        {
            // Act
            ILeaseManagerFactory dependency = _resolver.Resolve<ILeaseManagerFactory>();

            // Assert
            Assert.IsInstanceOfType(dependency, typeof(LeaseManagerFactory));
        }
    }
}
