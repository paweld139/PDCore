using PDCore.Extensions;
using PDCore.Interfaces;
using PDCoreNew.Helpers.DataLoaders;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PDCoreNew.Utils
{
    public static class IOUtils
    {
        public static async Task WriteAllBytesAsync(string path, byte[] data)
        {
            using (FileStream SourceStream = File.Open(path, FileMode.OpenOrCreate))
            {
                SourceStream.Seek(0, SeekOrigin.End);

                await SourceStream.WriteAsync(data, 0, data.Length);
            }
        }

        public static async Task<byte[]> ReadAllBytesAsync(string path)
        {
            byte[] result;

            using (FileStream stream = File.Open(path, FileMode.Open))
            {
                result = new byte[stream.Length];

                await stream.ReadAsync(result, 0, (int)stream.Length);
            }

            return result;
        }

        public static async Task<string> ReadAllTextAsync(string path)
        {
            string result;

            using (StreamReader stream = File.OpenText(path))
            {
                result = await stream.ReadToEndAsync();
            }

            return result;
        }

        /// <summary>
        /// This is the same default buffer size as
        /// <see cref="StreamReader"/> and <see cref="FileStream"/>.
        /// </summary>
        private const int DefaultBufferSize = 4096;

        /// <summary>
        /// Indicates that
        /// 1. The file is to be used for asynchronous reading.
        /// 2. The file is to be accessed sequentially from beginning to end.
        /// </summary>
        private const FileOptions DefaultOptions = FileOptions.Asynchronous | FileOptions.SequentialScan;

        public static Task<string[]> ReadAllLinesAsync(string path)
        {
            return ReadAllLinesAsync(path, Encoding.UTF8);
        }

        public static async Task<string[]> ReadAllLinesAsync(string path, Encoding encoding)
        {
            var lines = new List<string>();

            // Open the FileStream with the same FileMode, FileAccess
            // and FileShare as a call to File.OpenText would've done.
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, DefaultBufferSize, DefaultOptions))
            using (var reader = new StreamReader(stream, encoding))
            {
                string line;
                while ((line = await reader.ReadLineAsync()) != null)
                {
                    lines.Add(line);
                }
            }

            return lines.ToArray();
        }

        public static Task RemoveFileAsync(string path)
        {
            return Task.Run(() => { File.Delete(path); });
        }

        public const string DatabaseConnectionError = "Wystąpił błąd podczas łączenia z bazą {0}. Błąd: {1}";

        public static string CheckDatabaseStatus(params DbContext[] dbContexts)
        {
            List<string> statuses = new List<string>();

            foreach (var context in dbContexts)
            {
                var cnn = context.Database.Connection;

                try
                {
                    cnn.Open();
                }
                catch (Exception ex)
                {
                    statuses.Add(string.Format(DatabaseConnectionError, cnn.Database, ex.Message));
                }
                finally
                {
                    if (cnn.State != System.Data.ConnectionState.Closed)
                    {
                        cnn.Close();
                    }
                }
            }

            return string.Join(Environment.NewLine, statuses.ToArray());
        }

        public static IObservable<TProperty> ObservePropertyChanges<TProperty>(Expression<Func<TProperty>> property, object sender)
        {
            PropertyInfo propertyInfo = (PropertyInfo)((MemberExpression)property.Body).Member;

            return Observable
                .FromEventPattern(sender, "PropertyChanged")
                .Where(prop => ((PropertyChangedEventArgs)prop.EventArgs).PropertyName == propertyInfo.Name)
                .Select(x => propertyInfo.GetValue(sender, null))
                .DistinctUntilChanged()
                .Cast<TProperty>();
        }

        public static IDataLoader GetLoaderFor(string source)
        {
            IDataLoader loader;

            if (source.IsUrl())
            {
                loader = new WebLoader(source);
            }
            else
            {
                loader = new LocalLoader(source);
            }

            return loader;
        }
    }
}
