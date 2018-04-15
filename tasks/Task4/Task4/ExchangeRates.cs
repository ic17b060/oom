using System;
namespace Task4
{
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
