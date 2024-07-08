using System;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace MultiFaceRec.Models
{
    internal class mailgonderim
    {
        public void Microsoft(string senderName, string senderMail, string senderPassword, string recipientMail, string content)
        {
            try
            {
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Port = 587;
                smtpClient.Host = "smtp.outlook.com";
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(senderMail, senderPassword);

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(senderMail, senderName);
                mailMessage.To.Add(recipientMail);
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = content;

                smtpClient.Send(mailMessage);

                MessageBox.Show("Mail başarıyla gönderildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (SmtpException ex)
            {
                MessageBox.Show("SMTP Hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
