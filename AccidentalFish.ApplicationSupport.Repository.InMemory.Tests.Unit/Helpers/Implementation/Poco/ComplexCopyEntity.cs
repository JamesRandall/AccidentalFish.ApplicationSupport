using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccidentalFish.ApplicationSupport.Repository.InMemory.Tests.Unit.Helpers.Implementation.Poco
{
    public class ComplexCopyEntity
    {
        public byte Value { get; set; }

        public SimpleEntity Reference { get; set; }
    }
}
