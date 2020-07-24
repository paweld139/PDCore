using Newtonsoft.Json;
using PDWebCore.Factories.IFac;
using PDWebCore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDWebCore.Factories.Fac
{
    public class UserDataFactory : IUserDataFactory
    {
        public void Fill(UserDataModel userData, string jsonString, string usersIp)
        {
            if (!userData.ServiceUnresponded)
            {
                JsonConvert.PopulateObject(jsonString, userData);

                //if (userData.Location?.GeonameId == null)
                //    userData.Location = null;
            }

            HttpRequest request = HttpContext.Current.Request;

            string ua = request.UserAgent;
            string info = ua.Split(';')[0].Split('(')[1] + ua.Split(';')[1];

            userData.IP = usersIp;
            userData.Device = request.Browser.IsMobileDevice ? DeviceType.Mobile : DeviceType.Desktop;
            userData.PhoneManufacturer = request.Browser.MobileDeviceManufacturer == "Unknown" ? null : request.Browser.MobileDeviceManufacturer;
            userData.PhoneModel = request.Browser.MobileDeviceModel == "Unknown" ? null : request.Browser.MobileDeviceManufacturer;
            userData.Resolution = request.Browser.ScreenPixelsWidth.ToString() + "x" + request.Browser.ScreenPixelsHeight.ToString();
            userData.Browser = request.Browser.Browser + " " + request.Browser.Version;
            userData.Platform = request.Browser.Platform;
            userData.OperatingSystem = info.Length < 64 ? info : ua.Split(';')[0].Split('(')[1];
            userData.Language = request.UserLanguages.Length > 0 ? request.UserLanguages[0] : null;
        }
    }
}
