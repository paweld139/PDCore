﻿using PDCore.Enums;
using PDCore.Extensions;
using PDCore.Factories.Fac;
using PDCore.Helpers;
using PDCore.Helpers.DataStructures;
using PDCore.Interfaces;
using PDCore.Services.IServ;
using PDCore.Utils;
using PDCoreNew.Loggers;
using PDCoreNew.Loggers.Factory;
using PDCoreNew.Services.Serv;
using PDCoreTest.Factory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

namespace PDCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            _ = args;


            TestOrderBy();

            WriteSeparator();

            TestCsvParsing();

            WriteSeparator();

            TestGetEnumValues();

            WriteSeparator();

            TestLoggerFactory();

            WriteSeparator();

            TestFactory();

            WriteSeparator();

            TestLogMessageFactory();

            WriteSeparator();

            TestNameOf();

            WriteSeparator();

            TestCategoryCollection();

            WriteSeparator();

            TestConvertCSVToDataTableAndWriteDataTable();

            WriteSeparator();

            TestParseCSVToObjectAndDisplayObject();

            WriteSeparator();

            TestWriteResultAndWriteByte();

            WriteSeparator();

            TestStopWatchTime();

            WriteSeparator();

            TestDisposableStopwatch();

            WriteSeparator();

            TestCacheService();

            WriteSeparator();

            TestObjectConvertTo();

            WriteSeparator();

            TestIEnumerableConvertTo();

            WriteSeparator();

            TestSampledAverage();

            WriteSeparator();

            TestMultiply();


            Console.ReadKey();
        }

        private static void TestOrderBy()
        {
            string[] a = new string[] { "Indonesian", "Korean", "Japanese", "English", "German" };

            var sort = from s in a orderby s select s;

            ConsoleUtils.WriteLines(sort);


            string[] str = new string[] { "abc", "bacd", "pacds" };

            var result = str.Select(c => string.Concat(c.OrderBy(d => d)));

            ConsoleUtils.WriteLines(result);


            var i = new object[] { "efwef", 1, 2.0, true };

            ConsoleUtils.WriteLines(i);


            IEnumerable<object> i2 = new object[] { "efwef", 1, 2.0, true };

            ConsoleUtils.WriteLines(i2);

            ConsoleUtils.WriteLines(1, 2, 3);

            ConsoleUtils.WriteLines(1, false, "ergerg", 2.1, 3m, 6L);
        }

        private static void TestCsvParsing()
        {
            string path = @"D:\Downloads\1253a5fc75280025995fa7f3cb61000e-6b4989b6cf20ddc619c28a2e51eb9e27eb185709\fuel.csv";

            ConsoleUtils.WriteTableFromCSV(path);


            var dt = CSVUtils.ParseCSVToDataTable(path);

            ConsoleUtils.WriteDataTable(dt);


            var customers = CSVUtils.ParseCSV<Customer, CustomerMap>(path);

            ConsoleUtils.WriteLine(customers.Last().Name);


            var customers2 = CSVUtils.ParseCSV<Customer>(path);

            ConsoleUtils.WriteLine(customers2.First().Name);
        }

        private static void TestGetEnumValues()
        {
            var enumValues = ObjectUtils.GetEnumValues(typeof(CertificateType));

            var result = enumValues.ConvertOrCastTo<object, string>();


            string enumName = ObjectUtils.GetEnumName<CertificateType>(2);

            result = result.Append(enumName);


            ConsoleUtils.WriteLines(result);



            ConsoleUtils.WriteSeparator();



            var enumValues2 = ObjectUtils.GetEnumValues<CertificateType, decimal>();

            ConsoleUtils.WriteLines(enumValues2);


            var certificateType = CertificateType.WSS;

            object enumNumber = Convert.ChangeType(certificateType, typeof(string));

            ConsoleUtils.WriteLine(enumNumber);


            var enumNumbers = ObjectUtils.GetEnumNumbers<HorizontalTextAlignment>();

            ConsoleUtils.WriteLines(enumNumbers);
        }

        private static void TestLoggerFactory()
        {
            var loggerFactory = new LoggerFactory();


            loggerFactory.ExecuteCreation(Loggers.Console).Log("Wiadomość", LogType.Fatal);

            loggerFactory.ExecuteCreation(Loggers.Trace).Log("Wiadomość2", LogType.Debug);


            ILogger inMemoryLogger = loggerFactory.ExecuteCreation(Loggers.InMemory);

            inMemoryLogger.Log("Wiadomość3", LogType.Info);
            inMemoryLogger.Log("Wiadomość3", LogType.Error);

            ConsoleUtils.WriteLines(InMemoryLogger.Logs);
        }

        private static void TestFactory()
        {
            AirConditioner.InitializeFactories().ExecuteCreation(Actions.Cooling, 22.5).Operate();
        }

        private static void TestLogMessageFactory()
        {
            LogMessageFactory logMessageFactory = new LogMessageFactory();

            string message = logMessageFactory.Create("Wiadomość", new ArgumentException("Nieprawidłowa wartość argumentu"), LogType.Info);

            string message2 = logMessageFactory.Create(null, new ArgumentException("Nieprawidłowa wartość argumentu"), LogType.Info);

            string message3 = logMessageFactory.Create(null, null, LogType.Info);

            string message4 = logMessageFactory.Create("Wiadomość", null, LogType.Info);

            ConsoleUtils.WriteLines(message, message2, message3, message4);
        }

        private static void TestNameOf()
        {
            NamedObject namedObject = new NamedObject("Nazwa");

            string propertyName = namedObject.GetNameOf(x => x.Name);
            string propertyName2 = namedObject.Name.GetName(() => namedObject.Name);

            ConsoleUtils.WriteResult("Nazwa zmiennej Name z poziomu obiektu", propertyName);
            ConsoleUtils.WriteResult("Nazwa zmiennej Name bezpośrednio", propertyName2);
        }

        private static void TestCategoryCollection()
        {
            Stopwatch stopwatch = new Stopwatch();

            long time1 = stopwatch.Time(() =>
            {
                CategoryCollection categoryCollection = new CategoryCollection();

                Enumerable.Range(0, 1000000).ForEach(x => categoryCollection.Add("Kategoria", new NamedObject("Nazwa")));
            });

            long time2 = stopwatch.Time(() =>
            {
                CategoryCollection categoryCollection = new CategoryCollection();

                var itemsToAdd = Enumerable.Range(0, 1000000).Select(x => new NamedObject("Nazwa"));

                categoryCollection.AddRange("Kategoria", itemsToAdd);
            });

            ConsoleUtils.WriteResult("Elementy dodane pojedynczo", time1);
            ConsoleUtils.WriteResult("Elementy dodane wszystkie naraz", time2);
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

        private static void WriteSeparator()
        {
            ConsoleUtils.WriteSeparator(true);
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
            ICacheService inMemoryCache = new CacheService();

            IEnumerable<string> lines = ConsoleUtils.ReadLines();

            string text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            Console.WriteLine(text);

            text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            Console.WriteLine(text);

            text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            Console.WriteLine(text);

            lines.Where(x => x.Length > 1).Skip(3).ForEach(x => Console.WriteLine(x));
        }

        private static void TestObjectConvertTo()
        {
            double x = 1.5;

            x = x.ConvertOrCastTo<double, int>();

            Console.WriteLine(x);
        }

        private static void TestIEnumerableConvertTo()
        {
            IEnumerable<double> x = new[] { 1.5, 6.3, 7.9, 5.6 };

            var y = x.ConvertOrCastTo<double, int>();

            string result = string.Join(", ", y);

            Console.WriteLine(result);
        }

        private static void TestSampledAverage()
        {
            var items = new[] { true };

            var result = items.SampledAverage();

            Console.WriteLine(result);
        }

        private static void TestMultiply()
        {
            var item = true;

            int multiplier = 3;

            var result = item.Multiply(multiplier);

            Console.WriteLine(result);
        }
    }
}
