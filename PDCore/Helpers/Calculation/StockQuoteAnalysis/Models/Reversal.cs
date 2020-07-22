using PDCore.Helpers.Calculation.StockQuoteAnalysis.Enums;

namespace PDCore.Helpers.Calculation.StockQuoteAnalysis.Models
{
    public class Reversal
    {
        public Reversal(StockQuote quote, ReversalDirection direction)
        {
            StockQuote = quote;
            Direction = direction;
        }

        public ReversalDirection Direction { get; set; }
        public StockQuote StockQuote { get; set; }
    }
}
