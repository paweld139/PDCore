using PDCore.Helpers;
using PDCore.Helpers.Soap.ExceptionHandling;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
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
            if (baseObject is IDisposableWrapper<T>)
            {
                return (IDisposableWrapper<T>)baseObject;
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

        public static void ThrowIfNull(this object obj, string objName)
        {
            if (obj == null)
                throw new Exception(string.Format("{0} is null.", objName));
        }

        public static void ThrowIfNull(this object obj)
        {
            if (obj == null)
                throw new Exception(string.Format("obj is null."));
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

        public static bool IsEnum<T>(this object obj)
        {
            return obj.IsNumericDatatype() && Enum.IsDefined(typeof(T), obj);
        }

        public static bool IsNumericDatatype(this object obj)
        {
            switch (Type.GetTypeCode(obj.GetType()))
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

        public static int Power(this int x)
        {
            return x * x;
        }

        public static int Power(this int x, uint pow)
        {
            int ret = 1;

            while (pow != 0)
            {
                if ((pow & 1) == 1)
                    ret *= x;

                x *= x;
                pow >>= 1;
            }

            return ret;
        }

        public static string GetNameOf<T, TT>(this T obj, Expression<Func<T, TT>> propertyAccessor)
        {
            return ObjectUtils.GetNameOf(propertyAccessor.Body);
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

        public static long Time(this Stopwatch sw, Action action, int iterations = 1)
        {
            sw.Reset();

            sw.Start();

            for (int i = 0; i < iterations; i++)
            {
                action();
            }

            sw.Stop();

            return sw.ElapsedMilliseconds;
        }

        public static IDisposableWrapper<DisposableStopwatch> WrapStopwatch(this DisposableStopwatch disposableStopwatch)
        {
            return new StopWatchWrapper(disposableStopwatch);
        }

        public static int GetValueInt(this NumericUpDown numericUpDown) => Convert.ToInt32(Math.Round(numericUpDown.Value, 0));

        public static bool HasItems(this ListBox listBox)
        {
            return listBox.Items.Count > 0;
        }

        public static void ClearItemsIfExist(this ListBox listBox)
        {
            if (listBox.HasItems())
                listBox.Items.Clear();
        }

        public static IEnumerable<object> AsEnumerable(this ListBox.ObjectCollection objectCollection)
        {
            return objectCollection.Cast<object>();
        }

        public static IEnumerable<object> AsEnumerable(this ListBox listBox)
        {
            return listBox.Items.AsEnumerable();
        }

        public static string GetItemsText(this ListBox listBox)
        {
            if (listBox.HasItems())
                return string.Join(", ", listBox.AsEnumerable());

            return string.Empty;
        }
    }
}
