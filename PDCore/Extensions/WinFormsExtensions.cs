using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDCore.Extensions
{
    public static class WinFormsExtensions
    {
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

        public static void SetItemsTextToClipboard(this ListBox listBox)
        {
            if (listBox.HasItems()) //Schowek ulegnie zmianie tylko wtedy, gdy istnieją elementy
                Clipboard.SetText(listBox.GetItemsText());
        }

        public static void AddItems<TItem>(this ListBox listBox, TItem[] items, bool clearItemsBeforeAdd = true, bool throwIfNull = false)
        {
            if (clearItemsBeforeAdd)
                listBox.ClearItemsIfExist();

            if (items == null)
            {
                if (throwIfNull)
                    throw new ArgumentException("Nie przekazano elementów do dodania.", ObjectUtils.GetNameOf(() => items));
                else
                    return;
            }

            items.ForEach(x => listBox.Items.Add(x));
        }

        public static void SetMinAndMaxAsInt(this NumericUpDown numericUpDown)
        {
            numericUpDown.SetMinAsInt();
            numericUpDown.SetMaxAsInt();
        }

        public static void SetMinAsInt(this NumericUpDown numericUpDown)
        {
            numericUpDown.Minimum = int.MinValue;
        }

        public static void SetMaxAsInt(this NumericUpDown numericUpDown)
        {
            numericUpDown.Maximum = int.MaxValue;
        }
    }
}
