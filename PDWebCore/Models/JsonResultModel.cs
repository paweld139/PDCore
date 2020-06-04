using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PDWebCore.Models
{
    public class JsonResultModel
    {
        public object Data { get; set; }

        public string View { get; set; }

        public bool IsError { get; set; }

        public bool IsConfirm { get; set; }

        public string Message { get; set; }

        public JsonResultModel(object data, string message, string view, bool isError, bool isConfirm) : this(message, isError)
        {
            Data = data;
            View = view;
            IsConfirm = isConfirm;
        }

        public JsonResultModel(string message, bool isError)
        {
            Message = message;
            IsError = isError;
        }
    }
}
