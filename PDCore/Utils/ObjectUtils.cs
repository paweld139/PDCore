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
using System.Reflection;
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
            foreach (var prop in GetProperties<T>())
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            //creating rows
            foreach (var entity in entities)
            {
                var values = GetObjectPropertyValues(entity).ToArray();

                dt.Rows.Add(values);
            }


            return dt;
        }

        private static PropertyInfo[] GetProperties<T>()
        {
            return typeof(T).GetProperties();
        }

        public static IEnumerable<string> GetObjectPropertyNames<T>(PropertyInfo[] propertyInfos = null)
        {
            return (propertyInfos ?? GetProperties<T>()).GetPropertyNames();
        }

        public static IEnumerable<object> GetObjectPropertyValues<T>(T entity, PropertyInfo[] propertyInfos = null)
        {
            return (propertyInfos ?? GetProperties<T>()).Select(p => p.GetPropertyValue(entity));
        }

        public static IEnumerable<string> GetObjectPropertyStringValues<T>(T entity, PropertyInfo[] propertyInfos = null)
        {
            return GetObjectPropertyValues(entity, propertyInfos).EmptyIfNull();
        }

        public static IEnumerable<KeyValuePair<string, object>> GetObjectPropertyKeyValuePairs<T>(T entity)
        {
            return GetProperties<T>().GetKVP(k => k.Name, v => v.GetPropertyValue(entity));
        }

        public static IEnumerable<KeyValuePair<string, string>> GetObjectPropertyKeyValuePairsString<T>(T entity)
        {
            return GetProperties<T>().GetKVP(k => k.Name, v => v.GetPropertyValue(entity).EmptyIfNull());
        }

        public static TResult GetObjectPropertyIDictionary<T, TResult>(T entity) where TResult : class, IDictionary<string, object>, new()
        {
            return GetProperties<T>().ToIDictionary<PropertyInfo, string, object, TResult>(k => k.Name, v => v.GetPropertyValue(entity));
        }

        public static TResult GetObjectPropertyIDictionaryString<T, TResult>(T entity) where TResult : class, IDictionary<string, string>, new()
        {
            return GetProperties<T>().ToIDictionary<PropertyInfo, string, string, TResult>(k => k.Name, v => v.GetPropertyValue(entity).EmptyIfNull());
        }

        public static Dictionary<string, object> GetObjectPropertyDictionary<T>(T entity)
        {
            return GetObjectPropertyIDictionary<T, Dictionary<string, object>>(entity);
        }

        public static Dictionary<string, string> GetObjectPropertyDictionaryString<T>(T entity)
        {
            return GetObjectPropertyIDictionaryString<T, Dictionary<string, string>>(entity);
        }

        public static SortedDictionary<string, object> GetObjectPropertySortedDictionary<T>(T entity)
        {
            return GetObjectPropertyIDictionary<T, SortedDictionary<string, object>>(entity);
        }

        public static SortedDictionary<string, string> GetObjectPropertySortedDictionaryString<T>(T entity)
        {
            return GetObjectPropertyIDictionaryString<T, SortedDictionary<string, string>>(entity);
        }

        public static SortedList<string, object> GetObjectPropertySortedList<T>(T entity)
        {
            return GetObjectPropertyIDictionary<T, SortedList<string, object>>(entity);
        }

        public static SortedList<string, string> GetObjectPropertySortedListString<T>(T entity)
        {
            return GetObjectPropertyIDictionaryString<T, SortedList<string, string>>(entity);
        }

        public static KeyValuePair<string[], object[]> GetObjectPropertyNamesAndValues<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairs(entity).ToArrays();
        }

        public static KeyValuePair<string[], string[]> GetObjectPropertyNamesAndValuesString<T>(T entity)
        {
            var namesAndValues = GetObjectPropertyNamesAndValues(entity);

            return new KeyValuePair<string[], string[]>(namesAndValues.Key, namesAndValues.Value.ToArrayString());
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

        public static string GetSummary<TInput>(TInput input, int numberPrecision = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();

            var propertyNamesAndValues = GetObjectPropertyKeyValuePairsString(input);

            if (numberPrecision > 0)
                propertyNamesAndValues = propertyNamesAndValues.GetKVP(i => i.Key, i => i.Value.ToNumberString(numberPrecision));

            var propertyNamesAndValuesList = propertyNamesAndValues.OrderBy(i => i.Key).ToList();

            var columnWidths = StringUtils.GetColumnsWidths(propertyNamesAndValuesList);


            foreach (var item in propertyNamesAndValuesList)
            {
                stringBuilder.AppendLine(
                        StringUtils.ResultFormat,
                        item.Key.PadRight(columnWidths.Key),
                        " ",
                        item.Value.PadRight(columnWidths.Value));
            }

            return stringBuilder.ToString();
        }

        public static string GetSummary2<TInput>(TInput input, int numberPrecision = 0)
        {
            StringBuilder stringBuilder = new StringBuilder();


            var properties = GetProperties<TInput>();


            var propertyNamesArray = GetObjectPropertyNames<TInput>(properties).ToArray();

            var propertyValues = GetObjectPropertyStringValues(input, properties);

            if (numberPrecision > 0)
                propertyValues = propertyValues.Select(v => v.ToNumberString(numberPrecision));


            var propertyValuesArray = propertyValues.ToArray();


            Array.Sort(propertyNamesArray, propertyValuesArray);


            int padName = propertyNamesArray.GetMaxLength();

            int padValue = propertyValuesArray.GetMaxLength();


            propertyNamesArray.ForEach((p, i) =>
            {
                stringBuilder.AppendLine(
                        StringUtils.ResultFormat,
                        propertyNamesArray[i].PadRight(padName),
                        " ",
                        propertyValuesArray[i].PadLeft(padValue));
            });

            return stringBuilder.ToString();
        }

        public static IEnumerable<KeyValuePair<TKey, TValue>> GetKVPs<TKey, TValue>(IEnumerable<TKey> keys, IEnumerable<TValue> values)
        {
            return keys.GetKVP(k => k, i => values.ElementAt(i));
        }

        public static IEnumerable<T> GetChangedObjects<T>(IEnumerable<T> objects, IEnumerable<T> cachedObjects)
        {
            var newAndChanged = objects.Except(cachedObjects);
            var removedAndChanged = cachedObjects.Except(objects);
            var changed = newAndChanged.Concat(removedAndChanged);

            return changed;
        }

        public static void InvokeMethod(object obj, string methodName, params object[] parameters)
        {
            obj.GetType().GetMethod(methodName).Invoke(obj, parameters);
        }
        public static dynamic GetDynamic(string assemblyString, string typeName)
        {
            Type type = Assembly.Load(assemblyString).GetType(typeName);

            return Activator.CreateInstance(type);
        }

        public static dynamic GetDynamic(string progID)
        {
            Type type = Type.GetTypeFromProgID(progID);

            return Activator.CreateInstance(type);
        }

        public static dynamic GetExcel()
        {
            return GetDynamic("Excel.Application");
        }

        public static dynamic OpenExcel()
        {
            dynamic excel = GetExcel();

            excel.Visible = true;

            excel.Workbooks.Add();

            return excel;
        }

        public static dynamic OpenExcelAndGetActiveSheet()
        {
            dynamic excel = OpenExcel();

            return excel.ActiveSheet;
        }

        public static void OpenExcelWithProcessesAndThreads()
        {
            dynamic sheet = OpenExcelAndGetActiveSheet();

            var processesWithThreads = IOUtils.GetProcessesWithThreads();


            processesWithThreads.ForEach((e, i) =>
            {
                sheet.Cells[i + 1, "A"] = e.Key;

                sheet.Cells[i + 1, "B"] = e.Value;
            });
        }
    }
}
