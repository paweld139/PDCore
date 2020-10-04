using PDCoreNew.Helpers.Calculation.StockQuoteAnalysis.Models;
using System.Collections.Generic;

namespace PDCoreNew.Helpers.Calculation.StockQuoteAnalysis.Interfaces
{
    public interface IStockQuoteParser
    {
        IList<StockQuote> ParseQuotes();
    }
}
