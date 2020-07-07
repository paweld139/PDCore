using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PDWebCore.Services.Serv;
using System.Web;
using PDCore.Enums;

namespace PDWebCore.Models
{
    [Table("Log")]
    public class LogModel
    {
        public LogModel(string message, LogType logType, string requestUri, Exception exception = null)
        {
            Message = message;
            Date = DateTime.Now;

            if (exception != null)
            {
                ErrorType = exception.GetType().Name;
                StackTrace = exception.StackTrace;
                ErrorMessage = exception.Message;
            }

            MachineName = Environment.MachineName;
            LogLevel = logType;
            RequestUri = requestUri;
        }

        public LogModel() { }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ELId { get; set; }

        public string ErrorType { get; set; }

        public string Message { get; set; }

        public string ErrorMessage { get; set; }

        public string StackTrace { get; set; }

        public DateTime Date { get; set; }

        public LogType LogLevel { get; set; }

        public string RequestUri { get; set; }

        public string MachineName { get; set; }
    }
}
