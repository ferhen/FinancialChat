using System;

namespace FinancialBot.Models
{
    public class StockQuote
    {
        public string Symbol { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public float Open { get; set; }
        public float High { get; set; }
        public float Low { get; set; }
        public float Close { get; set; }
        public int Volume { get; set; }

        public string GetQuoteMessage()
        {
            return $"{Symbol} quote is ${Close} per share";
        }
    }
}
