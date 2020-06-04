using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDWebCore.Services.IServ
{
    public interface IUserDataService
    {
        Task<UserDataModel> GetAsync();

        Task SaveAsync();
    }
}
