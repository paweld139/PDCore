using System;

namespace PDCore.Helpers.Calculation.StockQuoteAnalysis.Models
{
    public class StockQuote
    {
        public DateTime Date { get; set; }
        public decimal Open { get; set; }
        public decimal High { get; set; }
        public decimal Low { get; set; }
        public decimal Close { get; set; }

        public bool ReversesDownFrom(StockQuote otherQuote)
        {
            return Open > otherQuote.High && Close < otherQuote.Low;
        }

        public bool ReversesUpFrom(StockQuote otherQuote)
        {
            return Open < otherQuote.Low && Close > otherQuote.High;
        }
    }
}
