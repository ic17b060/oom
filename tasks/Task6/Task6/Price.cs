using System;
namespace Task6
{
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
}
