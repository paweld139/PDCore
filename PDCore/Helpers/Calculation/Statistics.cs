using PDCore.Extensions;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Calculation
{
    public class Statistics
    {
        public virtual double Average
        {
            get
            {
                return Total / Count;
            }
        }

        public double Max { get; private set; }
        public double Min { get; private set; }

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

        public double Total { get; private set; }
        public int Count { get; private set; }

        public void Add(double number)
        {
            Total += number;
            Count++;
            Min = Math.Min(number, Min);
            Max = Math.Max(number, Max);
        }

        public Statistics()
        {
            Count = 0;
            Total = 0.0;
            Max = double.MinValue;
            Min = double.MaxValue;
        }

        public override string ToString()
        {
            return ObjectUtils.GetSummary(this);
        }
    }
}
