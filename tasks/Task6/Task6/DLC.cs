using System;
namespace Task6
{
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

        public void ChangePrice(Price newPrice)
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
}
