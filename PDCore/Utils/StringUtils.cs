using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Utils
{
    public static class StringUtils
    {
        public const string ResultFormat = "{0}: {1}";

        public const string Separator = "***";

        public static string ZeroFix(string element)
        {
            if (element != null && element.Length == 1)
            {
                return "0" + element;
            }

            return element;
        }

        public static string ZeroFixReversed(string element)
        {
            if(element != null && element.Length == 2 && element[0] == '0')
            {
                return element[1].ToString();
            }

            return element;
        }

        public static bool AreNullOrWhiteSpace(params string[] results)
        {
            return AreOrNotNullOrWhiteSpace(true, results);
        }

        public static bool AreNotNullOrWhiteSpace(params string[] results)
        {
            return AreOrNotNullOrWhiteSpace(false, results);
        }

        private static bool AreOrNotNullOrWhiteSpace(bool indicator = false, params string[] results)
        {
            bool result = true;

            foreach (string item in results)
            {
                result &= (string.IsNullOrWhiteSpace(item) == indicator);

                if (!result)
                {
                    break;
                }
            }

            return result;
        }
    }
}
