using PDCore.Helpers.Calculation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.DataStructures.Book
{
    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event EventHandler GradeAdded;
    }
}
