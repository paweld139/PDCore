using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Threading;

namespace PDCore.Converters
{
    public class FormattedDateTimeConverter : IsoDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            Culture = Thread.CurrentThread.CurrentCulture;

            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
    }
}
