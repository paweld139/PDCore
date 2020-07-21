using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreTest
{
    public class Employee
    {
        public string FirstName { get; set; }

        public void Speak()
        {
            Console.WriteLine("Hi, my name is {0}", FirstName);
        }
    }
}
