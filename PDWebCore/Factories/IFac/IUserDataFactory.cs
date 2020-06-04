using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Factories.IFac
{
    public interface IUserDataFactory
    {
        void Fill(UserDataModel userData, string jsonString, string usersIp);
    }
}
