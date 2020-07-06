using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Calculation
{
    public class Statistics
    {
        public double Average
        {
            get
            {
                return Sum / Count;
            }
        }

        public double High { get; private set; }
        public double Low { get; private set; }

        public char Letter
        {
            get
            {
                switch (Average)
                {
                    case var d when d >= 90.0:
                        return 'A';

                    case var d when d >= 80.0:
                        return 'B';

                    case var d when d >= 70.0:
                        return 'C';

                    case var d when d >= 60.0:
                        return 'D';

                    default:
                        return 'F';
                }
            }
        }

        public double Sum { get; private set; }
        public int Count { get; private set; }

        public void Add(double number)
        {
            Sum += number;
            Count++;
            Low = Math.Min(number, Low);
            High = Math.Max(number, High);
        }

        public Statistics()
        {
            Count = 0;
            Sum = 0.0;
            High = double.MinValue;
            Low = double.MaxValue;
        }
    }
}
