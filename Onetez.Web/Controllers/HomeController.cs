using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Data;
using System.Web.Mvc;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.EntityClasses;
using Onetez.Core.DbContext;

namespace Onetez.Web.Controllers
{
  public class HomeController : BaseController
  {
    [Authorize]
    [HttpGet]
    public ActionResult Index()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_role)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });


      return View();
    }


    [Authorize]
    [HttpGet]
    public ActionResult Page404()
    {

      return View();
    }


    [Authorize]
    [HttpGet]
    public ActionResult PageRole()
    {

      return View();
    }


    [HttpGet]
    public ActionResult Logout()
    {
      FormsAuthentication.SignOut();
      Response.Redirect("/");
      return RedirectToAction("Index", "Home");
    }


    [HttpGet]
    public ActionResult Language()
    {
      var lang = Request.QueryString["lang"];

      HttpCookie cookie = new HttpCookie("_language");
      cookie.Value = Request.QueryString["lang"];
      cookie.Expires = DateTime.Now.AddYears(1);
      Response.Cookies.Add(cookie);

      return RedirectToAction("Index", "Home");
    }
  }
}