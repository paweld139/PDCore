using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PDCore.Extensions;
using System;
using System.Web;

namespace PDCoreNew.Converters
{
    public class DateTimeOffsetConverter : IsoDateTimeConverter
    {
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object baseResult = base.ReadJson(reader, objectType, existingValue, serializer);

            DateTimeOffset? date = baseResult as DateTimeOffset?;

            if (date.HasValue)
            {
                //timezone offset
                int? timezoneOffsetMinutes = HttpContext.Current.Request.Headers["TimezoneOffsetMinutes"].ParseAsNullableInteger();

                TimeSpan? timezoneOffset = timezoneOffsetMinutes.HasValue
                    ? TimeSpan.FromMinutes(timezoneOffsetMinutes.Value)
                    : (TimeSpan?)null;

                return timezoneOffset.HasValue
                    ? date.Value.ToOffset(timezoneOffset.Value)
                    : date.Value;
            }

            return baseResult;
        }
    }
}
