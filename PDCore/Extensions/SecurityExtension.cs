using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Windows.Forms;

namespace PDCore.Extensions
{
    public static class SecurityExtension
    {
        public static long NextLong(this Random rnd)
        {
            byte[] buffer = new byte[8];

            rnd.NextBytes(buffer);

            return BitConverter.ToInt64(buffer, 0);
        }

        public static string MD5Hex(this string content)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = MD5.Create();

            byte[] inputBytes = Encoding.UTF8.GetBytes(content);

            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return sb.ToString();
        }

        public static void InvokeIfRequired(this ISynchronizeInvoke obj, MethodInvoker action)
        {
            if (obj.InvokeRequired)
            {
                var args = new object[0];

                obj.Invoke(action, args);
            }
            else
            {
                action();
            }
        }

        public static string GetInfo(this X509Certificate2 x509Certificate2)
        {
            StringBuilder info = new StringBuilder("Numer seryjny: ");

            info.Append(x509Certificate2.SerialNumber);

            info.AppendLine();

            info.AppendFormat("Okres ważności: {0} - {1}", x509Certificate2.NotBefore.ToYMD(), x509Certificate2.NotAfter.ToYMD());


            return info.ToString();
        }

        public static bool IsExpired(this X509Certificate2 x509Certificate2)
        {
            DateTime dateTimeNow = DateTime.Now;

            if (x509Certificate2.NotAfter <= dateTimeNow)
                return true;

            return false;
        }
    }
}
