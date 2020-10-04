using PDCoreNew.Helpers.Calculation.StockQuoteAnalysis.Interfaces;
using PDCoreNew.Helpers.Calculation.StockQuoteAnalysis.Models;
using System.Collections.Generic;

namespace PDCoreNew.Helpers.Calculation.StockQuoteAnalysis
{
    public class StockQuoteAnalyzer
    {
        private readonly IList<StockQuote> _quotes;

        public StockQuoteAnalyzer(IStockQuoteParser parser)
        {
            _quotes = parser.ParseQuotes();
        }

        public IEnumerable<Reversal> FindReversals()
        {
            var locator = new ReversalLocator(_quotes);

            return locator.Locate();
        }
    }
}
