using System;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Onetez.Core.DbContext;
using System.Collections.Generic;
using Onetez.Core.Libs;
using Onetez.Web.Modules;
using Onetez.Dal.EntityClasses;
using Newtonsoft.Json;

namespace Onetez.Web.Controllers
{
  public class AdsController : BaseController
  {
    #region Danh sách Ads

    [Authorize]
    [HttpGet]
    public ActionResult Ads()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_admin && !UserInfo.role.is_ads)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      // Danh sách shop
      var listShop = DbShop.GetList();

      var tab = Request.Params["tab"] != null ? Convert.ToInt32(Request.Params["tab"]) : 1;
      var paging = Request.Params["p"] != null ? Convert.ToInt32(Request.Params["p"]) : 1;
      var shopId = Request.Params["shop"] != null ? Convert.ToInt32(Request.Params["shop"]) : 0;
      var product = Request.Params["product"] != null ? Request.Params["product"].Replace("-", " ") : "";
      var day = Request.Params["day"] != null ? Request.Params["day"] : "";

      // Chuyển đến shop đầu tiên
      if (shopId == 0 && listShop.Count > 0)
        return RedirectToAction("Ads", "Ads", new { shop = listShop[0].Id });

      ViewBag.TabId = tab;
      ViewBag.ShopId = shopId;
      ViewBag.ConfigInfo = ConfigInfo;
      ViewBag.Day = day;
      ViewBag.Product = product;

      // Danh sách shop
      var slShop = new List<SelectListItem>()
        { new SelectListItem { Text = "- Chọn -", Value = "0" } };
      foreach (var sl in listShop)
        slShop.Add(new SelectListItem
        { Text = sl.Name, Value = sl.Id.ToString(), Selected = sl.Id == shopId });
      ViewBag.DdlShop = slShop;

      var results = DbAds.GetList(shopId, paging, PageSize, out int total);
      ViewBag.Pagination = Shared.CreateCollection(total, paging, PageSize, Request.RawUrl);

      return View(results);
    }


    [Authorize]
    [HttpPost]
    public ActionResult Ads(int shopId, string day, string product)
    {
      return RedirectToAction("News", "Sheet",
        new { shop = shopId, day = day, product = product.Replace(" ", "-") });
    }


    #endregion


    [Authorize]
    [HttpPost]
    public JsonResult Delete(long id)
    {
      var isStatus = false;
      var message = string.Empty;

      var current = DbAds.Get(id);
      if (current != null)
        isStatus = current.Delete();

      message = isStatus ? "Đã xóa thành công" : "Không thể xóa mục này";

      return Json(new { status = isStatus, msg = message });
    }


    [Authorize]
    [HttpPost]
    public JsonResult UpdateItem(long id, string day, string product, double cost, double rate, int shop)
    {
      var isStatus = false;
      var message = string.Empty;

      var item = id != 0 ? DbAds.Get(id) : new AdsEntity();
      if (item != null)
      {
        item.Day = Convert.ToDateTime(day);
        item.Product = product.Trim();
        item.Cost = cost;
        item.Rate = rate;
        item.ShopId = shop;
        if (item.Save())
        {
          return Json(new
          {
            status = true,
            msg = "Đã cập nhật thành công !",
            id = item.Id,
            day = string.Format("{0:dd-MM-yyyy}", item.Day),
            product = product,
            cost = string.Format("{0:0,0.00} $", item.Cost),
            rate = string.Format("{0:0,0} ₫", item.Rate),
            money = string.Format("{0:0,0} ₫", item.Cost * item.Rate)
          });
        }
        else
          message = "Không thể cập nhật, vui lòng thử lại";
      }
      else
        message = "Dữ liệu không tồn tại hoặc đã bị xóa !";

      return Json(new { status = isStatus, msg = message });
    }


    [Authorize]
    [HttpGet]
    public JsonResult SheetProducts(int shop, string day)
    {
      var results = DbSheet.GetListProductName(shop, day);

      return Json(results, JsonRequestBehavior.AllowGet);
    }
  }
}
