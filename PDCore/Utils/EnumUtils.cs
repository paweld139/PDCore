using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace PDCore.Utils
{
    public static class EnumUtils
    {
        public static IEnumerable<object> GetEnumValues(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("EnumType must be of Enum type", "enumType");

            return Enum.GetValues(enumType).Cast<object>();
        }

        public static IEnumerable<TEnum> GetEnumValues<TEnum>() where TEnum : struct
        {
            return GetEnumValues(typeof(TEnum)).Cast<TEnum>();
        }

        public static IEnumerable<TResult> GetEnumValues<TEnum, TResult>() where TEnum : struct
        {
            return GetEnumValues<TEnum>().ConvertOrCastTo<TEnum, TResult>();
        }

        public static IEnumerable<int> GetEnumNumbers<TEnum>() where TEnum : struct
        {
            return GetEnumValues<TEnum, int>();
        }

        public static string GetEnumName<TEnum>(object value) where TEnum : struct
        {
            return Enum.GetName(typeof(TEnum), value);
        }

        public static T GetValueFromEnumMember<T>(string value)
        {
            var type = typeof(T);

            if (!type.IsEnum) 
                throw new InvalidOperationException();

            foreach (var field in type.GetFields())
            {
                if (Attribute.GetCustomAttribute(field, typeof(EnumMemberAttribute)) is EnumMemberAttribute attribute)
                {
                    if (attribute.Value == value)
                        return (T)field.GetValue(null);
                }
                else
                {
                    if (field.Name == value)
                        return (T)field.GetValue(null);
                }
            }

            throw new ArgumentException($"unknow value: {value}");
        }
    }
}
