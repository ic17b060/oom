using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace Task4
{
    [TestFixture]
    public class VideoGameTests
    {
        [Test]
        public void TestVideoGamePrice()
        {
            Assert.Catch(() =>
            {
                var x = new VideoGame("The Legend of Zelda: A Link to the Past", "Super Nintendo", 1992, -25.13m, Currency.EUR);
            });
        }

        [Test]
        public void TestVideoGamePublishingYear()
        {
            Assert.Catch(() =>
            {
                var x = new VideoGame("The Legend of Zelda: A Link to the Past", "Super Nintendo", 1982, 25.13m, Currency.EUR);
            });
        }

        [Test]
        public void CanConvertVideoGamePrice()
        {
            var x = new amount(1, Currency.EUR);
            Assert.IsTrue(x.ConvertTo(Currency.YEN).Amount > 1);
        }
    }
}

