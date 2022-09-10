using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Core.DbContext
{
  public class DbUser
  {
    public static UsersEntity Get(string id)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Users
                   where c.UserId == id
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static UsersEntity GetByUserName(string username)
    {
      username = username.Trim().ToLower();

      var db = new LinqMetaData();
      var query = (from c in db.Users
                   where c.Username == username
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static UserModel ConvertToModel(UsersEntity user)
    {
      if (user != null)
      {
        var model = new UserModel()
        {
          id = user.UserId,
          name = user.Name,
          user = user.Username,
          avatar = user.Avatar
        };

        var roleList = new RoleModel();
        roleList.is_role = user.RoleId != 0;
        roleList.is_admin = user.RoleId == 1;
        roleList.is_staff = user.RoleId == 2;
        roleList.is_partner = user.RoleId == 3;
        roleList.is_ads = user.IsAds;
        roleList.is_report = user.IsReport;

        model.role = roleList;

        return model;
      }
      else
        return null;
    }


    public static List<UsersEntity> GetList()
    {
      var db = new LinqMetaData();
      var query = (from c in db.Users
                   where c.Username != "thahnv"
                   orderby c.RoleId, c.Username
                   select c).ToList();
      return query;
    }


    public static List<UsersEntity> GetListStaff()
    {
      var db = new LinqMetaData();
      var query = (from c in db.Users
                   where c.Username != "thahnv"
                   && c.RoleId < 3
                   orderby c.RoleId, c.Username
                   select c).ToList();
      return query;
    }


    public static bool Delete(string id)
    {
      var current = Get(id);
      if (current != null)
        return current.Delete();
      else
        return false;
    }


    #region Dữ liệu cố định

    // Quyền: danh sách
    public static List<StaticModel> Role()
    {
      var list = new List<StaticModel>();

      list.Add(new StaticModel
      {
        id = 1,
        name = "Admin",
        color = "is-danger",
      });

      list.Add(new StaticModel
      {
        id = 2,
        name = "Nhân viên",
        color = "is-link",
      });

      list.Add(new StaticModel
      {
        id = 3,
        name = "Người chia đơn",
        color = "is-primary",
      });

      return list;
    }

    // Quyền: chi tiết
    public static StaticModel Role(int id)
    {
      var query = from s in Role()
                  where s.id == id
                  select s;
      if (query.Count() > 0)
        return query.FirstOrDefault();
      return null;
    }

    #endregion
  }
}
