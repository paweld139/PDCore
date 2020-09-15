using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDCore.Converters
{
    public sealed class UtcDateTimeConverter : DateTimeConverter
    {
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            DateTime dateTime = ((DateTime)base.ConvertFrom(context, culture, value));

            if (dateTime.Kind == DateTimeKind.Local)
            {
                dateTime = dateTime.ToUniversalTime();
            }

            return dateTime;
        }
    }
}
