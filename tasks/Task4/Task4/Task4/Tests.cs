using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Task3
{
    [TestFixture]
    public class DLCTests()
    {
        [Test]

        public void IfDLCPriceIsLessThanZeroArgumentExceptionIsThrown()
        {
            var x = new decimal price {-10m};
            Assert.IsTrue(price = new decimal price);
        }
    }
}
