using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;

namespace Onetez.Core.DbContext
{
  public class DbConfig
  {
    public static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();

    public static int TimeUtc = Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]);

    public static string GenerateId()
    {
      string guid = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
      return string.Format("{0:yyMMdd}", DateTime.Now) + guid;
    }


    public static ConfigsEntity Get(int id)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Configs
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static bool SqlUpdate(string query, out string msg)
    {
      try
      {
        msg = string.Empty;

        var con = new SqlConnection(SqlConnect);
        con.Open();
        string sql = query;
        SqlCommand command = new SqlCommand(sql, con);
        int result = command.ExecuteNonQuery();
        con.Close();
        return true;
      }
      catch (Exception ex)
      {
        msg = ex.Message;

        return false;

        throw;
      }
    }


    public static DataTable SqlGet(string query)
    {
      var con = new SqlConnection(SqlConnect);
      con.Open();
      var da = new SqlDataAdapter(query, con);
      var myTable = new DataTable();
      da.Fill(myTable);
      con.Close();
      return myTable;
    }


    #region Dữ liệu cố định


    // Mốc giờ
    public static List<StaticModel> TimeList()
    {
      var list = new List<StaticModel>();

      for (int i = 0; i < 24; i++)
      {
        string time00 = string.Format("{0:00}:00", i);
        string time30 = string.Format("{0:00}:30", i);

        list.Add(new StaticModel
        {
          id = i * 10,
          name = time00,
        });

        list.Add(new StaticModel
        {
          id = i * 10 + 1,
          name = time30,
        });
      }

      return list;
    }

    #endregion
  }
}