using PDCore.Services;
using PDCore.Utils;
using PDWebCore.Factories.IFac;
using PDWebCore.Models;
using PDWebCore.Services.IServ;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using PDCore.Repositories.IRepo;

namespace PDWebCore.Services.Serv
{
    public class UserDataService : IUserDataService
    {
        private const string API_URL_FORMAT = "http://api.ipstack.com/{0}?access_key=33787ab45622e1c767d6857e593df627";

        private readonly WebClient webClient;
        private readonly IUserDataFactory userDataFactory;
        private readonly ISqlRepositoryEntityFramework<UserDataModel> userDataRepo;
        public UserDataService(WebClient webClient, IUserDataFactory userDataFactory, ISqlRepositoryEntityFramework<UserDataModel> userDataRepo)
        {
            this.webClient = webClient;
            this.userDataFactory = userDataFactory;
            this.userDataRepo = userDataRepo;
        }

        public async Task SaveAsync()
        {
            var userData = await GetAsync();

            userDataRepo.Add(userData);

            await userDataRepo.CommitAsync();
        }

        public async Task<UserDataModel> GetAsync()
        {
            string usersIp = Utils.GetIPAddress();

            if (string.IsNullOrEmpty(usersIp))
            {
                return new UserDataModel();
            }

            string apiUrl = string.Format(API_URL_FORMAT, usersIp);

            UserDataModel userData = new UserDataModel();

            string jsonString = string.Empty;

            try
            {
                jsonString = await webClient.DownloadStringTaskAsync(apiUrl);
            }
            catch
            {
                userData.ServiceUnresponded = true;
            }

            userDataFactory.Fill(userData, jsonString, usersIp);


            return userData;
        }
    }
}
