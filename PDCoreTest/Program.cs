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

            string filePath = @"D:\Users\User\OneDrive\Magisterka\Semestr 2\Business Intelligence w przedsięborstwie\Laboratoria\Zadanie 3\Zadanie\Dane\KLIENT_DATA_TABLE.csv";

           var customers = IOUtils.ParseCSV<Customer>(filePath);
        }

        private static void Write(string value)
        {
            Console.WriteLine(value);

            Console.ReadKey();
        }
    }
}
