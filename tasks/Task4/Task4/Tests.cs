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
                var x = new VideoGame("The Legend of Zelda: A Link to the Past", "Super Nintendo", 1992, new Price(-25.13m, Currency.EUR));
            });
        }

        [Test]
        public void TestVideoGamePublishingYear()
        {
            Assert.Catch(() =>
            {
                var x = new VideoGame("The Legend of Zelda: A Link to the Past", "Super Nintendo", 1982, new Price(25.13m, Currency.EUR));
            });
        }

        [Test]
        public void TestVideoGameTitle()
        {
            Assert.Catch(() =>
            {
                var x = new VideoGame("", "Super Nintendo", 1992, new Price(25.13m, Currency.EUR));
            });
        }

        [Test]
        public void TestVideoGameConsoleTitle()
        {
            Assert.Catch(() =>
            {
                var x = new VideoGame("The Legend of Zelda: A Link to the Past", "", 1992, new Price(25.13m, Currency.EUR));
            });
        }

        [Test]
        public void ExchangeRateForSameCurrencyIsOne()
        {
            var x = ExchangeRates.Get(Currency.EUR, Currency.EUR);
            Assert.IsTrue(x == 1);
        }

        [Test]
        public void CanDownloadDLC()
        {
            var x = new DLC("Final Fantasy XV DLC 4", "Playstation 4", 2017, new Price(8.90m, Currency.EUR));
            Assert.IsFalse(x.IsDownloaded == true);
        }


    }

}