using CsvHelper.Configuration;
using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreTest
{
    class Customer : IFromCSVParseable
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string NIP { get; set; }

        public string PhoneNumber { get; set; }


        public static Customer ParseCSV(string[] fields)
        {
            return new Customer
            {
                Id = fields[0],
                Name = fields[1],
                Address = fields[2],
                NIP = fields[3],
                PhoneNumber = fields[4]
            };
        }

        public void ParseFromCSV(string[] fields)
        {
            Id = fields[0];
            Name = fields[1];
            Address = fields[2];
            NIP = fields[3];
            PhoneNumber = fields[4];
        }
    }
}
