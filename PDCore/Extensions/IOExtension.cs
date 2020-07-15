using FTCore.CoreLibrary.AttributeApi;
using PDCore.Context.IContext;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;

namespace PDCore.Extensions
{
    /// <summary>
    /// Statyczna klasa rozszerzająca pomocn przy operacjach wejścia-wyjścia
    /// </summary>
    public static class IOExtension
    {
        /// <summary>
        /// Zwraca tablicę bajtów dla zadanego obiektu Image i formatu zdjęcia
        /// </summary>
        /// <param name="image">Zdjęcea</param>
        /// <param name="imageFormat">Format zdjęcia</param>
        /// <returns>Tablica bajtów będąca odzwierciedleniem przekazanego zdjęcia i biorąca pod uwagę format</returns>
        public static byte[] GetBuffer(this Image image, ImageFormat imageFormat)
        {
            using (MemoryStream ms = new MemoryStream()) //Utworzenie strumienia danych
            {
                image.Save(ms, imageFormat); //Zapisuje zdjęcie w zadanym formacie do strumienia danych

                byte[] buf = ms.GetBuffer(); //Pobranie tablicy bajtów ze strumienia danych

                return buf; //Zwrócenie tablicy najtów
            }
        }

        /// <summary>
        /// Zwrócenie tablicy bajtów ze strumienia
        /// </summary>
        /// <param name="input">Strumień</param>
        /// <returns>Tablica bajtów ze strumienia</returns>
        public static byte[] ReadFully(this Stream input)
        {
            byte[] buffer = new byte[16 * 1024]; //Utworzenie tablicy bajtów o pojemności ok. 16 KB

            using (MemoryStream ms = new MemoryStream()) //Utworzenie strumienia pamięci
            {
                int read; //Utworzenie zmiennej, która przechowa ilość bajtów wyczytanych ze strumienia

                //Odczytanie bajtów ze strumienia do bufora maksymalnie o wielkości bufora, bez pomijania bajtów, dopóki pozostały bajty do odczytania.
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read); //Zapisanie do strumienia pamięci, bajtów przechowywanych przez bufor
                }

                return ms.ToArray(); //Zwrócenie tablicy bajtów z bufora pamięci
            }
        }

        /// <summary>
        /// Zapisanie zdjęcia o zadanym formacie i nazwie jako pliku tymczasowego
        /// </summary>
        /// <param name="image">Zdjęcie do zapisania</param>
        /// <param name="imageFormat">Format w jakim zdjęcie zostanie zapisane</param>
        /// <param name="name">Nazwa pliku ze zdjęciem do zapisania</param>
        /// <returns>Ścieżka do zapisanego zdjęcia</returns>
        public static string SaveTemp(this Image image, ImageFormat imageFormat, string name)
        {
            string extension = new ImageFormatConverter().ConvertToString(imageFormat); //Przekonwertowanie formatu zdjęcia na rozszerzenie w formie łańcucha znaków

            string fileName = name + "." + extension; //Utworzenie nazwy pliku na podstawie przekazanej nazwy i otrzymanego roszerzenia

            string path = SecurityUtils.TemplateDirPath() + fileName; //Utworzenie ścieżki w jakiej zostanie zapisany plik, na podstawie pobranej ścieżki plików tymczasowych i otrzymanej nazwy pliku

            File.WriteAllBytes(path, image.GetBuffer(imageFormat)); //Pobranie tablicy bajtów dla zadanego zdjęcia i zapisanie zdjęcia w zadanej lokalizacji

            return path; //Zwrócenie ścieżki do zdjęcia
        }

        public static void OpenConnectionIfClosed(this DbConnection dbConnection)
        {
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();
        }

        public static string GetTableName<T>(this IAttributableDbContext context)  where T : Attributable, new()
        {
            string sql = context.GetQuery<T>(string.Empty);

            Regex regex = new Regex("FROM (?<table>.*) AS");
            Match match = regex.Match(sql);

            string table = match.Groups["table"].Value;
            return table;
        }
    }
}
