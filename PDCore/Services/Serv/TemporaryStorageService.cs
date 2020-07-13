using PDCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace PDCore.Services.Serv
{
    public class TemporaryStorageService : ITemporaryStorageService
    {
        public void Deposit<T>(T o, string key)
        {
            Application.Current.Properties[key] = o;
        }

        public T Withdraw<T>(string key)
        {
            T o = (T)Application.Current.Properties[key];

            Application.Current.Properties.Remove(key);

            return o;
        }
    }
}
