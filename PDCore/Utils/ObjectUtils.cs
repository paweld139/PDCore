using PDCore.Attributes;
using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows.Forms;

namespace PDCore.Utils
{
    public static class ObjectUtils
    {
        private static object resultListBox;

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
                if (param is string)
                {
                    return string.Format("\"{0}\"", (string)param);
                }
                else if (param is decimal)
                {
                    return ((decimal)param).ToString(CultureInfo.InvariantCulture);
                }
                else if (param is bool)
                {
                    return Convert.ToByte(param).ToString();
                }
                else if (param is DateTime)
                {
                    return ((DateTime)param).GetLong().ToString();
                }
                else if (param is Enum)
                {
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

        public static void ThrowIfNull(params object[] objects)
        {
            objects.ForEach(x => x.ThrowIfNull());
        }

        public static void ThrowIfNull(params KeyValuePair<string, object>[] objects)
        {
            objects.ForEach(x => x.Value.ThrowIfNull(x.Key));
        }

        public static IEnumerable<T> GetEnumValues<T>()
        {
            return Enum.GetValues(typeof(T)).Cast<T>();
        }

        public static IEnumerable<U> GetEnumValues<T, U>()
        {
            return Enum.GetValues(typeof(T)).Cast<U>();
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
                var memberExpression = expression as MemberExpression;

                if (memberExpression == null)
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
                var values = GetObjectValues(entity);
                dt.Rows.Add(values);
            }


            return dt;
        }

        public static object[] GetObjectValues<T>(T entity)
        {
            var values = new List<object>();

            foreach (var prop in typeof(T).GetProperties())
            {
                values.Add(prop.GetValue(entity, null));
            }

            return values.ToArray();
        }

        public static long Time(Action action, int iterations = 1)
        {
            Stopwatch stopwatch = new Stopwatch();

            return stopwatch.Time(action, iterations);
        }
    }
}
