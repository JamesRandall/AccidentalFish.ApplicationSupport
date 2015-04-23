using System;
using AccidentalFish.ApplicationSupport.Repository.InMemory.Helpers.Implementation;
using AccidentalFish.ApplicationSupport.Repository.InMemory.Tests.Unit.Helpers.Implementation.Poco;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Tests.Unit.Helpers.Implementation
{
    [TestClass]
    public class CopyCompilerTests
    {
        [TestMethod]
        public void BasicShallowCopy()
        {
            // Arrange
            CopyCompiler compiler = new CopyCompiler();
            Func<SimpleEntity, SimpleEntity> copier = compiler.Compile<SimpleEntity>();
            SimpleEntity input = new SimpleEntity
            {
                Id = 2,
                Name = "Zaphod Beeblebrox"
            };

            // Act
            SimpleEntity output = copier(input);

            // Assert
            Assert.AreEqual(2, output.Id);
            Assert.AreEqual("Zaphod Beeblebrox", output.Name);
        }

        [TestMethod]
        public void ShallowCopyIgnoresReferenceType()
        {
            // Arrange
            CopyCompiler compiler = new CopyCompiler();
            Func<ComplexCopyEntity, ComplexCopyEntity> copier = compiler.Compile<ComplexCopyEntity>();
            ComplexCopyEntity input = new ComplexCopyEntity
            {
                Reference = new SimpleEntity
                {
                    Id = 2,
                    Name = "Zaphod Beeblebrox"
                },
                Value = 95
            };

            // Act
            ComplexCopyEntity output = copier(input);

            // Assert
            Assert.AreEqual(95, output.Value);
            Assert.IsNull(output.Reference);
        }
    }
}
