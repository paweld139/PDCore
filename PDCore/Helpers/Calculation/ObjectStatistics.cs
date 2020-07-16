using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;

namespace PDCore.Helpers.Calculation
{
    public class ObjectStatistics<T> : Statistics
    {
        public ObjectStatistics<T> Accumulate(T item, Func<T, double> propertySelector)
        {
            var itemProperty = propertySelector(item);

            Add(itemProperty);

            return this;
        }

        public ObjectStatistics<T> Compute()
        {
            _average = Total / Count;

            return this;
        }

        private double _average;
        public override double Average => _average;
    }
}
