using System;
using System.Net;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Task4
{
    public static class StringExtensions
    {
        public static string Truncate(this string s, int maxLength) => (s == null || s.Length <= maxLength) ? s : s.Substring(0, maxLength);
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


    public enum Currency
    {
        EUR,
        USD,
        GBP,
        JPY

    }

    public class Price
    {
       
        public Price(decimal amount, Currency unit)
        {
            Amount = amount;
            Unit = unit;
        }

        public decimal Amount { get; }


        public Currency Unit { get; }


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
        string Description { get; }
        int Year { get; }
        Price Price { get; }

        void ChangePrice(Price newPrice);
        void AddDescription(String description);
    }

    public class DLC : IGame
    {

        public string Title { get; }
        public string Console { get; }
        public string Description { get; set; }
        public int Year { get; }
        public Price Price { get; private set; }

        public string Code { get; }
        public bool IsDownloaded { get; private set; }


        public DLC(string title, string console, int year, Price price)
        {
            if (title == null || title == "") throw new Exception("Leerer Titel!");
            if (console == null || console == "") throw new Exception("Keine Konsole definiert!");
            if (year < 1992 || year > 2018) throw new Exception("Undefiniertes Jahr!");

            Title = title;
            Console = console;
            Year = year;
            ChangePrice(price);

            IsDownloaded = false;
        }

        public void Download()
        {
            if (IsDownloaded) throw new InvalidOperationException($"DLC \"{Title}\" has already been downloaded!");
            else
            {
                IsDownloaded = true;   
            }
        }

        public void ChangePrice (Price newPrice)
        {
            if (newPrice.Amount <= 0) throw new ArgumentException("Der Betrag muss größer als 0 sein!", nameof(Price));
            Price = newPrice;
        }

        public void AddDescription(String description)
        {
            Description = description;
        }
    }

    public class VideoGame : IGame
    {
        public string Title { get; }
        public string Console { get; }
        public string Description { get; private set; }
        public int Year { get; }
        public Price Price { get; private set; }

        public VideoGame(string title, string console, int year, Price price)
        {
            if (title == null || title == "") throw new Exception("Leerer Titel!");
            if (console == null || console == "") throw new Exception("Keine Konsole definiert!");
            if (year < 1992 || year > 2017) throw new Exception("Undefiniertes Jahr!");

            Title = title;
            Console = console;
            Year = year;
            ChangePrice(price);
        }

        public void ChangePrice(Price newPrice)
        {
            if (newPrice.Amount < 0) throw new ArgumentException("Der Betrag muss größer als 0 sein!", nameof(Price));
            Price = newPrice;
        }

        public void AddDescription(String description)
        {
            Description = description;
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
                    new VideoGame("The Legend of Zelda: A Link to the Past", "Super Nintendo", 1992, new Price(25.13m, Currency.EUR)),
                    new VideoGame("Donkey Kong Country", "Super Nintendo", 1994, new Price(28.15m, Currency.EUR)),
                    new VideoGame("Final Fantasy 8", "Playstation", 1999, new Price(2800m, Currency.JPY)),
                    new VideoGame("The Last of Us", "Playstation 4", 2013, new Price(75.68m, Currency.USD)),
                    new DLC("Final Fantasy XV DLC 4", "Playstation 4", 2017, new Price(7.90m, Currency.EUR))
                };

                games[0].AddDescription("The Legend of Zelda: A Link to the Past is an action-adventure video game developed and published by Nintendo");
                games[1].AddDescription("Donkey Kong Country is a 1994 platform game developed by Rare and published by Nintendo");
                games[2].AddDescription("Final Fantasy 8 is a 1999 role-playing game developed by Squaresoft");

                foreach (var x in games)
                {
                    Console.WriteLine($"{x.Title,-40} \n\t{x.Console,-15} \n\t{x.Year,-5} \n\tPrice:\t\t\t{x.Price.Unit,-4} {x.Price.Amount,10:0.00} ");
                    var currency = Currency.EUR;
                    Console.WriteLine($"\tConverted Price:\t{currency,-4} {x.Price.ConvertTo(currency).Amount,10:0.00} ");
                    Console.WriteLine($"\tDescription: {x.Description.Truncate(50)}");
                    Console.WriteLine("\n");

                }

                var settings = new JsonSerializerSettings() { Formatting = Formatting.Indented, TypeNameHandling = TypeNameHandling.Auto };
                var text = JsonConvert.SerializeObject(games, settings);
                var desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                var filename = Path.Combine(desktop, "games.json");
                File.WriteAllText(filename, text);
                var textFromFile = File.ReadAllText(filename);
                var itemsFromFile = JsonConvert.DeserializeObject<IGame[]>(textFromFile, settings);

                foreach (var x in itemsFromFile)
                {
                    Console.WriteLine($"{x.Title,-40} \n\t{x.Console,-15}  \n\t{x.Year,-5} \n\tPrice:\t\t\t{x.Price.Unit,-4} {x.Price.Amount,10:0.00}");
                    Console.WriteLine("\n");
                }

            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("CATCH");
            }
        }

    }

}