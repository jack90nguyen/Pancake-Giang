using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Onetez.Core.Libs
{
  public class EmailSending
  {
    #region Cấu Hình Email thông báo

    static string ShopName = ConfigurationManager.AppSettings["ShopName"];
    static string Domain = ConfigurationManager.AppSettings["Domain"];

    static string MailSend = ConfigurationManager.AppSettings["MailSend"];
    static string MailPass = ConfigurationManager.AppSettings["MailPass"];
    static string MailServer = ConfigurationManager.AppSettings["MailServer"];
    static int MailPort = Convert.ToInt32(ConfigurationManager.AppSettings["MailPort"]);

    private static bool SendMail(string receiverEmail, string title, string body, string[] bcc, out string msg)
    {
      try
      {
        if (string.IsNullOrEmpty(receiverEmail.Trim()))
        {
          msg = "Không có email nhận";

          return false;
        }

        receiverEmail = receiverEmail.Trim();


        MailMessage mailMessage = new MailMessage();
        mailMessage.From = new MailAddress(MailSend, ShopName);
        mailMessage.To.Add(receiverEmail);
        mailMessage.Subject = title;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = true;

        if (bcc != null)
        {
          foreach (string b in bcc)
          {
            if (!string.IsNullOrEmpty(b.Trim()))
              mailMessage.Bcc.Add(b.Trim());
          }
        }


        SmtpClient mailClient = new SmtpClient(MailServer, MailPort);
        mailClient.Timeout = 15000;
        mailClient.EnableSsl = true;
        mailClient.Credentials = new NetworkCredential(MailSend, MailPass);
        mailClient.Send(mailMessage);

        msg = "Đã gửi";

        return true;
      }
      catch (Exception ex)
      {
        msg = ex.Message;

        return false;
      }
    }


    #endregion




    #region Nội dung email


    public static bool MailRegister(string email, string password, string company, out string msg)
    {
      string tilte = "Bạn đã thêm vào công ty " + company;

      var sb = new StringBuilder();
      sb.AppendFormat("<div>Xin chào {0} !</div>", email);
      sb.AppendFormat("<div>Bạn đã được thêm vào công ty {0}</div>", company);
      sb.AppendFormat("<div>Truy cập website <a href=\"{0}\">{0}</a> để sử dụng</div>", Domain);
      sb.AppendFormat("<div>Thông tin tài khoản:</div>");
      sb.AppendFormat("<div>Username: {0}</div>", email);
      sb.AppendFormat("<div>Password: {0}</div>", password);


      return SendMail(email, tilte, sb.ToString(), null, out msg);
    }




    private static string MailHeader()
    {
      var sb = new StringBuilder();

      //sb.Append("</div>");

      return sb.ToString();
    }


    private static string MailFooter()
    {
      var sb = new StringBuilder();

      //sb.Append("</div>");

      return sb.ToString();
    }

    #endregion
  }
}
