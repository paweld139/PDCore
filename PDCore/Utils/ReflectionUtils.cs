using IronRuby;
using PDCore.Attributes;
using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PDCore.Utils
{
    public static class ReflectionUtils
    {
        public static dynamic GetIronRubyRunitimeGlobals(IDictionary<string, object> variables, string fileToExecute)
        {
            var engine = Ruby.CreateEngine();

            var scope = engine.CreateScope();

            variables?.ForEach(v => scope.SetVariable(v.Key, v.Value));

            if (!string.IsNullOrEmpty(fileToExecute))
                engine.ExecuteFile(fileToExecute, scope);

            return engine.Runtime.Globals;
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

        public static string GetCallerMethodName(int index = 2)
        {
            // Get call stack
            StackTrace stackTrace = new StackTrace();

            // Get calling method name
            return stackTrace.GetFrame(index).GetMethod().Name;

            //(new System.Diagnostics.StackTrace()).GetFrame(1).GetMethod().Name //one-liner
        }

        public static DataTable CreateDataTable<T>(IEnumerable<T> entities)
        {
            var dt = new DataTable();

            var properties = GetProperties<T>();

            //creating columns
            foreach (var prop in properties)
            {
                dt.Columns.Add(prop.Name, prop.PropertyType);
            }

            //creating rows
            foreach (var entity in entities)
            {
                var values = GetObjectPropertyValues(entity, properties).ToArray();

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
            return (propertyInfos ?? GetProperties<T>()).GetPropertyValues(entity);
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
            return GetProperties<T>().GetKVP(k => k.Name, v => v.GetPropertyValueString(entity));
        }

        public static Dictionary<string, object> GetObjectPropertyDictionary<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairs(entity).ToDictionary();
        }

        public static Dictionary<string, string> GetObjectPropertyDictionaryString<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairsString(entity).ToDictionary();
        }

        public static SortedDictionary<string, object> GetObjectPropertySortedDictionary<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairs(entity).ToSortedDictionary();
        }

        public static SortedDictionary<string, string> GetObjectPropertySortedDictionaryString<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairsString(entity).ToSortedDictionary();
        }

        public static SortedList<string, object> GetObjectPropertySortedList<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairs(entity).ToSortedList();
        }

        public static SortedList<string, string> GetObjectPropertySortedListString<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairsString(entity).ToSortedList();
        }

        public static KeyValuePair<string[], object[]> GetObjectPropertyNamesAndValues<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairs(entity).ToArrays();
        }

        public static KeyValuePair<string[], string[]> GetObjectPropertyNamesAndValuesString<T>(T entity)
        {
            return GetObjectPropertyKeyValuePairsString(entity).ToArrays();
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

        public static IEnumerable<Type> GetImmediateInterfaces<T>() => typeof(T).GetImmediateInterfaces();

        public static bool ImplementsInterface<TInput, TInterface>() => typeof(TInput).ImplementsInterface<TInterface>();

        public static object CreateCollection(Type collectionType, Type itemType)
        {
            var closedType = CreateCollectionType(collectionType, itemType); // Nie unbound type

            return Activator.CreateInstance(closedType);
        }

        public static Type CreateCollectionType(Type collectionType, Type itemType) => collectionType.MakeGenericType(itemType);
    }
}
