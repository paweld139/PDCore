using PDCore.Helpers.Calculation.StockQuoteAnalysis.Models;
using System.Collections.Generic;

namespace PDCore.Helpers.Calculation.StockQuoteAnalysis.Interfaces
{
    public interface IStockQuoteParser
    {
        IList<StockQuote> ParseQuotes();
    }
}
