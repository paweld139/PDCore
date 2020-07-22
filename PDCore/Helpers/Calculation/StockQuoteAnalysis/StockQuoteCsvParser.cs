using PDCore.Helpers.Calculation.StockQuoteAnalysis.Interfaces;
using PDCore.Helpers.Calculation.StockQuoteAnalysis.Models;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PDCore.Helpers.Calculation.StockQuoteAnalysis
{
    public class StockQuoteCsvParser : IStockQuoteParser
    {
        private readonly IDataLoader _loader;

        public StockQuoteCsvParser(IDataLoader loader)
        {
            _loader = loader;
        }

        public IList<StockQuote> ParseQuotes()
        {
            var csvData = _loader.LoadData().Split('\n');

            return
                    (from line in csvData.Skip(1)
                     let data = line.Split(',')
                     where data[0].Length > 0
                     select new StockQuote
                     {
                         Date = DateTime.Parse(data[0]),
                         Open = decimal.Parse(data[1]),
                         High = decimal.Parse(data[2]),
                         Low = decimal.Parse(data[3]),
                         Close = decimal.Parse(data[4])
                     }).ToList();
        }
    }
}
