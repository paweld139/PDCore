using PDCore.Helpers;
using PDCore.Interfaces;
using PDCore.Repositories.IRepo;
using System;

namespace PDCore.Utils
{
    public static class RepositoryUtils
    {
        public static void DumpItems<T>(IReadOnlyRepository<T> repository, Action<T> print = null) where T : class
        {
            if (print == null)
                print = Console.WriteLine;

            var result = repository.FindAll();

            foreach (var item in result)
            {
                print(item);
            }
        }

        //Out - bardziej szczegółowe typy zostaną zwrócone jako mniej szczegółowe
        public static void DumpNamedObject(IReadOnlyRepository<NamedObject> repository, Action<NamedObject> print = null)
        {
            if (print == null)
                print = i => Console.WriteLine(i.Name);

            DumpItems(repository, print);
        }

        public static void SetLogging(bool input, ILogger logger, bool isLoggingEnabled, Action enableLogging, Action disableLogging)
        {
            if (input == isLoggingEnabled || logger == null)
            {
                return;
            }

            if (input)
                enableLogging();
            else
                disableLogging();
        }
    }
}
