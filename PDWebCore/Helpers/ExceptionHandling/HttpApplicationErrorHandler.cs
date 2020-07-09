using Newtonsoft.Json;
using PDCore.Enums;
using PDCoreNew;
using PDCoreNew.Services.Serv;
using PDWebCore.Context.IContext;
using PDWebCore.Models;
using PDWebCore.Services.Serv;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDWebCore.Helpers.ExceptionHandling
{
    public static class HttpApplicationErrorHandler
    {
        private async static Task<Tuple<string, HttpStatusCode>> DoHandleExceptionAsync(Exception exception, LogType logType, bool sync)
        {
            string msg;

            HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError;

            Task logTask = null;

            if (exception is HttpException httpException)
            {
                int httpCode = httpException.GetHttpCode();

                if (!sync)
                {
                    logTask = LogService.LogAsync(httpCode.ToString(), exception, logType);
                }
                else
                {
                    LogService.Log(httpCode.ToString(), exception, logType);
                }

                httpStatusCode = (HttpStatusCode)httpCode;             

                switch (httpCode)
                {
                    case 404:
                        // page not found
                        msg = "Błąd 404 - zasób nie został odnaleziony.";
                        break;
                    case 500:
                        // server error
                        msg = "Błąd 500 - wewnętrzny błąd serwera.";
                        break;
                    default:
                        msg = string.Empty;
                        break;
                }

                msg += " " + exception.Message;
            }
            else if (exception is DbEntityValidationException ex)
            {
                msg = ex.GetErrors();

                if (!sync)
                {
                    logTask = LogService.LogAsync(msg, exception, logType);
                }
                else
                {
                    LogService.Log(msg, exception, logType);
                }
            }
            else
            {
                msg = exception.Message;

                if (!sync)
                {
                    logTask = LogService.LogAsync(exception, logType);
                }
                else
                {
                    LogService.Log(exception, logType);
                }
            }

            if(!sync)
            {
                await logTask;
            }

            return new Tuple<string, HttpStatusCode>(msg, httpStatusCode);
        }

        public static Task<Tuple<string, HttpStatusCode>> HandleExceptionAsync(Exception exception, LogType logType)
        {
            return DoHandleExceptionAsync(exception, logType, false);
        }

        public static Tuple<string, HttpStatusCode> HandleException(Exception exception, LogType logType)
        {
            return DoHandleExceptionAsync(exception, logType, true).Result;
        }

        public static void HandleLastException(HttpServerUtility httpServerUtility, HttpRequest httpRequest, HttpResponse httpResponse)
        {
            Exception exception = httpServerUtility.GetLastError();

            httpServerUtility.ClearError();

            try
            {
                httpResponse.Clear();
            }
            catch
            {
                LogService.Log(exception, LogType.Fatal);

                return;
            }

            string msg = HandleException(exception, LogType.Fatal).Item1;


            if (httpRequest.IsAjaxRequest())
            {
                var jsonResult = new JsonResultModel(msg, true);

                string response = JsonConvert.SerializeObject(jsonResult);

                httpResponse.Write(response);
            }
            else
            {
                httpResponse.Redirect(string.Format("~/Errors?message={0}", msg));
            }
        }
    }
}
