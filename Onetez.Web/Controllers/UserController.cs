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
  public class UserController : BaseController
  {
    #region Danh sách tài khoản

    [Authorize]
    [HttpGet]
    public ActionResult Index()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_admin)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });


      var results = DbUser.GetList();

      return View(results);
    }

    #endregion


    #region Tạo tài khoản

    [Authorize]
    [HttpGet]
    public ActionResult Create()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_role)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });


      var user = new UsersEntity();

      user.RoleId = 2;

      FormCreate();

      return View(user);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Create(UsersEntity model)
    {
      var user = new UsersEntity();

      user.UserId = DbConfig.GenerateId();

      if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
      {
        ViewBag.Notification = Shared.RenderNotification("Nhập các thông tin bắt buộc", false);
      }
      else if (DbUser.GetByUserName(model.Username) != null)
      {
        ViewBag.Notification = Shared.RenderNotification("Username này đã được sử dụng, vui lòng chọn tên khác", false);
      }
      else
      {
        if (Request.Files.Count > 0)
        {
          var file = Request.Files[0];
          if (file != null && file.ContentLength > 0)
          {
            string folder = "Upload/user/" + user.UserId + "/";
            string path = AppDomain.CurrentDomain.BaseDirectory + folder;
            if (!Directory.Exists(path))
              Directory.CreateDirectory(path);

            var fileName = Path.GetFileName(ConvertString.RenameFile(file.FileName));
            file.SaveAs(path + fileName);

            user.Avatar = "/" + folder + fileName;
          }
        }
        if (string.IsNullOrEmpty(user.Avatar))
          user.Avatar = "/Images/avatar.png";

        user.Username = model.Username.Trim().ToLower();
        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5");
        user.Name = model.Name;
        user.Email = model.Email.Trim().ToLower();
        user.RoleId = model.RoleId;
        user.IsAds = model.IsAds;
        user.IsReport = model.IsReport;
        user.Save();

        return RedirectToAction("Index", "User");
      }


      FormCreate();

      return View(user);
    }

    public void FormCreate()
    {
      #region Quyền

      var slRole = new List<SelectListItem>();
      slRole.Add(new SelectListItem { Text = "-- Chọn quyền --", Value = "0" });
      foreach (var sl in DbUser.Role())
        slRole.Add(new SelectListItem { Text = sl.name, Value = sl.id.ToString() });
      ViewBag.DdlRole = slRole;

      #endregion
    }

    #endregion


    #region Thông tin tài khoản

    [Authorize]
    [HttpGet]
    public ActionResult Info(string id)
    {
      var user = DbUser.Get(id);

      if (user != null)
      {
        user.Password = "";

        FormInfo();

        return View(user);
      }
      else
      {
        return RedirectToAction("Page404", "Home", new { url = Request.RawUrl });
      }
    }

    [Authorize]
    [HttpPost]
    public ActionResult Info(string id, UsersEntity model)
    {
      var user = DbUser.Get(id);

      if (Request.Files.Count > 0)
      {
        var file = Request.Files[0];
        if (file != null && file.ContentLength > 0)
        {
          string folder = "Upload/user/" + user.UserId + "/";
          string path = AppDomain.CurrentDomain.BaseDirectory + folder;
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

          var fileName = Path.GetFileName(ConvertString.RenameFile(file.FileName));
          file.SaveAs(path + fileName);

          user.Avatar = "/" + folder + fileName;
        }
      }
      if (string.IsNullOrEmpty(user.Avatar))
        user.Avatar = "/Images/avatar.png";

      user.Name = model.Name;
      user.Email = model.Email.Trim().ToLower();

      if (!string.IsNullOrEmpty(model.Password))
        user.Password = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5");

      // USER: kiểm tra quyền
      if (UserInfo.role.is_admin)
      {
        user.RoleId = model.RoleId;
        user.IsAds = model.IsAds;
        user.IsReport = model.IsReport;
      }

      user.Save();

      return RedirectToAction("Info", "User", new { @id = user.UserId });
    }

    public void FormInfo()
    {
      ViewBag.IsRoleAdmin = UserInfo.role.is_admin;

      #region Quyền

      var slRole = new List<SelectListItem>();
      slRole.Add(new SelectListItem { Text = "-- Chọn quyền --", Value = "0" });
      foreach (var sl in DbUser.Role())
        slRole.Add(new SelectListItem { Text = sl.name, Value = sl.id.ToString() });
      ViewBag.DdlRole = slRole;

      #endregion
    }

    #endregion


    #region Đăng nhập

    [HttpGet]
    public ActionResult Login()
    {
      return View();
    }

    [HttpPost]
    public ActionResult Login(string username, string password)
    {
      if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
      {
        ViewBag.Notification = Shared.RenderNotification("Nhập các thông tin đăng nhập", false);
      }
      else
      {
        var user = DbUser.GetByUserName(username);

        string pass_md5 = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");

        if (user != null && user.Password == pass_md5)
        {
          FormsAuthentication.SetAuthCookie(user.UserId, true);

          if (Request.QueryString["ReturnUrl"] != null)
            return Redirect(Request.QueryString["ReturnUrl"]);
          else
            return RedirectToAction("Index", "User");
        }
        else
        {
          ViewBag.Notification = Shared.RenderNotification("Thông tin đăng nhập không chính xác", false);
        }
      }

      return View();
    }

    #endregion
  }
}
