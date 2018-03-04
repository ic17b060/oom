using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2
{
    public class VideoGame
    {
        private string title;
        private string console;
        private int year;

        public string GetTitle() { return title; }
        public string GetConsole() { return console; }
        public int GetYear() { return year; }

        public void SetYear(int newYear)
        {
            if (newYear < 1992 || newYear > 2013) throw new Exception("Undefiniertes Jahr!");
            year = newYear;
        }


        public VideoGame(string a, string b, int y)
        {
            if (a == null || a == "") throw new Exception("Leerer Titel!");
            if (b == null || b == "") throw new Exception("Keine Konsole definiert!");
            if (y < 1992 || y > 2013) throw new Exception("Undefiniertes Jahr!");
            SetYear(y);
            title = a;
            console = b;
            year = y;

        }

        class Program
        {
            static void Main(string[] args)
            {
                try
                {
                    VideoGame a = new VideoGame("The Legend of Zelda: A Link to the Past,", "Super Nintendo,", 1992);
                    Console.WriteLine($"a. {a.GetTitle()} {a.GetConsole()} {a.GetYear()}");
                    VideoGame b = new VideoGame("Donkey Kong Country,", "Super Nintendo,", 1994);
                    Console.WriteLine($"b. {b.GetTitle()} {b.GetConsole()} {b.GetYear()}");
                    VideoGame c = new VideoGame("Final Fantasy 8,", "Playstation,", 1999);
                    Console.WriteLine($"c. {c.GetTitle()} {c.GetConsole()} {c.GetYear()}");
                    VideoGame d = new VideoGame("The Last of Us,", "Playstation 4,", 2013);
                    Console.WriteLine($"d. {d.GetTitle()} {d.GetConsole()} {d.GetYear()}");
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