using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.IO;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Reflection;
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
    }
}
