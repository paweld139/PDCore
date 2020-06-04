using PDCore.Extensions;
using PDCore.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace PDCore.Helpers
{
    public class ImportHelper
    {
        private readonly string filePath;
        private readonly string defaultFilePath;

        public ImportHelper(string filePath, string defaultFilePath)
        {
            this.filePath = filePath;
            this.defaultFilePath = defaultFilePath;
        }

        public StreamReader GetFileStreamForImport(string importFileName)
        {
            return SecurityUtils.GetAssemblyStreamReaderByPath(MakePath(importFileName, filePath));
        }

        private static void SaveFileWithSaveDialog(string path)
        {
            Stream st = SecurityUtils.GetAssemblyStreamByPath(path);

            if (st == null)
            {
                return;
            }


            byte[] file = st.ReadFully();


            SaveFileDialog saveFileDialog = new SaveFileDialog();

            saveFileDialog.Filter = "CSV Files|*.csv";

            saveFileDialog.Title = "Zapisz plik";


            string[] pathSplit = path.Split('.');

            int fileNameIndex = pathSplit.Length - 2;

            saveFileDialog.FileName = pathSplit[fileNameIndex];


            if (saveFileDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                File.WriteAllBytes(saveFileDialog.FileName, file);
            }

            st.Close();
        }

        private static string MakePath(string fileName, string path)
        {
            return path + "." + fileName + ".csv";
        }

        public void SaveDefaultFile(string importFileName)
        {
            SaveFileWithSaveDialog(MakePath(importFileName, defaultFilePath));
        }

        public void SaveFile(string importFileName)
        {
            SaveFileWithSaveDialog(MakePath(importFileName, filePath));
        }
    }
}
