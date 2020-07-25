using PDCore.Repositories.IRepo;
using PDCoreNew.Utils;
using PDWebCore.Factories.IFac;
using PDWebCore.Models;
using PDWebCore.Services.IServ;
using System.Threading.Tasks;
using PDCoreNew.Extensions;
using PDCore.Extensions;
using System;

namespace PDWebCore.Services.Serv
{
    public class UserDataService : IUserDataService
    {
        private const string API_URL_FORMAT = "http://api.ipstack.com/{0}?access_key=33787ab45622e1c767d6857e593df627";

        //private readonly WebClient webClient;
        private readonly IUserDataFactory userDataFactory;
        private readonly ISqlRepositoryEntityFrameworkDisconnected<UserDataModel> userDataRepo;

        public UserDataService(IUserDataFactory userDataFactory, ISqlRepositoryEntityFrameworkDisconnected<UserDataModel> userDataRepo)
        {
            this.userDataFactory = userDataFactory;
            this.userDataRepo = userDataRepo;
        }

        public async Task SaveAsync()
        {
            var userData = await GetAsync();

            await userDataRepo.SaveNewAsync(userData);
        }

        public async Task<UserDataModel> GetAsync()
        {
            UserDataModel userData = new UserDataModel();

            string usersIp = Utils.GetIPAddress();

            if (string.IsNullOrEmpty(usersIp))
            {
                return userData;
            }

            string apiUrl = string.Format(API_URL_FORMAT, usersIp);

            string jsonString = string.Empty;


            Func<string, Task<string>> func = WebUtils.GetTextAsyncFromWebClient;

            jsonString = await func.Partial(apiUrl).WithRetry();

            if (string.IsNullOrEmpty(jsonString))
                userData.ServiceUnresponded = true;


            userDataFactory.Fill(userData, jsonString, usersIp);


            return userData;
        }
    }
}
