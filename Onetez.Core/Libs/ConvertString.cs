using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace Onetez.Core.Libs
{
  public class ConvertString
  {
    private static readonly string[] VietnameseSigns = new string[]
                                                           {
                                                                   "aAeEoOuUiIdDyY",
                                                                   "áàạảãâấầậẩẫăắằặẳẵ",
                                                                   "ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
                                                                   "éèẹẻẽêếềệểễ",
                                                                   "ÉÈẸẺẼÊẾỀỆỂỄ",
                                                                   "óòọỏõôốồộổỗơớờợởỡ",
                                                                   "ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
                                                                   "úùụủũưứừựửữ",
                                                                   "ÚÙỤỦŨƯỨỪỰỬỮ",
                                                                   "íìịỉĩ",
                                                                   "ÍÌỊỈĨ",
                                                                   "đ",
                                                                   "Đ",
                                                                   "ýỳỵỷỹ",
                                                                   "ÝỲỴỶỸ"

                                                           };

    public static string Convert(string str)
    {
      byte[] strBytes = Encoding.UTF8.GetBytes(str);

      byte[] asciiBytes = Encoding.Convert(Encoding.UTF8, Encoding.ASCII, strBytes);

      string asciiStr = Encoding.ASCII.GetString(asciiBytes);

      if (asciiStr.Contains("?"))
        return asciiStr.Replace("?", "");
      else
        return str;
    }

    /// <summary>
    /// Chuyển chuỗi tiếng Việt có dấu thành chuỗi không dấu và kô có chữ hoa
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string NoVNeseLower(string str)
    {
      //Loại bỏ ký tự đặc biệt
      str = NoSpecial(str);

      //Xóa dấu "SPACE" ở đầu và cuối
      str = str.Trim();

      //Xóa 2 dấu "SPACE" liên tiếp
      while (str.Contains("  "))
      {
        str = str.Replace("  ", " ");
      }

      //Chuyển dấu "SPACE" thành "-"
      str = str.Replace(" ", "-");

      //Chuyển thành chuỗi không dấu
      str = NoVNeseLowerOverSpecial(str);

      //Chuyển thành chữ thường
      str = str.ToLower();

      return str;
    }

    /// <summary>
    /// Chuyển chuỗi tiếng Việt có dấu thành chuỗi không dấu và kô có chữ hoa, không loại bỏ ký tự đặc biết
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string NoVNeseLowerOverSpecial(string str)
    {
      //Tiến hành thay thế , lọc bỏ dấu cho chuỗi
      for (int i = 1; i < VietnameseSigns.Length; i++)
      {
        for (int j = 0; j < VietnameseSigns[i].Length; j++)
          str = str.Replace(VietnameseSigns[i][j], VietnameseSigns[0][i - 1]);
      }
      str = Convert(str);
      return str.ToLower().Trim();
    }

    /// <summary>
    /// Loại bỏ ký tự đặt biệt, không xóa dấu phẩy
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string NoSpecial(string str)
    {
      //Loại bỏ ký tự đặc biệt
      str = str.Replace("?", "");
      str = str.Replace("'", "");
      str = str.Replace("-", "");
      str = str.Replace("/", "");
      str = str.Replace("\"", "");
      str = str.Replace("’", "");
      str = str.Replace(",", "");
      str = str.Replace(":", "");
      str = str.Replace(";", "");
      str = str.Replace("!", "");
      str = str.Replace(".", "");
      str = str.Replace("~", "");
      str = str.Replace("`", "");
      str = str.Replace("#", "");
      str = str.Replace("@", "");
      str = str.Replace("$", "");
      str = str.Replace("%", "");
      str = str.Replace("^", "");
      str = str.Replace("&", "");
      str = str.Replace("*", "");
      str = str.Replace("(", "");
      str = str.Replace(")", "");
      str = str.Replace("<", "");
      str = str.Replace(">", "");
      str = str.Replace("[", "");
      str = str.Replace("]", "");
      str = str.Replace("{", "");
      str = str.Replace("}", "");
      str = str.Replace("=", "");
      str = str.Replace("+", "");
      str = str.Replace("®", "");
      str = str.Replace("–", "");
      str = str.Replace("|", "");
      return str.Trim();
    }

    /// <summary>
    /// Chuẩn hóa File Name
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string RenameFile(string fileName)
    {
      //Loại bỏ ký tự đặc biệt
      #region Special

      fileName = fileName.Replace("?", "");
      fileName = fileName.Replace("'", "");
      fileName = fileName.Replace("/", "");
      fileName = fileName.Replace("\"", "");
      fileName = fileName.Replace("’", "");
      fileName = fileName.Replace(",", "");
      fileName = fileName.Replace(":", "");
      fileName = fileName.Replace(";", "");
      fileName = fileName.Replace("!", "");
      fileName = fileName.Replace("~", "");
      fileName = fileName.Replace("`", "");
      fileName = fileName.Replace("#", "");
      fileName = fileName.Replace("@", "");
      fileName = fileName.Replace("$", "");
      fileName = fileName.Replace("%", "");
      fileName = fileName.Replace("^", "");
      fileName = fileName.Replace("&", "");
      fileName = fileName.Replace("*", "");
      fileName = fileName.Replace("(", "");
      fileName = fileName.Replace(")", "");
      fileName = fileName.Replace("<", "");
      fileName = fileName.Replace(">", "");
      fileName = fileName.Replace("[", "");
      fileName = fileName.Replace("]", "");
      fileName = fileName.Replace("{", "");
      fileName = fileName.Replace("}", "");
      fileName = fileName.Replace("=", "");
      fileName = fileName.Replace("+", "");
      fileName = fileName.Replace("®", "");
      fileName = fileName.Replace("|", "");

      #endregion

      //Xóa dấu "SPACE" ở đầu và cuối
      fileName = fileName.Trim();

      //Xóa 2 dấu "SPACE" liên tiếp
      while (fileName.Contains("  "))
      {
        fileName = fileName.Replace("  ", " ");
      }

      //Chuyển dấu "SPACE" thành "-"
      fileName = fileName.Replace(" ", "-");

      //Chuyển thành chuỗi không dấu
      fileName = NoVNeseLowerOverSpecial(fileName);

      //Chuyển thành chữ thường
      fileName = fileName.ToLower();

      return fileName;
    }

    /// <summary>
    /// Lấy định dạng file
    /// </summary>
    /// <param name="fileName"></param>
    /// <returns></returns>
    public static string GetFileFormat(string fileName)
    {
      int index_dot = fileName.LastIndexOf('.');
      if (index_dot > 0)
        return fileName.Substring(index_dot + 1);
      else
        return "?";
    }

    /// <summary>
    /// Hàm làm tròn kết quả
    /// </summary>
    /// <param name="input">Số Cần làm tròn</param>
    /// <param name="r">Muốn làm tròn</param>
    /// <returns></returns>
    public static double RoundNumber(double input, int r)
    {
      if (r == 0)
      {
        return input;
      }
      else if (r > 0)
      {
        return Math.Round(input, r);
      }
      else
      {
        double mu = Math.Pow(10, -r);
        return System.Convert.ToInt64(input / mu) * mu;
      }
    }

    /// <summary>
    /// Đổi cách hiển thị thời gian
    /// </summary>
    /// <returns></returns>
    public static string ConvertDate(DateTime? dateCompare, DateTime? date)
    {
      var DateTimeNow = DateTime.Now.AddHours(System.Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]));
      if (dateCompare != null)
        DateTimeNow = dateCompare.Value;

      string postTime = string.Empty;
      if (date != null)
      {
        if (DateTime.Compare(date.Value, DateTimeNow) <= 0)
        {
          TimeSpan spanMe = DateTimeNow.Subtract(date.Value);
          if (spanMe.Days < 1)
          {
            if (spanMe.Hours < 1)
            {
              if (spanMe.Minutes < 1)
              {
                if (spanMe.Seconds < 5)
                  postTime = "vừa xong";
                else
                  postTime = spanMe.Seconds + " giây trước";
              }
              else
                postTime = spanMe.Minutes + " phút trước";
            }
            else
              postTime = spanMe.Hours + " giờ trước";
          }
          else if (spanMe.Days < 30)
          {
            postTime = spanMe.Days + " ngày trước";
          }
          else if (spanMe.Days < 365)
          {
            postTime = (System.Convert.ToInt32(spanMe.Days / 30)) + " tháng trước";
          }
          else if (spanMe.Days > 365)
          {
            postTime = (System.Convert.ToInt32(spanMe.Days / 365)) + " năm trước";
          }
        }
        else
        {
          TimeSpan spanMe = date.Value.Subtract(DateTimeNow);
          if (spanMe.Days < 1)
          {
            if (spanMe.Hours < 1)
            {
              if (spanMe.Minutes < 1)
              {
                if (spanMe.Seconds < 5)
                  postTime = "bây giờ";
                else
                  postTime = spanMe.Seconds + " giây nữa";
              }
              else
                postTime = spanMe.Minutes + " phút nữa";
            }
            else
              postTime = spanMe.Hours + " giờ nữa";
          }
          else if (spanMe.Days < 30)
          {
            postTime = spanMe.Days + " ngày nữa";
          }
          else if (spanMe.Days < 365)
          {
            postTime = (System.Convert.ToInt32(spanMe.Days / 30)) + " tháng nữa";
          }
          else if (spanMe.Days > 365)
          {
            postTime = (System.Convert.ToInt32(spanMe.Days / 365)) + " năm nữa";
          }
        }
      }

      return postTime;
    }

    /// <summary>
    /// Đổi cách hiển thị tiền tệ
    /// </summary>
    /// <returns></returns>
    public static string ConvertCurrency(double money)
    {
      string str = string.Empty;
      if (money != 0)
      {
        bool isNegative = money < 0;
        money = isNegative ? -money : money;

        if (money >= 1000000000)
        {
          str = " " + String.Format("{0:0,0.00}", money / 1000000000);
          str = str.Replace(" 0", "").Replace(".00", "");
          str += "b";
          //unit = "tỷ";
        }
        else if (money >= 1000000)
        {
          str = " " + String.Format("{0:0,0.00}", money / 1000000);
          str = str.Replace(" 0", "").Replace(".00", "");
          str += "m";
          //unit = "triệu";
        }
        else
        {
          str = String.Format("{0:0,0}", money / 1000);
          str += "k";
          //unit = "nghìn";
        }

        str = isNegative ? "-" + str : str;
      }
      else
      {
        str = "0";
      }

      return str;
    }

    /// <summary>
    /// Nhận diện link trong text
    /// </summary>
    /// <returns></returns>
    public static string GetLinks(string text)
    {
      string content = text;
      List<string> links = new List<string>();
      Regex urlRx = new Regex(@"((https?|ftp|file)\://|www.)[A-Za-z0-9\.\-]+(/[A-Za-z0-9\?\&\=;\+!'\(\)\*\-\._~%]*)*", RegexOptions.IgnoreCase);

      MatchCollection matches = urlRx.Matches(text);
      foreach (Match match in matches)
      {
        links.Add(match.Value);
      }

      links = links.Distinct().ToList();

      foreach (var item in links)
        content = content.Replace(item, "<a href=\"" + item + "\" target=\"_blank\">" + item + "</a>");

      return content;
    }

    /// <summary>
    /// Chuyển mãng thành từ khóa
    /// </summary>
    /// <returns></returns>
    public static string Keywords(string[] keys)
    {
      var sbKey = new StringBuilder();

      for (int i = 0; i < keys.Length; i++)
      {
        sbKey.Append(keys[i] + " ");
      }

      return sbKey.ToString().ToLower();
    }
  }
}
