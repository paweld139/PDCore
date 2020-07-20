using PDCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows;

namespace PDCore.Models
{
    public class MailMessageModel
    {
        public MailMessageModel()
        {
        }

        public MailMessageModel(string receiverEmail, string subject, string body, bool isBodyHtml = true, IEnumerable<string> attachmentPaths = null)
        {
            ReceiverEmails = receiverEmail;
            Subject = subject;
            Body = body;
            IsBodyHtml = isBodyHtml;
            AttachmentPaths = attachmentPaths;
        }

        public string ReceiverEmails { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public bool IsBodyHtml { get; set; }

        public IEnumerable<string> AttachmentPaths { get; set; }


        public MailMessage GetMailMessage(SmtpSettingsModel smtpSettingsModel)
        {
            MailMessage message = new MailMessage
            {
                Subject = Subject,
                Body = Body,
                IsBodyHtml = IsBodyHtml
            };

            if (!string.IsNullOrEmpty(smtpSettingsModel.DisplayName))
                message.From = new MailAddress(smtpSettingsModel.Email, smtpSettingsModel.DisplayName);
            else if(!string.IsNullOrEmpty(smtpSettingsModel.Email))
                message.From = new MailAddress(smtpSettingsModel.Email);

            message.To.Add(ReceiverEmails);

            if(AttachmentPaths != null)
            {
                AttachmentPaths.ForEach(a => message.Attachments.Add(new Attachment(a)));
            }

            return message;
        }
    }
}
