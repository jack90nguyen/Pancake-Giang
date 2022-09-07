using Onetez.Core.DbContext;
using Onetez.Dal.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Runtime.Caching;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Onetez.Web.Areas.APIv1.Controllers
{
  public class BaseController : Controller
  {
    public DateTime DateTimeNow = DateTime.Now.AddHours(Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]));

    public UserModel UserInfo = GetUserInfo();

    public static UserModel GetUserInfo()
    {
      var user_id = System.Web.HttpContext.Current.User.Identity.Name;
      if (!string.IsNullOrEmpty(user_id))
      {
        var user = DbUser.Get(user_id);

        if (user != null)
        {
          return DbUser.ConvertToModel(user);
        }
        else
        {
          FormsAuthentication.SignOut();
          return null;
        }
      }
      else
        return null;
    }

    #region Caching

    public bool SetCache(string key, object value)
    {
      try
      {
        //Thời gian cache, tính bằng phút
        int TimeCache = 5;

        ObjectCache cache = MemoryCache.Default;
        CacheItemPolicy policy = new CacheItemPolicy();
        policy.AbsoluteExpiration = DateTimeOffset.Now.AddMinutes(TimeCache);
        if (value != null)
        {
          cache.Add(key, value, policy, null);
          return true;
        }
        else
          return false;
      }
      catch (Exception)
      {
        return false;
      }
    }

    public void ClearCache(string keyCache)
    {
      var cache = MemoryCache.Default;
      var keys = new List<string>();

      if (!string.IsNullOrEmpty(keyCache))
        keys.Add(keyCache);

      foreach (var key in keys)
      {
        if (!string.IsNullOrEmpty(key) && cache[key] != null)
          cache.Remove(key);
      }
    }


    #endregion
  }
}
