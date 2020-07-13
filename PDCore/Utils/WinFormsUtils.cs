using PDCore.Extensions;
using PDCore.Helpers;
using PDCore.Helpers.Calculation;
using System;
using System.Collections.Generic;
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
                MessageBox.Show(ex.Message, "Błąd");
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

        public static bool ShowQuestion(string content, string title = "Uwaga")
        {
            DialogResult dialogResult = MessageBox.Show(content, title, MessageBoxButtons.YesNo);

            bool approved = dialogResult == DialogResult.Yes;

            return approved;
        }
    }
}
