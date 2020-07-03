using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Instrumentation;
using System.Security.Cryptography;
using System.Text;
using PDCore.Extensions;

namespace PDCore.Helpers
{
    public static class RandomNumberGenerator
    {
        private static readonly Random random;

        /// <summary>
        /// Modyfikator dostępu jest niedozwolony dla statycznych konstruktorów
        /// </summary>
        static RandomNumberGenerator()
        {
            random = new Random();
        }

        public static int[] Next(int from, int to, int instances)
        {
            if (instances <= 0)
                throw new ArgumentOutOfRangeException(ObjectUtils.GetNameOf(() => instances), instances, "Wartość \"od\" musi być większa od zera");

            if (from > to)
                throw new ArgumentOutOfRangeException(ObjectUtils.GetNameOf(() => from), from, "Wartość \"od\" nie może być większa od wartości \"do\"");

            int[] result = new int[instances];

            for (int i = 0; i < instances; i++)
            {
                result[i] = random.Next(from, to + 1);
            }

            return result;

            //return Enumerable.Range(0, instances).ToArray(x => random.Next(from, to + 1));
        }
    }
}
