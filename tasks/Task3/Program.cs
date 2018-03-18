using System;
using System.Net;
using System.Globalization;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public enum Currency
    {
        EUR,
        USD,
        YEN
    }

    public interface IGame
    {
        string Description { get; }
        double GetPrice(Currency currency);
    }

    public class DLC : IGame
    {
        public decimal amount;

        public bool IsDownloaded { get; private set; }

        public DLC(decimal amount, Currency currency)
        {
            if (amount <= 0) throw new ArgumentException("Der Betrag muss größer als 0 sein!", nameof(amount));

            Amount = amount;
            Currency = currency;
            Code = Guid.NewGuid().ToString();
            IsDownloaded = false;
        }

        public decimal Amount { get; }

        public string Description { get; }

        public Currency Currency { get; }

        public string Code { get; }

        public void Download()
        {
            if (IsDownloaded) throw new InvalidOperationException($"DLC {Code} has already been downloaded!");
        }

        public double GetPrice(Currency currency)
        {
            if (currency == Currency) return Amount;

            var from = Currency.ToString();
            var to = currency.ToString();
            var url = $"https://api.fixer.io/latest?base={from}&symbols={to}";
            var data = new WebClient().DownloadString(url);
            var json = JObject.Parse(data);
            var rate = decimal.Parse((string)json["rates"][to], CultureInfo.InvariantCulture);

            return Amount * rate;

        }
    }

    public class VideoGame : IGame
    {
        private string title;
        private string console;
        private int year;
        private double price;
        private decimal currency;

        public string GetTitle() { return title; }
        public string GetConsole() { return console; }
        public int GetYear() { return year; }
        public double GetPrice() { return price; }
        public string Code { get; }

        public void SetYear(int newYear)
        {
            if (newYear < 1992 || newYear > 2013) throw new Exception("Undefiniertes Jahr!");
            year = newYear;
        }

        public void SetPrice(double newPrice)
        {
            if (newPrice < 0) throw new Exception("Negativer Preis.");
            price = newPrice;
        }

        public double GetPrice(Currency currency)
        {
            // if the price is requested in it's own currency, then simply return the stored price
            if (currency == Currency) return Amount;

            var from = Currency.ToString();
            var to = currency.ToString();

            var url = $"https://api.fixer.io/latest?base={from}&symbols={to}";

            var data = new WebClient().DownloadString(url);

            var json = JObject.Parse(data);
             
            var rate = decimal.Parse((string)json["rates"][to], CultureInfo.InvariantCulture);


            return Amount * rate;
        }

        public string Description => "DLC" + Code;

        public VideoGame(string a, string b, int y, double p, double c)
        {
            if (a == null || a == "") throw new Exception("Leerer Titel!");
            if (b == null || b == "") throw new Exception("Keine Konsole definiert!");
            if (y < 1992 || y > 2013) throw new Exception("Undefiniertes Jahr!");
            if (p < 0) throw new Exception("Negativer Preis!");
            SetYear(y);
            title = a;
            console = b;
            year = y;
            price = p;
            currency = c;
        }

        class Program
        {
            static void Main(string[] args)
            {
                try
                {
                    VideoGame a = new VideoGame("The Legend of Zelda: A Link to the Past,", "Super Nintendo,", 1992, 25.13, Currency.EUR);
                    Console.WriteLine($"a. {a.GetTitle()} {a.GetConsole()} {a.GetYear()} {a.GetPrice()}");
                    VideoGame b = new VideoGame("Donkey Kong Country,", "Super Nintendo,", 1994, 28.15, Currency.EUR);
                    Console.WriteLine($"b. {b.GetTitle()} {b.GetConsole()} {b.GetYear()} {b.GetPrice()}");
                    VideoGame c = new VideoGame("Final Fantasy 8,", "Playstation,", 1999, 2800, Currency.YEN);
                    Console.WriteLine($"c. {c.GetTitle()} {c.GetConsole()} {c.GetYear()} {c.GetPrice()}");
                    VideoGame d = new VideoGame("The Last of Us,", "Playstation 4,", 2013, 75.68, Currency.USD);
                    Console.WriteLine($"d. {d.GetTitle()} {d.GetConsole()} {d.GetYear()} {d.GetPrice()}");
                }

                catch(IndexOutOfRangeException)
                {
                    Console.WriteLine("CATCH");
                }
                Console.WriteLine("DONE");
            }

        }
    }
}