using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace PDCore.Extensions
{
    public static class ReflectionExtension
    {
        public static IEnumerable<string> GetPropertyNames(this PropertyInfo[] propertyInfos)
        {
            return propertyInfos.Select(p => p.Name);
        }

        public static object GetPropertyValue<T>(this PropertyInfo propertyInfo, T entity)
        {
            return propertyInfo.GetValue(entity, null);
        }

        public static string GetPropertyValueString<T>(this PropertyInfo propertyInfo, T entity)
        {
            return propertyInfo.GetPropertyValue(entity).EmptyIfNull();
        }

        public static IEnumerable<object> GetPropertyValues<T>(this PropertyInfo[] propertyInfos, T entity)
        {
            return propertyInfos.Select(p => p.GetPropertyValue(entity));
        }

        public static IEnumerable<string> GetPropertyValuesString<T>(this PropertyInfo[] propertyInfos, T entity)
        {
            return propertyInfos.GetPropertyValues(entity).EmptyIfNull();
        }

        public static string GetTypeName(this Type type)
        {
            StringBuilder typeName = new StringBuilder(type.Name);

            if (type.IsGenericType)
                type.GetGenericArguments().ForEach(a => typeName.AppendFormat("[{0}]", a.Name));

            return typeName.ToString();
        }

        public static Type GetType(this TypeCode code)
        {
            return Type.GetType("System." + EnumUtils.GetEnumName<TypeCode>(code));
        }

        public static bool IsNumericDatatype(this object obj)
        {
            switch (obj.GetTypeCode())
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.Decimal:
                case TypeCode.Double:
                case TypeCode.Single:
                    return true;
                default:
                    return false;
            }
        }

        public static string GetNameOf<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
        {
            _ = obj;

            return ReflectionUtils.GetNameOf(propertyAccessor.Body);
        }

        public static string GetName<T>(this T obj, Expression<Func<T>> accessor)
        {
            _ = obj;

            return ReflectionUtils.GetNameOf(accessor.Body);
        }

        public static TypeCode GetTypeCode(this object obj)
        {
            return obj.GetType().GetTypeCode();
        }

        public static TypeCode GetTypeCode(this Type type)
        {
            return Type.GetTypeCode(type);
        }

        public static bool IsEnum<TEnum>(this object obj)
        {
            Type enumType = typeof(TEnum);

            TypeCode enumTypeCode = enumType.GetTypeCode(); //Typ numeru enuma

            if (enumTypeCode != obj.GetTypeCode())
                obj = obj.ConvertObject(enumTypeCode);

            return Enum.IsDefined(enumType, obj);
        }

        public static IEnumerable<Type> GetImmediateInterfaces(this Type type)
        {
            var interfaces = type.GetInterfaces();

            var result = new HashSet<Type>(interfaces);

            foreach (Type i in interfaces)
                result.ExceptWith(i.GetInterfaces());

            return result;
        }

        public static bool ImplementsInterface<TInterface>(this Type type) => typeof(TInterface).IsAssignableFrom(type);

        public static bool ImplementsInterface<TInterface>(this object input)
        {
            return input.GetType().ImplementsInterface<TInterface>();
        }

        /// <summary>
        /// Determine whether a type is simple (String, Decimal, DateTime, etc) 
        /// or complex (i.e. custom class with public properties and methods).
        /// </summary>
        /// <see cref="http://stackoverflow.com/questions/2442534/how-to-test-if-type-is-primitive"/>
        public static bool IsSimpleType(
           this Type type)
        {
            return
               type.IsValueType ||
               type.IsPrimitive ||
               new[]
               {
               typeof(string),
               typeof(decimal),
               typeof(DateTime),
               typeof(DateTimeOffset),
               typeof(TimeSpan),
               typeof(Guid)
               }.Contains(type) ||
               (Convert.GetTypeCode(type) != TypeCode.Object);
        }

        public static Type GetUnderlyingType(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Event:
                    return ((EventInfo)member).EventHandlerType;
                case MemberTypes.Field:
                    return ((FieldInfo)member).FieldType;
                case MemberTypes.Method:
                    return ((MethodInfo)member).ReturnType;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).PropertyType;
                default:
                    throw new ArgumentException
                    (
                       "Input MemberInfo must be if type EventInfo, FieldInfo, MethodInfo, or PropertyInfo"
                    );
            }
        }

        public static bool PublicInstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
        {
            if (self != null && to != null)
            {
                var type = typeof(T);
                var ignoreList = new List<string>(ignore);
                var unequalProperties =
                    from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    where !ignoreList.Contains(pi.Name) && pi.GetUnderlyingType().IsSimpleType() && pi.GetIndexParameters().Length == 0
                    let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
                    let toValue = type.GetProperty(pi.Name).GetValue(to, null)
                    where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
                    select selfValue;
                return !unequalProperties.Any();
            }
            return self == to;
        }
    }
}
