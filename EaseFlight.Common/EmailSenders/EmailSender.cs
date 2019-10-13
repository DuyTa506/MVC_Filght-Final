using System.Net;
using System.Net.Mail;

namespace EaseFlight.Common.EmailSenders
{
    public static class EmailSender
    {
        public static bool Send(string toAddress, string baseUrl, string token)
        {
            var smtpAddress = "smtp.gmail.com";
            var portNumber = 587;
            var enableSSL = true;
            var defaultEmailSender = Constants.Constant.CONST_EMAIL_DEFAULT_EMAIL_SENDER;
            var password = Constants.Constant.CONST_EMAIL_DEFAULT_EMAIL_SENDER_PASSWORD;
            var emailToAddress = toAddress;
            var subject = Constants.Constant.CONST_EMAIL_RESET_PASSWORD_SUBJECT;
            var linkReset = baseUrl + string.Format(Constants.Constant.CONST_EMAIL_RESET_PASSWORD_LINK, token);
            var body = Constants.Constant.CONST_EMAIL_RESET_PASSWORD_BODY1 + linkReset + Constants.Constant.CONST_EMAIL_RESET_PASSWORD_BODY2;

            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(defaultEmailSender);
                    mail.To.Add(emailToAddress);
                    mail.Subject = subject;
                    mail.Body = body;
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient(smtpAddress, portNumber))
                    {
                        smtp.Credentials = new NetworkCredential(defaultEmailSender, password);
                        smtp.EnableSsl = enableSSL;
                        smtp.Send(mail);
                    }
                }

                return true;
            }
            catch
            {

            }

            return false;
        }
    }
}
