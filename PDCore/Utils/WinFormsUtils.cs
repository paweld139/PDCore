using PDCore.Extensions;
using PDCore.Helpers;
using PDCore.Helpers.Calculation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDCore.Utils
{
    public static class WinFormsUtils
    {
        public static int[] GetRandomNumbers(NumericUpDown fromNumericUpDown, NumericUpDown toNumericUpDown, NumericUpDown amountNumericUpDown)
        {
            int from = fromNumericUpDown.GetValueInt();

            int to = toNumericUpDown.GetValueInt();

            int amount = amountNumericUpDown.GetValueInt();


            int[] result = null;

            try
            {
                result = RandomNumberGenerator.Next(from, to, amount);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                ShowError(ex);
            }

            return result;
        }

        public static void SetMinAndMaxAsInt(params NumericUpDown[] numericUpDown)
        {
            numericUpDown.ForEach(x => x.SetMinAndMaxAsInt());
        }

        public static void ShowMessage(string content, string title)
        {
            MessageBox.Show(content, title);
        }

        public static void ShowError(string content)
        {
            ShowMessage(content, "Uwaga");
        }

        public static void ShowInformation(string content)
        {
            ShowMessage(content, "Informacja");
        }

        public static void ShowError(string content, Exception exception)
        {
            string message = string.Format("{0}{1}{1}{2}{1}{1}{3}", content, Environment.NewLine, exception.Message, exception.StackTrace);

            ShowError(message);
        }

        public static void ShowError(Exception exception)
        {
            ShowError(exception.Message);
        }

        public static bool ShowQuestion(string content, string title = "Uwaga")
        {
            DialogResult dialogResult = MessageBox.Show(content, title, MessageBoxButtons.YesNo);

            bool approved = dialogResult == DialogResult.Yes;

            return approved;
        }

        public static string[] OpenFiles(string title = "Otwórz", string filter = null, int filesCount = 1)
        {
            if (filesCount < 1)
                throw new ArgumentOutOfRangeException(nameof(filesCount), filesCount, "Podano nieprawidłową ilość plików do wybrania. Minimum to 1.");

            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.RestoreDirectory = true;

                openFileDialog.Title = title; //"Otwórz pliki do importu (rejestr RPL i SORL)";

                openFileDialog.Filter = filter; //"XML Files|*.xml";

                openFileDialog.Multiselect = filesCount > 1; //true;


                if (openFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
                {
                    int selectedFilesCount = openFileDialog.FileNames.Length;

                    if (selectedFilesCount == filesCount)
                    {
                        return openFileDialog.FileNames;
                    }
                    else
                    {
                        throw new InvalidOperationException($"Oczekiwano wyboru {filesCount} plików, a wybrano {selectedFilesCount}.");
                    }
                }

                return null;
            }
        }

        public static Tuple<string, string> OpenTextFile()
        {
            string[] fileNames;

            try
            {
                fileNames = OpenFiles("Otwórz plik tekstowy", "Text |*.txt");
            }
            catch (Exception ex)
            {
                if (ex is InvalidOperationException || ex is ArgumentOutOfRangeException)
                {
                    ShowError(ex);
                }

                throw;
            }

            if (fileNames != null) //Wybrano pliki i nie wystąpił wyjątek
            {
                string fileName = fileNames[0];

                string text = File.ReadAllText(fileName, Encoding.UTF8);


                return Tuple.Create(text, fileName);
            }

            return null;
        }
    }
}
