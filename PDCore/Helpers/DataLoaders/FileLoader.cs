using PDCore.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.DataLoaders
{
    public class FileLoader : IDataLoader
    {
        private readonly string _fileName;

        public FileLoader(string fileName)
        {
            _fileName = fileName;
        }

        public string LoadData()
        {
            return File.ReadAllText(_fileName);
        }
    }
}
