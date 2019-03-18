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
        public Stream FileStream { get; set; }
        public string FileName { get; set; }
    }
    public interface IEmailService
    {
        Task SendEmail(string email, string subject, string message, List<string> cc, List<FileAttachmentModel> fileAttachments);
    }
    public class EmailService : IEmailService
    {
        public async Task SendEmail(string email, string subject, string message, List<string> cc, List<FileAttachmentModel> fileAttachments)
        {
            MailMessage objeto_mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.Credentials = new System.Net.NetworkCredential("testmailbusiness1412@gmail.com", "!QAZxsw2");
            client.EnableSsl = true;
            objeto_mail.Subject = subject;
            objeto_mail.From = new MailAddress("testmailbusiness1412@gmail.com", "HiSoft JSC");
            objeto_mail.To.Add(new MailAddress(email));
            objeto_mail.Body = message;
            objeto_mail.IsBodyHtml = true;
            if (cc != null)
            {
                foreach (var item in cc)
                {
                    objeto_mail.CC.Add(new MailAddress(item));
                }
            }
            if (fileAttachments != null)
            {
                foreach (var item in fileAttachments)
                {
                    item.FileStream.Position = 0;
                    ContentType ct = new System.Net.Mime.ContentType(FileUtils.GetContentType(item.FileName));
                    System.Net.Mail.Attachment attach = new System.Net.Mail.Attachment(item.FileStream, ct);
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
