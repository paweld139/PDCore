using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Calculation
{
    public class NamedObject
    {
        public NamedObject(string name)
        {
            Name = name;
        }

        public string Name { get; protected set; }
    }

    public interface IBook
    {
        void AddGrade(double grade);
        Statistics GetStatistics();
        string Name { get; }
        event EventHandler GradeAdded;
    }

    public abstract class Book : NamedObject, IBook
    {
        protected Book(string name) : base(name)
        {

        }

        public abstract event EventHandler GradeAdded;
        public abstract void AddGrade(double grade);
        public abstract Statistics GetStatistics();
    }

    public class DiskBook : Book
    {
        public DiskBook(string name) : base(name)
        {

        }

        public override event EventHandler GradeAdded;

        public override void AddGrade(double grade)
        {
            using (var writer = File.AppendText($"{Name}.txt"))
            {
                writer.WriteLine(grade);

                GradeAdded?.Invoke(this, new EventArgs());
            }
        }

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            string line;
            double number;

            using (var reader = File.OpenText($"{Name}.txt"))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    number = double.Parse(line);
                    result.Add(number);
                }
            }

            return result;
        }
    }

    public class InMemoryBook : Book
    {
        public InMemoryBook(string name) : base(name)
        {
            grades = new List<double>();
        }

        private readonly List<double> grades;

        public void AddGrade(char letter)
        {
            switch (letter)
            {
                case 'A':
                    AddGrade(90);
                    break;

                case 'B':
                    AddGrade(80);
                    break;

                case 'C':
                    AddGrade(70);
                    break;

                default:
                    AddGrade(0);
                    break;
            }
        }

        public override void AddGrade(double grade)
        {
            if (grade <= 100 && grade >= 0)
            {
                grades.Add(grade);

                GradeAdded?.Invoke(this, new EventArgs());
            }
            else
            {
                throw new ArgumentException($"Invalid {nameof(grade)}");
            }
        }

        public override event EventHandler GradeAdded;

        public override Statistics GetStatistics()
        {
            var result = new Statistics();

            for (var i = 0; i < grades.Count; i++)
            {
                result.Add(grades[i]);
            }

            return result;
        }
    }
}
