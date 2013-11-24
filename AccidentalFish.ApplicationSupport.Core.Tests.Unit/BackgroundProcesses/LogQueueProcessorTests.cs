using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccidentalFish.ApplicationSupport.Core.Components;
using AccidentalFish.ApplicationSupport.Core.InternalMappers;
using AccidentalFish.ApplicationSupport.Core.Naming;
using AccidentalFish.ApplicationSupport.Core.Policies;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AccidentalFish.ApplicationSupport.Core.Tests.Unit.BackgroundProcesses
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
