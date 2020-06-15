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

namespace PDCoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //string text = "PawełDywanYoJa";

            //text = text.AddSpaces();

            //Write(text);

            const string filePathFormat = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Business Intelligence w przedsięborstwie\Laboratoria\Zadanie 3\Zadanie\Dane\{0}_DATA_TABLE.csv";

            string tableName = "klient";

            string filePath = string.Format(filePathFormat, tableName.ToUpper());


            //var customers = CSVUtils.ParseCSV<Customer>(filePath);

            //var customers = CSVUtils.ParseCSV<Customer, CustomerMap>(filePath, false);

            //ConsoleUtils.WriteTableFromObjects(customers, false);


            filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Big Data\Laboratoria\Koronawirus - Zadanie\CSVs\Podzielone pliki\20.csv";

            filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 1\Python\Notebooki\Dane\orders\orders.csv";

            filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Data Mining\Ćwiczenia\Dane\datasets_12603_17232_Life Expectancy Data.csv";

            DataTable dataTable = CSVUtils.ParseCSVToDataTable(filePath);

            //Stopwatch stopwatch = Stopwatch.StartNew();

            //ConsoleUtils.WriteTableFromCSV(filePath);

            //ConsoleUtils.WriteResult("Ilość lat", 23);

            //ConsoleUtils.WriteByte(37);

            //stopwatch.Stop();


            //ConsoleUtils.WriteLine(stopwatch.Elapsed.TotalSeconds.ToString());

            //Stopwatch stopwatch = Stopwatch.StartNew();

            //DataTable dataTable = CSVUtils.ParseCSVToDataTable(filePath, delimiter: "\t");

            //var lineFields = CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();

            //long time = ObjectUtils.Time(() => CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList());

            //using (new DisposableStopwatch(t => Console.WriteLine("{0} elapsed", t)))
            //{
            //    // do stuff that I want to measure
            //    CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            //}

            //using (new DisposableStopwatch())
            //{
            //    CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            //}

            //Stopwatch stopwatch = new Stopwatch();

            //long time = stopwatch.Time(() =>
            //{
            //    CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            //    CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            //    CSVUtils.ParseCSVLines(filePath, delimiter: "\t").ToList();
            //});

            //Console.WriteLine(time);

            //using (new FileRepository(null).WrapRepo())
            //{

            //}

            //stopwatch.Stop();

            //Console.WriteLine(stopwatch.Elapsed.TotalSeconds);

            //Console.WriteLine(time);

            //ICacheService inMemoryCache = new InMemoryCache();

            //IEnumerable<string> lines = ConsoleUtils.ReadLines();

            //string text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            //Console.WriteLine(text);

            //text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            //Console.WriteLine(text);

            //text = inMemoryCache.GetOrSet("text", () => Console.ReadLine());
            //Console.WriteLine(text);

            //lines.Where(x => x.Length > 1).Skip(3).ForEach(x => Console.WriteLine(x));

            Console.ReadKey();

            //ConsoleUtils.WriteDataTable(dataTable);
        }
    }
}
