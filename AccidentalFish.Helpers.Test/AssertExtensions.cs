using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AccidentalFish.Helpers.Test
{
    public static class AssertEx
    {
        public static void AreEqual(Color expected, Color actual)
        {
            if (!(expected.R == actual.R && expected.G == actual.G && expected.B == actual.B && expected.A == actual.A))
                throw new AssertFailedException("Colors do not match");
        }
    }
}
