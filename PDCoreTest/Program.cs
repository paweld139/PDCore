using PDCore.Extensions;
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
            string text = "PawełDywanYoJa";

            text = text.AddSpaces();

            Write(text);
        }

        private static void Write(string value)
        {
            Console.WriteLine(value);

            Console.ReadKey();
        }
    }
}
