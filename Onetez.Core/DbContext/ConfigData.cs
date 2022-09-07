using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.Linq;

namespace Onetez.Core.Data_v1
{
    public class ConfigData
    {
        /// <summary>
        /// Lấy ngôn ngữ hiện tại
        /// </summary>
        /// <returns></returns>
        public static int GetLanguage()
        {
            int langId = 1;
            if (System.Web.HttpContext.Current.Request.Cookies["LanguageCookie"] != null)
            {
                HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["LanguageCookie"];
                if (cookie["LanguageId"] != null)
                    langId = Convert.ToInt32(cookie["LanguageId"]);
                else
                {
                    cookie.Values.Add("LanguageId", langId.ToString());
                    cookie.Expires = DateTime.Now.AddYears(1);
                    System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
                }
            }
            else
            {
                HttpCookie cookie = new HttpCookie("LanguageCookie");
                cookie.Values.Add("LanguageId", langId.ToString());
                cookie.Expires = DateTime.Now.AddYears(1);
                System.Web.HttpContext.Current.Response.Cookies.Add(cookie);
            }
            return langId;
        }

        /// <summary>
        /// Lấy ConfigSystem
        /// </summary>
        /// <returns></returns>
        public static ConfigSystemEntity RetrieveConfigSystem(int langId)
        {
            var db = new LinqMetaData();
            var query = (from p in db.ConfigSystem
                         where p.LanguageId == langId
                         select p).ToList();
            if (query.Count > 0)
                return query.First();
            else
            {
                var vietConfig = new ConfigSystemEntity(1);
                var newConfig = new ConfigSystemEntity();
                newConfig.Domain = vietConfig.Domain;
                newConfig.Company = vietConfig.Company;
                newConfig.MailFromAdress = vietConfig.MailFromAdress;
                newConfig.MailFromPass = vietConfig.MailFromPass;
                newConfig.LanguageId = langId;
                newConfig.Save();

                return newConfig;
            }
        }
    }
}
