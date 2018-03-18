using System;
using System.Net;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task3
{
    public enum Currency
    {
        EUR,
        USD,
        YEN

    }

    public interface IGame
    {
        string Title { get; }
        string Console { get; }
        int Year { get; }
        Currency Currency { get; }


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

        private decimal price;

        public DLC(string title, string console, int year, decimal price, Currency currency)
        {
            if (price <= 0) throw new ArgumentException("Der Betrag muss größer als 0 sein!", nameof(price));
            Title = title;
            Console = console;
            Year = year;
            this.price = price;
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
            return price;
        }
    }

    public class VideoGame : IGame
    {
        private string title;
        private string console;
        private int year;
        private decimal price;
        private Currency currency;
        private decimal amount;

        public string Description { get; }
        public string Title { get; }
        public string Console { get; }
        public int Year { get; }
        public string Code { get; }
        public Currency Currency { get; }

        public VideoGame(string title, string console, int year, decimal price, Currency currency)
        {
            if (title == null || title == "") throw new Exception("Leerer Titel!");
            if (console == null || console == "") throw new Exception("Keine Konsole definiert!");
            if (price < 0) throw new Exception("Negativer Preis!");
            if (year < 1992 || year > 2017) throw new Exception("Undefiniertes Jahr!");
            Title = title;
            Console = console;
            Year = year;
            SetPrice(price);
            this.currency = currency;
        }


        public void SetPrice(decimal newPrice)
        {
            if (newPrice < 0) throw new Exception("Negativer Preis.");
            price = newPrice;
        }

        public decimal GetPrice()
        {
            return price;
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
                }

            }
            catch (IndexOutOfRangeException)
            {
                Console.WriteLine("CATCH");
            }
        }

    }
}