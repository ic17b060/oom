using System;
using System.Net;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Task4
{
    public enum Currency
    {
        EUR,
        USD,
        YEN

    }

    public class Price
    {
        /// <summary>
        /// Creates a price in given currency.
        /// </summary>
        public Price(decimal amount, Currency unit)
        {
            Amount = amount;
            Unit = unit;
        }

        /// <summary>
        /// Gets the amount.
        /// </summary>
        public decimal Amount { get; }

        /// <summary>
        /// Amount's currency.
        /// </summary>
        public Currency Unit { get; }

        /// <summary>
        /// Converts price to given currency.
        /// </summary>
        public Price ConvertTo(Currency targetCurrency)
        {
            if (targetCurrency == Unit) return this;
            return new Price(Amount * ExchangeRates.Get(Unit, targetCurrency), targetCurrency);
        }
    }

    public interface IGame
    {
        string Title { get; }
        string Console { get; }
        int Year { get; }
        Currency Currency { get; }
        Price Price { get; }


        decimal GetPrice();
    }

    public class DLC : IGame
    {

        public bool IsDownloaded { get; private set; }

        public string Description { get; }
        public string Title { get; }
        public string Console { get; }
        public int Year { get; }
        public string Code { get; }
        public Currency Currency { get; }
        public Price Price { get; }

        public DLC(string title, string console, int year, Price Price, Currency currency)
        {
            if (Price <= 0) throw new ArgumentException("Der Betrag muss größer als 0 sein!", nameof(Price));
            Title = title;
            Console = console;
            Year = year;
            this.Price = Price;
            Currency = currency;
            Code = Guid.NewGuid().ToString();
            IsDownloaded = false;
        }

        public void Download()
        {
            if (IsDownloaded) throw new InvalidOperationException($"DLC {Code} has already been downloaded!");
        }

        public decimal GetPrice()
        {
            return Price;
        }
    }

    public class VideoGame : IGame
    {
        private string title;
        private string console;
        private int year;
        private Currency currency;
        private Price price; 
        private decimal amount;

        public string Description { get; }
        public string Title { get; }
        public string Console { get; }
        public int Year { get; }
        public string Code { get; }
        public Currency Currency { get; }
        public Price Price { get; }

        public VideoGame(string title, string console, int year, Price price, Currency currency)
        {
            if (title == null || title == "") throw new Exception("Leerer Titel!");
            if (console == null || console == "") throw new Exception("Keine Konsole definiert!");
            if (Price < 0) throw new Exception("Negativer Preis!");
            if (year < 1992 || year > 2017) throw new Exception("Undefiniertes Jahr!");
            Title = title;
            Console = console;
            Year = year;
            SetPrice(Price);
            this.currency = currency;
        }


        public void SetPrice(decimal newPrice)
        {
            if (newPrice < 0) throw new Exception("Negativer Preis.");
            Price = newPrice;
        }

        public decimal GetPrice()
        {
            return Price;
        }

        public static class ExchangeRates
        {
            private static Dictionary<string, decimal> s_rates = new Dictionary<string, decimal>();

            /// <summary>
            /// Gets exchange rate 'from' currency 'to' another currency.
            /// </summary>
            public static decimal Get(Currency from, Currency to)
            {
                // exchange rate is 1:1 for same currency
                if (from == to) return 1;

                // use web service to query current exchange rate
                // request : http://download.finance.yahoo.com/d/quotes.csv?s=EURUSD=X&f=sl1d1t1c1ohgv&e=.csv
                // response: "EURUSD=X",1.0930,"12/29/2015","6:06pm",-0.0043,1.0971,1.0995,1.0899,0
                var key = string.Format("{0}{1}", from.ToString(), to.ToString()); // e.g. EURUSD means "How much is 1 EUR in USD?".

                // use web service to query current exchange rate
                // request : https://api.fixer.io/latest?base=EUR&symbols=USD
                // response: {"base":"EUR","date":"2018-01-24","rates":{"USD":1.2352}}
                var url = $"https://api.fixer.io/latest?base={from}&symbols={to}";
                // download the response as string
                var data = new WebClient().DownloadString(url);
                // parse JSON
                var json = JObject.Parse(data);
                // convert the exchange rate part to a decimal 
                var rate = decimal.Parse((string)json["rates"][to.ToString()], CultureInfo.InvariantCulture);

                // cache the exchange rate
                s_rates[key] = rate;

                // and finally perform the currency conversion
                return rate;
            }
        }


    }
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var games = new IGame[]
                {
                    new VideoGame("The Legend of Zelda: A Link to the Past", "Super Nintendo", 1992, 25.13m, Currency.EUR),
                    new VideoGame("Donkey Kong Country", "Super Nintendo", 1994, 28.15m, Currency.EUR),
                    new VideoGame("Final Fantasy 8", "Playstation", 1999, 2800m, Currency.YEN),
                    new VideoGame("The Last of Us", "Playstation 4", 2013, 75.68m, Currency.USD),
                    new DLC("Final Fantasy XV DLC 4", "Playstation 4", 2017, 7.90m, Currency.EUR)
                };

                foreach (var x in games)
                {
                    Console.WriteLine($"{x.Title,-40} {x.Console,-20} {x.Year,-8} {x.GetPrice()} {x.Currency,-5}");
                    var currency = Currency.EUR;
                    Console.WriteLine($"{x.Description.Truncate(50),-50} {x.Price.ConvertTo(currency).Amount,8:0.00} {currency}");
                }

            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("CATCH");
            }
        }

    }
}