using PDCore.Helpers;
using PDCore.Repositories.IRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDCore.Utils
{
    public static class RepositoryUtils
    {
        public static void DumpItems<T>(ISqlRepositoryEntityFramework<T> repository, Action<T> print = null) where T : class
        {
            if (print == null)
                print = i => Console.WriteLine(i);

            var result = repository.FindAll();

            foreach (var item in result)
            {
                print(item);
            }
        }

        //Out - bardziej szczegółowe typy zostaną zwrócone jako mniej szczegółowe
        public static void DumpNamedObject(ISqlRepositoryEntityFramework<NamedObject> repository, Action<NamedObject> print = null)
        {
            if (print == null)
                print = i => Console.WriteLine(i.Name);

            DumpItems(repository, print);
        }
    }
}
