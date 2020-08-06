using PDCore.Helpers.Calculation;
using PDCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.DataStructures.Book
{
    public abstract class Book : NamedObject, IBook
    {
        protected Book(string name) : base(name)
        {

        }

        public abstract event EventHandler GradeAdded;
        public abstract void AddGrade(double grade);
        public abstract Statistics GetStatistics();
    }
}
