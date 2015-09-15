using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Runtime.Implementation;
using AccidentalFish.ApplicationSupport.DependencyResolver;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.Runtime.Implementation
{
    [TestClass]
    public class ComponentFactoryTests
    {
        private Mock<IDependencyResolver> _dependencyResolver;

        [TestInitialize]
        public void Setup()
        {
            _dependencyResolver = new Mock<IDependencyResolver>();
        }

        [TestMethod]
        public void Construct()
        {
            var factory = new ComponentFactory(_dependencyResolver.Object);
            Assert.IsNotNull(factory);
        }

        [TestMethod]
        public void ReturnsComponent()
        {
            // Arrange
            var identity = new ComponentIdentity("hello");
            var hostableComponent = new Mock<IHostableComponent>();
            _dependencyResolver.Setup(x => x.Resolve<IHostableComponent>("hello")).Returns(hostableComponent.Object);
            var factory = new ComponentFactory(_dependencyResolver.Object);

            // Act
            IHostableComponent result = factory.Create(identity);

            // Asssert
            Assert.AreSame(hostableComponent.Object, result);
        }

        [TestMethod]
        public void UnregisteredThrowsException()
        {
            // Arrange
            var identity = new ComponentIdentity("hellonotfound");
            var hostableComponent = new Mock<IHostableComponent>();
            _dependencyResolver.Setup(x => x.Resolve<IHostableComponent>("hellonotfound")).Throws<Exception>();
            var factory = new ComponentFactory(_dependencyResolver.Object);

            // Act
            IHostableComponent result = factory.Create(identity);

            // Assert
            Assert.IsNull(result);
        }
    }
}
