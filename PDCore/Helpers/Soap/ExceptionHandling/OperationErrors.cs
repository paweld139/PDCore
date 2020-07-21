using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Xml.Linq;

namespace PDCore.Helpers.Soap.ExceptionHandling
{
    public abstract class OperationErrors<T> : IOperationErrors where T : class, new()
    {
        private void FillErrors (MessageFault messageFault)
        {
            var reader = messageFault.GetReaderAtDetailContents();

            XElement xElement = (XElement)XNode.ReadFrom(reader);

            Errors = new List<T>();

            xElement.Elements().ForEach(x => Errors.Add(x.DeserializeFromXML<T>()));
        }

        public List<T> Errors { get; private set; }

        private string Error { get; set; }

        private const string errorFormat = "Wystąpił błąd: {0}";

        public override string ToString()
        {
            if(Errors == null)
            {
                return Error ?? string.Empty;
            }

            SetError(string.Join(", ", GetErrorsString(Errors)));

            return Error;
        }

        protected abstract IEnumerable<string> GetErrorsString(List<T> errors);

        private void SetError(string error)
        {
            this.Error = string.Format(errorFormat, error);
        }

        public void HandleException(Exception ex)
        {
            if (ex is MessageSecurityException)
            {
                if (ex.InnerException == null)
                {
                    SetError(ex.Message);

                    return;
                }

                HandleException(ex.InnerException);
            }
            else if (ex is FaultException exception)
            {
                HandleFaultException(exception);
            }
            else if (ex is TimeoutException)
            {
                SetError("Upłynął limit czasu operacji usługi. " + ex.Message);
            }
            else if(ex is CommunicationException)
            {
                SetError("Wystąpił problem z komunikacją. " + ex.Message + ex.StackTrace);
            }
            else
            {
                SetError(ex.Message);
            }
        }

        private void HandleFaultException(FaultException fe)
        {
            MessageFault mf = fe.CreateMessageFault();

            if (mf.HasDetail)
            {
                FillErrors(mf);
            }
            else
            {
                SetError(mf.Reason.ToString());
            }
        }
    }
}
