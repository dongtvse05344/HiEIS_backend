using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace HiEIS.Service
{
    public class FileAttachmentModel
    {
        public Stream FileContentStream { get; set; }
        public string FileName { get; set; }
    }
    public class EmailModel
    {
        public EmailModel()
        {
            FromMail = "testmailbusiness1412@gmail.com";
            Password = "!QAZxsw2";
        }
        public string FromMail { get; set; }
        public string MailName { get; set; }
        public string Password { get; set; }
        public string ToMail { get; set; }
        public string Subject { get; set; }
        public string  Message { get; set; }
        public List<string> Cc { get; set; }
        public List<FileAttachmentModel> FileAttachments { get; set; }
    }
    public interface IEmailService
    {
        Task SendEmail(EmailModel model);
    }
    public class EmailService : IEmailService
    {
        public async Task SendEmail(EmailModel model)
        {
            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential(model.FromMail, model.Password);
            client.EnableSsl = true;
            objeto_mail.Subject = model.Subject;
            objeto_mail.From = new MailAddress(model.FromMail, model.MailName);
            objeto_mail.To.Add(new MailAddress(model.ToMail));
            objeto_mail.Body = model.Message;
            objeto_mail.IsBodyHtml = true;
            if (model.Cc != null)
            {
                foreach (var item in model.Cc)
                {
                    objeto_mail.CC.Add(new MailAddress(item));
                }
            }
            if (model.FileAttachments != null)
            {
                foreach (var item in model.FileAttachments)
                {
                    item.FileContentStream.Position = 0;
                    ContentType ct = new System.Net.Mime.ContentType(FileUtils.GetContentType(item.FileName));
                    System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(item.FileContentStream, ct);
                    attach.ContentDisposition.FileName = item.FileName;
                    objeto_mail.Attachments.Add(attach);
                }
                await client.SendMailAsync(objeto_mail);

                foreach (var att in objeto_mail.Attachments)
                {
                    att.Dispose();
                }
            }

        }

    }
}
