using System;
namespace Task4
{
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
}
