using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;

namespace PDCore.Models
{
    public class MailMessageModel
    {
        public MailMessageModel()
        {
        }

        public MailMessageModel(string receiverEmail, string title, string body, bool isBodyHtml)
        {
            ReceiverEmail = receiverEmail;
            Title = title;
            Body = body;
            IsBodyHtml = isBodyHtml;
        }

        public string ReceiverEmail { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }


        public MailMessage GetMailMessage(SmtpSettingsModel smtpSettingsModel)
        {
            MailMessage message = new MailMessage
            {
                Subject = Title,
                Body = Body,
                IsBodyHtml = IsBodyHtml
            };

            if (!string.IsNullOrEmpty(smtpSettingsModel.DisplayName))
                message.From = new MailAddress(smtpSettingsModel.Email, smtpSettingsModel.DisplayName);
            else
                message.From = new MailAddress(smtpSettingsModel.Email);

            message.To.Add(ReceiverEmail);

            return message;
        }
    }
}
