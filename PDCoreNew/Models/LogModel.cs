using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using PDCore.Enums;
using PDCore.Interfaces;

namespace PDCoreNew.Models
{
    [Table("Log")]
    public class LogModel : IByDateFindable
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
