using Microsoft.CSharp.RuntimeBinder;
using PDCore.Helpers;
using PDCore.Helpers.Soap.ExceptionHandling;
using PDCore.Helpers.Wrappers.DisposableWrapper;
using PDCore.Interfaces;
using PDCore.Models;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ServiceModel;
using System.Text;
using System.Windows.Forms;

namespace PDCore.Extensions
{
    public static class ObjectExtension
    {
        // core "just dispose it without barfing"
        public static IDisposableWrapper<T> Wrap<T>(this T baseObject) where T : class, IDisposable
        {
            if (baseObject is IDisposableWrapper<T> wrapper)
            {
                return wrapper;
            }

            return new DisposableWrapper<T>(baseObject);
        }

        // specific handling for service-model
        public static IDisposableWrapper<TProxy> Wrap<TProxy, TService>(
            this TProxy proxy)
            where TProxy : ClientBase<TService>
            where TService : class
        {
            return new ClientWrapper<TProxy, TService>(proxy);
        }

        //public static void ThrowIfNull(this object obj)
        //{
        //    if (obj == null)
        //        throw new Exception();
        //}

        public static void ThrowIfNull<T>(this T obj, string objName)
        {
            if (obj == null)
                throw new ArgumentNullException(objName);
        }

        public static void ThrowIfNull<T>(this T obj)
        {
            obj.ThrowIfNull(nameof(obj));
        }

        public static void SwapValues<T>(this T[] source, long index1, long index2)
        {
            ObjectUtils.SwapValues(ref source[index1], ref source[index2]);
        }

        public static void SwapValues<T>(this T[] source)
        {
            if (source.Length < 2)
                throw new ArgumentException("Tablica musi zawierać co najmniej dwa elementy");

            source.SwapValues(0, 1);
        }

        public static T[] GetRow<T>(this T[,] array, int row)
        {
            if (!typeof(T).IsPrimitive)
                throw new InvalidOperationException("Not supported for managed types.");

            if (array == null)
                throw new ArgumentNullException("array");

            int cols = array.GetUpperBound(1) + 1;
            T[] result = new T[cols];

            int size;

            if (typeof(T) == typeof(bool))
                size = 1;
            else if (typeof(T) == typeof(char))
                size = 2;
            else
                size = Marshal.SizeOf(typeof(T));

            Buffer.BlockCopy(array, row * cols * size, result, 0, cols * size);

            return result;
        }

        public static Tuple<TimeSpan, TResult> Time<T, TResult>(this Stopwatch sw, Func<T, TResult> func, T param, int iterations = 1)
        {
            sw.Reset();

            sw.Start();

            TResult result = default;

            for (int i = 0; i < iterations; i++)
            {
                result = func(param);
            }

            sw.Stop();

            return new Tuple<TimeSpan, TResult>(sw.Elapsed, result);
        }

        public static Tuple<TimeSpan, T> Time<T>(this Stopwatch sw, Func<T> func, int iterations = 1)
        {
            return sw.Time(f => func(), true, iterations);
        }

        public static long TimeMillis(this Stopwatch sw, Action action, int iterations = 1)
        {
            var elapsed = sw.Time(p => { action(); return true; }, true, iterations);

            return elapsed.Item1.Milliseconds;
        }

        public static IDisposableWrapper<DisposableStopwatch> WrapStopwatch(this DisposableStopwatch disposableStopwatch)
        {
            return new StopWatchDisposableWrapper(disposableStopwatch);
        }

        public static TOutput ConvertOrCastTo<TInput, TOutput>(this TInput input, Converter<TInput, TOutput> converter = null)
        {
            if (input is TOutput output)
                return output;

            if (converter != null)
                return converter(input);

            //var simpleConverter = TypeDescriptor.GetConverter(typeof(TInput));

            return input.ConvertObject<TOutput>();
        }

        public static bool TryConvertOrCastTo<TInput, TOutput>(this TInput input, out TOutput output, Converter<TInput, TOutput> converter = null)
        {
            try
            {
                output = input.ConvertOrCastTo(converter);

                return true;
            }
            catch
            {
                output = default;
            }

            return false;
        }

        /// <summary>
        /// Pobierz opis dla danej wartości enuma
        /// </summary>
        /// <typeparam name="T">Typ enuma</typeparam>
        /// <param name="enumerationValue">Wartość enuma</param>
        /// <returns>Opis dla danej wartości enuma z atrybutu Description</returns>
        public static string GetDescription<T>(this T enumerationValue) where T : struct //Typ musi być typem wartościowym, enumem
        {
            Type type = enumerationValue.GetType(); //Pobranie typu przekazanej wartości enuma

            if (!type.IsEnum) //Jeśli nie jest to enum
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", "enumerationValue"); //Problem z argumentem, więc odpowiedni wyjątek
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            MemberInfo[] memberInfo = type.GetMember(enumerationValue.ToString()); //Pobranie wartości enuma jako członka po nazwie jego typu

            if (memberInfo != null && memberInfo.Length > 0) //Jeśli członek istnieje
            {
                object[] attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false); //Pobranie atrybutu dla cżłonka

                if (attrs != null && attrs.Length > 0) //Jeżeli znaleziono atrybut
                {
                    //Pull out the description value
                    return ((DescriptionAttribute)attrs[0]).Description; //Pobranie wartości z atrybutu
                }
            }

            //If we have no description attribute, just return the ToString of the enum
            return enumerationValue.ToString();
        }

        public static T CastObject<T>(this object input)
        {
            return (T)input;
        }

        public static T ConvertObject<T>(this object input)
        {
            return (T)input.ConvertObject(typeof(T));
        }

        public static object ConvertObject(this object input, Type outputType)
        {
            return input.ConvertObject(outputType.GetTypeCode());
        }

        public static object ConvertObject(this object input, TypeCode outputTypeCode)
        {
            return Convert.ChangeType(input, outputTypeCode);
        }

        public static string EmptyIfNull<T>(this T value)
        {
            if (value == null)
                return string.Empty;

            return value.ToString();
        }

        public static Tuple<T1, object> GetTuple<T1>(this object item2, T1 item1)
        {
            return Tuple.Create(item1, item2);
        }

        public static int GetEnumNumber<TEnum>(this TEnum value) where TEnum : struct
        {
            return value.ConvertOrCastTo<TEnum, int>();
        }

        public static string ToNumberString<T>(this T value, int precision)
        {
            return value.ToString().ToNumberString(precision);
        }

        public static bool ValueIn<TInput>(this TInput input, params TInput[] values) where TInput : IEquatable<TInput>
        {
            return values.Any(v => v.Equals(input));
        }

        public static bool ArePropertiesNotNull<T>(this T obj)
        {
            if (obj == null)
                return false;

            return PropertyCache<T>.PublicProperties.All(propertyInfo => propertyInfo.GetPropertyValue(obj) != null);
        }

        public static string GetUserName(this SmtpClient smtpClient) => (smtpClient.Credentials as NetworkCredential)?.UserName;

        public static ConsoleColor ToConsoleColor(this Color color)
        {
            return color.Name.ParseEnum<ConsoleColor>();
        }

        public static bool IsNew(this IModificationHistory history) => history.DateCreated == DateTime.MinValue;

        public static RowInfo GetRowInfo(this IModificationHistory modificationHistory) => new RowInfo(modificationHistory);
    }
}
