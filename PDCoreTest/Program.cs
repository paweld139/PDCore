using PDCore.Extensions;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

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

            string tableName = "PRODUKT";

            string filePath = string.Format(filePathFormat, tableName);

            //var customers = IOUtils.ParseCSV<Customer>(filePath);

            //ConsoleUtils.WriteTable(customers, false);

            ConsoleUtils.WriteTableFromCSV(filePath);
        }
    }
}
