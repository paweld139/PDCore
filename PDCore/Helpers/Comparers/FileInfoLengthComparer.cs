using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PDCore.Helpers.Comparers
{
    public class FileInfoLengthComparer : IComparer<FileInfo>
    {
        public int Compare(FileInfo x, FileInfo y)
        {
            return y.Length.CompareTo(x.Length);
        }
    }
}
