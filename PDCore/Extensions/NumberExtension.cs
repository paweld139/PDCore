using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Extensions
{
    public static class NumberExtension
    {
        /// <summary>
        /// Sprawdzenie czy dana liczba jest liczbą pierwszą
        /// </summary>
        /// <param name="number">Liczba do sprawdzenia</param>
        /// <returns>Informacja czy dana liczba jest liczbą pierwszą</returns>
        public static bool IsPrime(this int number)
        {
            //Liczba pierwsza dzieli się jedynie przez jeden i samą siebie. Jeśli dzieli się jeszcze przez coś, to nie jest liczbą pierwszą.

            bool result = true; //Początkowo zakłada się, że dana liczba jest liczbą pierwszą

            for (long i = 2; i < number; i++) //Dla każdej liczby mniejszej od zadanej liczby i większej od dwa
            {
                if (number % i == 0) //Sprawdzenie czy zadana liczba jest podzielna przez aktualną liczbę z pętli
                {
                    result = false; //Jeśli tak, to zadana liczba nie jest liczbą pierwsza. Rezultat to nieprawda, następuje przewanie pętli.

                    break;
                }
            }
            //Każda liczba oprócza zera jest podzielna przez 1 i przez samą siebie. W przypadku 0, 1, 2 pętla nawet się nie zacznie.
            //Jeśli pętla zakończy się w całości, to oznacza że liczba jest liczbą pierwszą

            return result; //Zwrócenie rezultatu
        }

        public static double SampledAverageDouble(this double[] numbers)
        {
            var count = 0;
            var sum = 0.0;

            for (int i = 0; i < numbers.Length; i += 2)
            {
                sum += numbers[i];
                count++;
            }

            return sum / count;
        }

        public static T SampledAverage<T>(this T[] numbers) where T : struct, IComparable
        {
            int count = 0;
            dynamic sum = default(T);

            try
            {
                for (int i = 0; i < numbers.Length; i += 2)
                {
                    sum += numbers[i];
                    count++;
                }

                return sum / count;
            }
            catch (RuntimeBinderException)
            {
                return sum;
            }
        }

        public static T Multiply<T>(this T multiplicand, int multiplier) where T : struct, IComparable // Mnożna i mnożnik
        {
            T val = default;

            try
            {
                val = (dynamic)multiplicand * multiplier;
            }
            catch (RuntimeBinderException)
            {

            }

            return val;
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
    }
}
