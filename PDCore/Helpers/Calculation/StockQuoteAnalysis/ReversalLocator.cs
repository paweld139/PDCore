using PDCore.Helpers.Calculation.StockQuoteAnalysis.Enums;
using PDCore.Helpers.Calculation.StockQuoteAnalysis.Models;
using System.Collections.Generic;

namespace PDCore.Helpers.Calculation.StockQuoteAnalysis
{
    public class ReversalLocator
    {
        private readonly IList<StockQuote> _quotes;

        public ReversalLocator(IList<StockQuote> quotes)
        {
            _quotes = quotes;
        }

        public IEnumerable<Reversal> Locate()
        {
            for (int i = 0; i < _quotes.Count - 1; i++)
            {
                if (_quotes[i].ReversesDownFrom(_quotes[i + 1]))
                {
                    yield return new Reversal(_quotes[i], ReversalDirection.Down);
                }

                if (_quotes[i].ReversesUpFrom(_quotes[i + 1]))
                {
                    yield return new Reversal(_quotes[i], ReversalDirection.Up);
                }
            }
        }
    }
}
