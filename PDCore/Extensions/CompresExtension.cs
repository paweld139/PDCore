using FTCore.CoreLibrary;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace PDCore.Extensions
{
    /// <summary>
    /// Statyczna klasa rozszerzająca zawierająca metody umożliwiające kompresowanie i dekompresowanie danych
    /// </summary>
    public static partial class CompresExtension
    {
        /// <summary>
        /// Kompresowanie łańcucha znaków
        /// </summary>
        /// <param name="s">Łańcuch znaków do skompresowania</param>
        /// <returns>Skompresowny łańcuch znaków</returns>
        public static string Compress(this string s)
        {
            byte[] bytesToEncode = Encoding.UTF8.GetBytes(s); //Zamiana łańcucha znaków do skompresowania na tablicę bajtów z wykorzystaniem kodowania znaków UTF-8

            return Convert.ToBase64String(bytesToEncode.Compress()); //Skompresowanie i zwrócenie tablicy bajtów w formie łańcucha znaków, który jest zakodowany cyframi 64-bitowymi
        }

        /// <summary>
        /// Kompresowanie tablicy bajtów
        /// </summary>
        /// <param name="bytesToEncode">Tablica bajtów do skompresowania</param>
        /// <returns>Skompresowana tablica bajtów</returns>
        public static byte[] Compress(this byte[] bytesToEncode)
        {
            using (MemoryStream input = new MemoryStream(bytesToEncode)) //Utworzenie strumienia pamięci na podstawie przekazanej tablicy bajtów
            {
                using (MemoryStream output = new MemoryStream()) //Utworzenie nowego strumienia danych (zostanie zniszczony po zakończeniu wywoływania kodu w obrębie bloku "using")
                {
                    //Utworzenie strumienia GZip służącego do kompresji i dekompresji strumieni. Zostaje przekazane wyjście danych, jak i metoda kompresji, czyli dane zostaną skompresowne.
                    using (GZipStream zip = new GZipStream(output, CompressionMode.Compress))
                    {
                        input.CopyTo(zip); //Odczytuje bajty z wejściowego strumienia pamięci i kopiuje je do strumienia GZip celem skompresowania danych
                    }

                    return output.ToArray(); //Zwrócenie skompresowanej tablicy bajtów
                }
            }
        }

        /// <summary>
        /// Dekompresowanie łańcucha znaków
        /// </summary>
        /// <param name="s">Łańcuch znaków do zdekompresowania</param>
        /// <returns>Zdekompresowany łańcuch znaków</returns>
        public static string Explode(this string s)
        {
            byte[] compressedBytes = Convert.FromBase64String(s); //Zamiana łańcucha znaków do zdekompresowania na tablicę bajtów z wykorzystaniem kodowania znaków UTF-8

            return Encoding.UTF8.GetString(compressedBytes.Explode()); //Zdekompresowanie tablicy bajtów i zamiana jej w łańcuch znaków z wykorzystaniem kodowania znaków UTF-8 i zwrócenie
        }

        /// <summary>
        /// Dekompresowanie tablicy bajtów
        /// </summary>
        /// <param name="compressedBytes">Tablica bajtów do zdekompresowania</param>
        /// <returns></returns>
        public static byte[] Explode(this byte[] compressedBytes)
        {
            using (MemoryStream input = new MemoryStream(compressedBytes)) //Utworzenie strumienia pamięci na podstawie przekazanej tablicy bajtów
            {
                using (MemoryStream output = new MemoryStream()) //Utworzenie nowego strumienia danych (zostanie zniszczony po zakończeniu wywoływania kodu w obrębie bloku "using")
                {
                    //Utworzenie strumienia GZip służącego do kompresji i dekompresji strumieni. Zostaje przekazane wejście danych, jak i metoda kompresji, czyli dane zostaną zdekompresowne.
                    using (GZipStream zip = new GZipStream(input, CompressionMode.Decompress))
                    {
                        zip.CopyTo(output); //Odczytuje zdekompresowaną tablicę bajtów ze strumienia GZip i kopiuje ją do wyjściowego steumienia pamięci
                    }

                    return output.ToArray(); //Zwrócenie zdekompresowanej tablicy bajtów
                }
            }
        }
    }
}
