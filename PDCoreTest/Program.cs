using PDCore.Extensions;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using PDCore.Enums;
using System.Diagnostics;
using System.Data;
using PDCore.Helpers;
using PDWebCore.Repositories.Repo;
using PDWebCore;
using PDCoreNew;
using System.IO;
using System.Runtime.InteropServices;

namespace PDCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestConvertCSVToDataTableAndWriteDataTable();

            //TestParseCSVToObjectAndDisplayObject();

            //TestWriteResultAndWriteByte();

            //TestWrapRepo();

            //TestStopWatchTime();

            //TestDisposableStopwatch();

            //TestCacheService();

            //TestObjectConvertTo();

            //TestIEnumerableConvertTo();

            //TestSampledAverage();

            TestMultiply();


            Console.ReadKey();
        }

        private static void TestMultiply()
        {
            var item = true;

            int multiplier = 3;

            var result = item.Multiply(multiplier);

            Console.WriteLine(result);
        }

        private static void TestSampledAverage()
        {
            var items = new[] { true };

            var result = items.SampledAverage();

            Console.WriteLine(result);
        }

        private static void TestObjectConvertTo()
        {
            double x = 1.5;

            x = x.ConvertTo<double, int>();

            Console.WriteLine(x);
        }

        private static void TestIEnumerableConvertTo()
        {
            IEnumerable<double> x = new[] { 1.5, 6.3, 7.9, 5.6 };

            var y = x.ConvertTo<double, int>();

            string result = string.Join(", ", y);

            Console.WriteLine(result);
        }

        private static void TestConvertCSVToDataTableAndWriteDataTable()
        {
            string filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Big Data\Laboratoria\Koronawirus - Zadanie\CSVs\Podzielone pliki\20.csv";

            string filePath2 = @"D:\Users\User\OneDrive\Magisterka\Semestr 1\Python\Notebooki\Dane\orders\orders.csv";

            string filePath3 = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Data Mining\Ćwiczenia\Dane\datasets_12603_17232_Life Expectancy Data.csv";


            DataTable dataTable = CSVUtils.ParseCSVToDataTable(filePath);

            ConsoleUtils.WriteDataTable(dataTable);


            dataTable = CSVUtils.ParseCSVToDataTable(filePath2, delimiter: "\t");

            ConsoleUtils.WriteDataTable(dataTable);


            dataTable = CSVUtils.ParseCSVToDataTable(filePath3);

            ConsoleUtils.WriteDataTable(dataTable);
        }

        private static void TestParseCSVToObjectAndDisplayObject()
        {
            const string filePathFormat = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Business Intelligence w przedsięborstwie\Laboratoria\Zadanie 3\Zadanie\Dane\{0}_DATA_TABLE.csv";

            string tableName = "klient";

            string filePath = string.Format(filePathFormat, tableName.ToUpper());


            var customers = CSVUtils.ParseCSV<Customer>(filePath);

            var customersFromMap = CSVUtils.ParseCSV<Customer, CustomerMap>(filePath, false);


            ConsoleUtils.WriteTableFromObjects(customers, false);

            ConsoleUtils.WriteTableFromObjects(customersFromMap, false);
        }

        private static void TestWriteResultAndWriteByte()
        {
            ConsoleUtils.WriteResult("Ilość lat", 23);

            ConsoleUtils.WriteByte(37);
        }

        private static void TestWrapRepo()
        {
            using (new FileRepository(null).WrapRepo())
            {

            }
        }

        private static void TestStopWatchTime()
        {
            string filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 1\Python\Notebooki\Dane\orders\orders.csv";

            Stopwatch stopwatch = new Stopwatch();

            long time = stopwatch.Time(() =>
            {
                CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
                CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
                CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            });

            Console.WriteLine(time);


            time = ObjectUtils.Time(() => CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList());

            Console.WriteLine(time);


            time = ObjectUtils.Time(() => CSVUtils.ParseCSVLines2(filePath, delimiter: "\t").ToList());

            Console.WriteLine(time);
        }

        private static void TestDisposableStopwatch()
        {
            string filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 1\Python\Notebooki\Dane\orders\orders.csv";

            using (new DisposableStopwatch(t => Console.WriteLine("{0} elapsed", t)))
            {
                // do stuff that I want to measure
                CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            }

            using (new DisposableStopwatch())
            {
                CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            }
        }

        private static void TestCacheService()
        {
            ICacheService inMemoryCache = new InMemoryCache();

            IEnumerable<string> lines = ConsoleUtils.ReadLines();

            string text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            Console.WriteLine(text);

            text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            Console.WriteLine(text);

            text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            Console.WriteLine(text);

            lines.Where(x => x.Length > 1).Skip(3).ForEach(x => Console.WriteLine(x));
        }
    }
}
