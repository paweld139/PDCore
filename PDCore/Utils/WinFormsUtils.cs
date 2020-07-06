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
    }
}
