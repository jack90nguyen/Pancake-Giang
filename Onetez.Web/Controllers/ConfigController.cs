using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Onetez.Core.Libs;
using Onetez.Core.DbContext;
using Onetez.Dal.Models;
using Onetez.Dal.EntityClasses;
using Newtonsoft.Json;
using System.Configuration;

namespace Onetez.Web.Controllers
{
  public class ConfigController : BaseController
  {
    private bool IsPancake = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPancake"]);


    #region Cấu hình hệ thống

    [Authorize]
    [HttpGet]
    public ActionResult Index()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_admin)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      GetShopList();

      return View(ConfigInfo);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Index(ConfigsEntity model)
    {
      ConfigInfo.GoogleServiceAccount = model.GoogleServiceAccount;
      ConfigInfo.GoogleApplicationName = model.GoogleApplicationName;
      ConfigInfo.PancakeApiUrl = model.PancakeApiUrl;
      ConfigInfo.IsAuto = model.IsAuto;
      ConfigInfo.SheetCategory = model.SheetCategory;

      ConfigInfo.Save();

      return RedirectToAction("Index", "Config");
    }

    public void GetShopList()
    {
      ViewBag.ShopList = DbShop.GetList();
    }


    #endregion


    #region Danh sách shop

    [Authorize]
    [HttpGet]
    public ActionResult Shop(int id)
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_admin)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      var shop = id == 0 ? new ShopsEntity() : DbShop.Get(id);

      if (shop != null)
      {
        GetShopList();

        // Số lượng trang đơn hàng cần lấy
        var sllPage = new List<SelectListItem>();
        for (int i = 0; i <= 60; i++)
          sllPage.Add(new SelectListItem { Text = string.Format("{0:0,0}", i * 500), Value = i.ToString() });
        ViewBag.DdlOrderPage = sllPage;

        // Lưu cache về shop
        if (id != 0)
          SetCache("shop_" + id, DbShop.ConvertToModel(shop));

        return View(shop);
      }
      else
      {
        return RedirectToAction("Page404", "Home", new { url = Request.RawUrl });
      }
    }

    [Authorize]
    [HttpPost]
    public ActionResult Shop(int id, ShopsEntity model, int sheetdate, int sheetname, int sheetphone, int sheetaddress, int sheetproduct, int sheetsize, int sheetcolor, int sheetlink, int sheetnote, int sheetsave, int sheetother)
    {
      var shop = DbShop.Get(id);

      // Tạo bản sao
      if (Request["copy"] != null)
      {
        var copy = new ShopsEntity();
        copy.Id = model.Id + 1;
        copy.Name = model.Name;
        copy.ApiKey = model.ApiKey;
        copy.SpreadsheetId = model.SpreadsheetId;
        copy.SpreadsheetTab = model.SpreadsheetTab;
        copy.WarehouseId = model.WarehouseId;
        copy.WarehouseInfo = model.WarehouseInfo;
        copy.OrderPage = model.OrderPage;
        copy.ProductOtherInNote = model.ProductOtherInNote;
        copy.ProductErrorToNote = model.ProductErrorToNote;
        copy.ProductFindByName = model.ProductFindByName;
        copy.ProductToOrderEmpty = model.ProductToOrderEmpty.Trim().ToLower();
        copy.SheetColumns = shop.SheetColumns;
        copy.Save();

        return RedirectToAction("Shop", "Config", new { id = copy.Id });
      }
      else if(shop == null)
        shop = new ShopsEntity();

      // Tự tạo ID
      if (model.Id == 0 && !IsPancake)
        model.Id = Shared.RandomInt(1000000, 9000000);

      if (model.Id != 0 && !string.IsNullOrEmpty(model.Name) && !string.IsNullOrEmpty(model.ApiKey))
      {
        var sheetColumns = new ShopModel.SheetColumns()
        {
          date = sheetdate,
          name = sheetname,
          phone = sheetphone,
          address = sheetaddress,
          product = sheetproduct,
          link = sheetlink,
          note = sheetnote,
          size = sheetsize,
          color = sheetcolor,
          other = sheetother,
          save = sheetsave
        };

        shop.Id = model.Id;
        shop.Name = model.Name;
        shop.ApiKey = model.ApiKey;
        shop.SpreadsheetId = model.SpreadsheetId;
        shop.SpreadsheetTab = model.SpreadsheetTab;
        shop.WarehouseId = model.WarehouseId;
        shop.WarehouseInfo = model.WarehouseInfo;
        shop.OrderPage = model.OrderPage;
        shop.ProductOtherInNote = model.ProductOtherInNote;
        shop.ProductErrorToNote = model.ProductErrorToNote;
        shop.ProductFindByName = model.ProductFindByName;
        shop.ProductToOrderEmpty = model.ProductToOrderEmpty.Trim().ToLower();
        shop.SheetColumns = JsonConvert.SerializeObject(sheetColumns);

        shop.Save();

        //Xóa Cache về shop
        ClearCache("shop_" + id); 
        ClearCache("shopEntity_" + id);

        return RedirectToAction("Shop", "Config", new { id = shop.Id });
      }
      else
      {
        GetShopList();

        // Số lượng trang đơn hàng cần lấy
        var sllPage = new List<SelectListItem>();
        for (int i = 0; i <= 20; i++)
          sllPage.Add(new SelectListItem { Text = (i * 500).ToString(), Value = i.ToString() });
        ViewBag.DdlOrderPage = sllPage;

        ViewBag.Notification = Shared.RenderNotification("Nhập tất cả các thông tin bắt buộc", false);

        return View(shop);
      }
    }

    #endregion


    #region Cấu hình Color

    [Authorize]
    [HttpGet]
    public ActionResult Color(string type)
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_admin)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      var color = new ColorsEntity();
      if (Request.QueryString["colorId"] != null)
        color = DbColors.Get(Convert.ToInt32(Request.QueryString["colorId"]));

      FormColor(type);

      return View(color);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Color(string type, ColorsEntity model)
    {
      var color = new ColorsEntity();

      if (Request.QueryString["colorId"] != null)
        color = DbColors.Get(Convert.ToInt32(Request.QueryString["colorId"]));

      if (!string.IsNullOrEmpty(model.Name))
      {
        if (type == "ship")
          color.Name = string.Format("{0:0,0}", Convert.ToInt32(model.Name));
        else
          color.Name = model.Name.Trim().ToLower();
        color.Color = model.Color;
        color.Type = type;
        color.Save();

        return RedirectToAction("Color", "Config", new { @type = type });
      }
      else
      {
        ViewBag.Notification = Shared.RenderNotification("Nhập tất cả các thông tin bắt buộc", false);

        FormColor(type);

        return View();
      }
    }

    public void FormColor(string type)
    {
      ViewBag.ColorType = type;

      GetShopList();

      ViewBag.ColorList = DbColors.GetList(type);

      var colors = new string[] {
                "is-dark", "is-primary", "is-link", "is-info", "is-success", "is-warning", "is-danger",
                "is-primary is-light", "is-link is-light", "is-info is-light", "is-success is-light", "is-warning is-light", "is-danger is-light",
            };

      var tags = new List<StaticModel>();
      foreach (var color in colors)
      {
        tags.Add(new StaticModel() { name = color.Replace("is-", ""), color = color });
      }
      ViewBag.Tags = tags;
    }


    [Authorize]
    [HttpPost]
    public JsonResult ColorDelete(int id)
    {
      bool isRole = UserInfo.role.is_admin;
      bool isStatus = false;
      string message = string.Empty;

      if (isRole)
      {
        isStatus = DbColors.Delete(id);

        if (!isStatus)
          message = "Không thể xóa từ khóa này, vui lòng thử lại";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message });
    }


    #endregion


    #region SQL Server

    [Authorize]
    [HttpGet]
    public ActionResult SqlServer()
    {
      // USER: kiểm tra quyền
      if (UserInfo.role.is_admin && UserInfo.user == "thahnv")
      {
        return View();
      }
      else
      {
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });
      }
    }

    [HttpPost]
    [ValidateInput(false)]
    public ActionResult SqlServer(string sqlQuery)
    {
      if (Request["update"] != null)
      {
        bool isRun = DbConfig.SqlUpdate(sqlQuery, out string msg);

        if (isRun)
          ViewBag.Notification = Shared.RenderNotification("Đã chạy lênh SQL thành công", true);
        else
          ViewBag.Notification = Shared.RenderNotification("ERROR: " + msg, false);
      }
      else if (Request["get"] != null)
      {
        var data = DbConfig.SqlGet(sqlQuery);

        ViewBag.Data = data;
      }

      return View();
    }

    #endregion
  }
}
