using PDCore.Attributes;
using PDCore.Extensions;
using PDCore.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Forms;

namespace PDCore.Utils
{
    public static class ObjectUtils
    {
        public static bool CheckIsOnePropertyTrue(object o)
        {
            return o.GetType().GetProperties().Where(p => p.PropertyType == typeof(bool)).Any(x => x.GetValue(o, null) as bool? == true);
        }

        public static object[] GetParamsOrderedByOrderAttribute(object o)
        {
            return (from property in o.GetType().GetProperties()
                    where Attribute.IsDefined(property, typeof(OrderAttribute))
                    orderby ((OrderAttribute)property
                              .GetCustomAttributes(typeof(OrderAttribute), false)
                              .Single()).Order
                    select property.GetValue(o, null)).ToArray();
        }

        public static string GetParam(object param)
        {
            if (param == null)
            {
                return string.Empty;
            }
            else
            {
                switch (param)
                {
                    case var p when p is string:
                        return string.Format("\"{0}\"", (string)param);

                    case var p when p is decimal:
                        return ((decimal)param).ToString(CultureInfo.InvariantCulture);

                    case var p when p is bool:
                        return Convert.ToByte(param).ToString();

                    case var p when p is DateTime:
                        return ((DateTime)param).GetLong().ToString();

                    case var p when p is Enum:
                        return ((Enum)param).ToString("D");
                }

                return param.ToString();
            }
        }

        public static string GetContent(params object[] content)
        {
            string[] parameters = new string[content.Length];

            int index = 0;

            foreach (object item in content)
            {
                parameters[index] = GetParam(item);

                index++;
            }

            return string.Join(",", parameters);
        }

        public static string Validate(object o)
        {
            ValidationContext context = new ValidationContext(o, null, null);

            IList<ValidationResult> errors = new List<ValidationResult>();

            if (!Validator.TryValidateObject(o, context, errors, true))
            {
                return string.Join(Environment.NewLine, errors);
            }
            else
            {
                return null;
            }
        }
        public static bool AreNull(params object[] results)
        {
            return AreOrNotNull(true, results);
        }

        public static bool AreNotNull(params object[] results)
        {
            return AreOrNotNull(false, results);
        }

        private static bool AreOrNotNull(bool indicator = false, params object[] results)
        {
            bool result = true;

            foreach (object item in results)
            {
                result &= ((item == null) == indicator);

                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        public static string FirstNotNullOrWhiteSpace(params string[] results)
        {
            return results.First(x => !string.IsNullOrWhiteSpace(x));
        }

        public static void ThrowIfNull(params object[] objects) //Parametry mogą mieć różne typy i dlatego brak parametru generycznego
        {
            objects.ForEach(x => x.ThrowIfNull());
        }

        public static void ThrowIfNull(params Tuple<string, object>[] objects)
        {
            objects.ForEach(x => x.Item2.ThrowIfNull(x.Item1));
        }

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

        public static string GetNameOf<T, TT>(Expression<Func<T, TT>> accessor)
        {
            return GetNameOf(accessor.Body);
        }

        public static string GetNameOf<T>(Expression<Func<T>> accessor)
        {
            return GetNameOf(accessor.Body);
        }

        public static string GetNameOf(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                if (!(expression is MemberExpression memberExpression))
                    return null;

                return memberExpression.Member.Name;
            }

            return null;
        }

        public static void SwapValues<T>(ref T object1, ref T object2) //Zmienne muszą być zainicjalizowane przed przekazaniem do metody. Można odczytać wartości.
        {
            T temp = object1;


            object1 = object2;

            object2 = temp;
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> entities)
        {
            var dt = new DataTable();

            //creating columns
            foreach (var prop in typeof(T).GetProperties())
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            //creating rows
            foreach (var entity in entities)
            {
                var values = GetObjectPropertyValues(entity);

                dt.Rows.Add(values);
            }


            return dt;
        }

        public static string[] GetObjectPropertyNames<T>()
        {
            return typeof(T).GetProperties().ConvertArray(p => p.Name);
        }

        public static object[] GetObjectPropertyValues<T>(T entity)
        {
            var values = new List<object>();

            foreach (var prop in typeof(T).GetProperties())
            {
                values.Add(prop.GetValue(entity, null));
            }

            return values.ToArray();
        }

        public static string[] GetObjectPropertyStringValues<T>(T entity)
        {
            return GetObjectPropertyValues(entity).ConvertArray<object, string>();
        }

        public static long Time(Action action, int iterations = 1)
        {
            Stopwatch stopwatch = new Stopwatch();

            return stopwatch.Time(action, iterations);
        }
        public static string GetEnumName<TEnum>(object value) where TEnum : struct
        {
            return Enum.GetName(typeof(TEnum), value);
        }

        public static IEnumerable<double> Random()
        {
            var random = new Random();

            while (true)
            {
                yield return random.NextDouble(); //Między 0 a 1
            }
        }

        public static IEnumerable<char> GetOrderedCharacters(IEnumerable<char> source)
        {
            return source.OrderBy(c => c);
        }

        public static IEnumerable<char> GetCharacters(IEnumerable<string> source)
        {
            return source.SelectMany(s => s);
        }

        public static IEnumerable<char> GetOrderedCharacters(IEnumerable<string> source)
        {
            var characters = GetCharacters(source);

            return GetOrderedCharacters(characters);
        }

        public static IEnumerable<string> GetWithOrderedCharacters(IEnumerable<string> source)
        {
            //return source.Select(s => string.Concat(s.OrderBy(c => c)));
            //return source.Select(s => string.Concat(GetOrderedCharacters(s)));
            //return source.Map(s => s.Order());
            //return from text in source
            //       select string.Concat
            //       (
            //           from character in text
            //           orderby character
            //           select character
            //       );

            return source.Select(s => s.Order());
        }

        public static void SetLogging(bool input, ILogger logger, bool isLoggingEnabled, Action enableLogging, Action disableLogging)
        {
            if (input == isLoggingEnabled || logger == null)
            {
                return;
            }

            if (input)
                enableLogging();
            else
                disableLogging();
        }

        public static string GetCallerMethodName(int index = 2)
        {
            // Get call stack
            StackTrace stackTrace = new StackTrace();

            // Get calling method name
            return stackTrace.GetFrame(index).GetMethod().Name;

            //(new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name //one-liner
        }

        public static bool ValueIn<TInput>(this TInput input, params TInput[] values) where TInput : struct, IEquatable<TInput>
        {
            return values.Any(v => v.Equals(input));
        }

        public static string GetSummary<TInput>(TInput input, int numberPrecision = 2)
        {
            StringBuilder stringBuilder = new StringBuilder();

            string[] propertyNames = GetObjectPropertyNames<TInput>();

            string[] propertyValues = GetObjectPropertyStringValues(input).ConvertArray(v => v.ToNumberString(numberPrecision));

            int padName = propertyNames.GetMaxLength();

            int padValue = propertyValues.GetMaxLength();


            propertyNames.ForEach((p, i) =>
            {
                stringBuilder.AppendLine(
                        StringUtils.ResultFormat,
                        propertyNames[i].PadRight(padName),
                        propertyValues[i].PadLeft(padValue));
            });

            return stringBuilder.ToString();
        }
    }
}
