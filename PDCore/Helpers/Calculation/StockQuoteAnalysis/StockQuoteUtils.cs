using PDCore.Extensions;
using PDCore.Helpers.Calculation.StockQuoteAnalysis.Enums;
using PDCore.Helpers.Calculation.StockQuoteAnalysis.Models;
using PDCore.Helpers.DataLoaders;
using PDCore.Interfaces;
using System;
using System.Drawing;

namespace PDCore.Helpers.Calculation.StockQuoteAnalysis
{
    public static class StockQuoteUtils
    {
        public static IDataLoader GetLoaderFor(string source)
        {
            IDataLoader loader;

            if (source.IsUrl())
            {
                loader = new WebLoader(source);
            }
            else
            {
                loader = new FileLoader(source);
            }

            return loader;
        }

        private static void PrintReversal(Reversal reversal, Action<string, Color> print)
        {
            if (reversal.Direction == ReversalDirection.Down)
            {
                print(string.Format("Pivot downside {0}", reversal.StockQuote.Date.ToShortDateString()), Color.Red);
            }

            if (reversal.Direction == ReversalDirection.Up)
            {
                print(string.Format("Pivot upside {0}", reversal.StockQuote.Date.ToShortDateString()), Color.Green);
            }
        }

        public static void PrintReversalOnConsole(Reversal reversal)
        {
            PrintReversal(reversal, (m, c) =>
            {
                Console.ForegroundColor = c.ToConsoleColor();

                Console.WriteLine(m);
            });

            Console.ForegroundColor = ConsoleColor.White;
        }

        public static void AnalyzeAndPrintStockQuote(string source, Action<string, Color> print)
        {
            // set the scene ...
            var loader = GetLoaderFor(source);
            var parser = new StockQuoteCsvParser(loader);
            var analyzer = new StockQuoteAnalyzer(parser);

            // ... action!!!
            foreach (var reversal in analyzer.FindReversals())
            {
                PrintReversal(reversal, print);
            }
        }
    }
}
