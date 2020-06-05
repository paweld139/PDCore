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


            //filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Big Data\Laboratoria\Koronawirus - Zadanie\CSVs\Podzielone pliki\20.csv";

            //filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 1\Python\Notebooki\Dane\orders\orders.csv";


            //Stopwatch stopwatch = Stopwatch.StartNew();

            //ConsoleUtils.WriteTableFromCSV(filePath);

            //ConsoleUtils.WriteResult("Ilość lat", 23);

            //ConsoleUtils.WriteByte(37);

            //stopwatch.Stop();


            //ConsoleUtils.WriteLine(stopwatch.Elapsed.TotalSeconds.ToString());

            DataTable dataTable = CSVUtils.ParseCSVToDataTable(filePath);

            ConsoleUtils.WriteDataTable(dataTable);
        }
    }
}
