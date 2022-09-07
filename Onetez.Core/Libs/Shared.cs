using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Onetez.Core.Libs
{
  public class Shared
  {
    /// <summary>
    /// Phân trang
    /// </summary>
    #region Paging

    /// <summary>
    /// Phân trang cho Admin
    /// </summary>
    public static string CreateCollection(int total, int current, int size, string url)
    {
      if (size == 0)
        return "";

      //--Tính số trang
      int col = total / size;
      float tp = total / (float)size;
      if (total % size != 0 && tp > (col))
        col = total / size + 1;
      else
        col = total / size;

      if (!url.Contains("?"))
        url = url + "?p=#";
      else if (url.Contains("?p=" + current))
        url = url.Replace("?p=" + current, "?p=#");
      else if (url.Contains("&p=" + current))
        url = url.Replace("&p=" + current, "&p=#");
      else
        url = url + "&p=#";

      string str = string.Empty;
      int feft = 0;
      int right = 0;
      int view = 4;
      for (int i = 1; i <= col; i++)
      {
        string link = url.Replace("p=#", "p=" + i);
        string css = i == current ? "is-current" : "";
        string style = "";

        if (current - i > view)
        {
          style += "style=\"display: none\"";
          feft++;
        }
        if (i - current > view)
        {
          style += "style=\"display: none\"";
          right++;
        }

        str += "<li>";
        str += "<a class=\"pagination-link " + css + "\" " + style + " href=\"" + link + "\">" + i + "</a>";
        str += "</li>";


        if (feft == 1 && current - (view + 1) > 1)
        {
          str += "<li><span class=\"pagination-ellipsis\">&hellip;</span></li>";
          feft++;
        }
        if (right == 1 && current + view + 1 < col)
        {
          str += "<li><span class=\"pagination-ellipsis\">&hellip;</span></li>";
          right++;
        }
      }

      string frist = current - view > 1 ? "<li><a class=\"pagination-link\" href=\"" + url.Replace("p=#", "p=1") + "\">1</a></li>" : "";
      string last = current + view < col ? "<li><a class=\"pagination-link\" href=\"" + url.Replace("p=#", "p=" + col) + "\">" + col + "</a></li>" : "";
      string result = ((current - 1) * size + 1) + " - " + ((current * size) < total ? (current * size) : total) + " / " + total;

      string list = "<nav class=\"pagination is-rounded is-small\">"
          + "<span class=\"pagination-next\">" + result + "</span>"
          + "<ul class=\"pagination-list\">" + frist + str + last + "</ul>"
          + "</nav>";

      return list;
    }

    #endregion


    /// <summary>
    /// Các hàm khác
    /// </summary>
    #region Các hàm khác

    //Hiển thị thông báo
    public static string RenderNotification(string content, bool isSuccess)
    {
      var sb = new StringBuilder();

      sb.Append("<div class=\"notification is-" + (isSuccess ? "success" : "danger") + " is-light\">");
      sb.Append("<button class=\"delete\"></button>");
      sb.Append(content);
      sb.Append("</div>");

      return sb.ToString();
    }


    /// <summary>
    /// Xóa file gốc
    /// </summary>
    /// <returns></returns>
    public static bool DeleteFile(string file)
    {
      if (file.StartsWith("/"))
        file = ("#" + file).Replace("#/", "");

      string getFile = AppDomain.CurrentDomain.BaseDirectory + file.Replace("/", "\\");

      try
      {
        if (File.Exists(getFile))
        {
          File.Delete(getFile);
          return true;
        }
        else
          return false;
      }
      catch (Exception ex)
      {
        return false;

        throw;
      }
    }


    /// <summary>
    /// Lấy ngày thứ 2
    /// </summary>
    public static DateTime GetMonday(DateTime date)
    {
      var monday = Convert.ToDateTime(date.ToShortDateString());

      if (monday.DayOfWeek == DayOfWeek.Tuesday)
        monday = monday.AddDays(-1);
      else if (monday.DayOfWeek == DayOfWeek.Wednesday)
        monday = monday.AddDays(-2);
      else if (monday.DayOfWeek == DayOfWeek.Thursday)
        monday = monday.AddDays(-3);
      else if (monday.DayOfWeek == DayOfWeek.Friday)
        monday = monday.AddDays(-4);
      else if (monday.DayOfWeek == DayOfWeek.Saturday)
        monday = monday.AddDays(-5);
      else if (monday.DayOfWeek == DayOfWeek.Sunday)
        monday = monday.AddDays(-6);

      return monday;
    }


    #endregion


    /// <summary>
    /// Các hàm tạo giá trị ngẫu hiên
    /// </summary>
    #region Random

    private static readonly Random random = new Random();

    //Tạo một số ngẫu nhiên
    public static int RandomInt(int min, int max)
    {
      return random.Next(min, max);
    }

    //Tạo chuỗi ngẫu nhiên
    public static string RandomString(int length)
    {
      var arrayChar = new List<string> { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z", "0", "1", "2", "3", "4", "5", "6", "7", "8", "9" };
      string text = string.Empty;
      for (int i = 0; i < length; i++)
      {
        string t = arrayChar[random.Next(0, 36)];
        text += random.Next(0, 2) == 0 ? t : t.ToUpper();
      }
      return text;
    }

    #endregion


    /// <summary>
    /// Chuẩn hóa số điện thoại
    /// </summary>
    public static string StandardizedPhone(string phone)
    {
      phone = phone.Trim().Replace(" ", "").Replace("+", "").Replace("-", "");

      if (phone.StartsWith("84") && phone.Length >= 11)
        phone = "0" + phone.Substring(2);
      else if (!phone.StartsWith("0"))
        phone = "0" + phone;

      return phone;
    }
  }
}
