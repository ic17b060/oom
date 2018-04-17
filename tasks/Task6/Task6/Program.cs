using System;
using System.Net;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reactive.Subjects;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Task6
{
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

            VideoGamePush.Run();
        }

    }

}