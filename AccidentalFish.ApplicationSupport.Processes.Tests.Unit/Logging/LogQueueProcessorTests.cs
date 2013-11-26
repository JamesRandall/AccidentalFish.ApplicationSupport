using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.Policies;
using AccidentalFish.ApplicationSupport.Processes.Mappers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Processes.Tests.Unit.Logging
{
    [TestClass]
    public class LogQueueProcessorTests
    {
        private Mock<IApplicationResourceFactory> _applicationResourceFactory;
        private Mock<IAsynchronousBackoffPolicy> _asynchronousBackoffPolicy;
        private Mock<IMapperFactory> _mapperFactory;

        [TestInitialize]
        public void Setup()
        {
            _applicationResourceFactory = new Mock<IApplicationResourceFactory>();
            _asynchronousBackoffPolicy = new Mock<IAsynchronousBackoffPolicy>();
            _mapperFactory = new Mock<IMapperFactory>();
        }
    }
}
