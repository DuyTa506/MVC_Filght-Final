using System.Net;
using System.Net.Mail;

namespace EaseFlight.Common.EmailSenders
{
    public static class EmailSender
    {
        public static bool SendMailResetPassword(string toAddress, string baseUrl, string token)
        {
            var subject = Constants.Constant.CONST_EMAIL_RESET_PASSWORD_SUBJECT;
            var linkReset = baseUrl + string.Format(Constants.Constant.CONST_EMAIL_RESET_PASSWORD_LINK, token);
            var body = Constants.Constant.CONST_EMAIL_RESET_PASSWORD_BODY1 + linkReset + Constants.Constant.CONST_EMAIL_RESET_PASSWORD_BODY2;

            var result = SendMail(subject, body, toAddress);

            return result;
        }

        public static bool SendMailBookingSuccess(string toAddress, string myticketUrl, string paymentId, string departDate, string flight, string passenger, string seat, string price, int ticketID)
        {
            var subject = Constants.Constant.CONST_EMAIL_BOOK_SUCCESS_SUBJECT;
            var body = "<html><body style=\"font-size:13px; font-family:Tahoma;\"><h1>Flight: "+ flight +"</h1><p>You have successfully booked a ticket, to review all ticket: <a href=\"" + myticketUrl + "\">My Ticket</a></p>"
                       + "<p>Ticket Information:</p><ul><li><b>Ticket ID:</b> " + ticketID + "</li><li><b>Payment ID:</b> " + paymentId +"</li><li><b>Depart Date:</b> "+ departDate +"</li><li><b>Flight:</b> "+ flight +"</li><li><b>Passengers:</b> "
                       + passenger +"</li><li><b>Seat:</b> "+ seat +"</li><li><b>Payment Method:</b> Paypal</li><li><b>Amount Paid:</b> $"+ price +"</li></ul></body></html>";

            var result = SendMail(subject, body, toAddress);

            return result;
        }

        public static bool SendMailReturnTicket(string toAddress, string flight, string ticketprice, string refund, string admin)
        {
            var subject = Constants.Constant.CONST_EMAIL_RETURN_TICKET_SUBJECT;
            var body = "<html><body style=\"font-size:13px; font-family:Tahoma;\"><h1>Ticket: "+ flight +"</h1><p>You have successfully return ticket <b>"+ flight + " ($"+ ticketprice +")</b>, you get the money refund: <b>$"+ refund +"</b></p>"
                        + "<p>Thank you for booking from EaseFlight</p></body></html>";

            if(admin!= null && admin.Equals("admin"))
            {
                subject = "[Ease Flight] Your ticket has returned by Admin";
                body = "<html><body style=\"font-size:13px; font-family:Tahoma;\"><h1>Ticket: " + flight + "</h1><p>You has returned ticket <b>" + flight + " ($" + ticketprice + ")</b> by Admin EaseFlight, you get the money refund: <b>$" + refund + "</b></p>"
                        + "<p>If you have any questions, please contact EaseFlight support.</p></body></html>";
            }

            var result = SendMail(subject, body, toAddress);

            return result;

        }

        private static bool SendMail(string subject, string body, string toAddress)
        {
            var smtpAddress = "smtp.gmail.com";
            var portNumber = 587;
            var enableSSL = true;
            var defaultEmailSender = Constants.Constant.CONST_EMAIL_DEFAULT_EMAIL_SENDER;
            var password = Constants.Constant.CONST_EMAIL_DEFAULT_EMAIL_SENDER_PASSWORD;
            var emailToAddress = toAddress;

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