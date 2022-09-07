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
  public class SheetController : BaseController
  {
    #region Đơn chưa xử lý

    [Authorize]
    [HttpGet]
    public ActionResult News()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_admin)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      // Danh sách shop
      var listShop = DbShop.GetList();

      var tab = Request.Params["tab"] != null ? Convert.ToInt32(Request.Params["tab"]) : 1;
      var paging = Request.Params["p"] != null ? Convert.ToInt32(Request.Params["p"]) : 1;
      var shopId = Request.Params["shop"] != null ? Convert.ToInt32(Request.Params["shop"]) : 0;
      var phone = Request.Params["phone"] != null ? Request.Params["phone"] : "";
      var product = Request.Params["product"] != null ? Request.Params["product"].Replace("-", " ") : "";

      ViewBag.TabId = tab;
      ViewBag.ShopId = shopId;
      ViewBag.ConfigInfo = ConfigInfo;
      ViewBag.Phone = phone;
      ViewBag.Product = product;

      // Danh sách shop
      var slShop = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("- Chọn -"), Value = "0" } };
      foreach (var sl in listShop)
        slShop.Add(new SelectListItem
          { Text = sl.Name, Value = sl.Id.ToString(), Selected = sl.Id == shopId });
      ViewBag.DdlShop = slShop;


      // Danh sách nhân viên
      var slUser = new List<SelectListItem>();
      foreach (var item in DbUser.GetListStaff())
        slUser.Add(new SelectListItem { Text = item.Name, Value = item.UserId });
      ViewBag.DdlUser = slUser;


      ViewBag.ListOrder = DbSheet.GetList(phone, product, shopId, "", 0, "", "", paging, PageSize, out int total);
      ViewBag.Pagination = Shared.CreateCollection(total, paging, PageSize, Request.RawUrl);

      return View();
    }


    [Authorize]
    [HttpPost]
    public ActionResult News(int shopId, string phone, string product)
    {
      return RedirectToAction("News", "Sheet", 
        new { shop = shopId, phone = phone, product = product.Replace(" ", "-") });
    }


    #endregion


    #region Đơn đang xử lý

    [Authorize]
    [HttpGet]
    public ActionResult Process()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_role)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      // Danh sách shop
      var listShop = DbShop.GetList();

      var tab = Request.Params["tab"] != null ? Convert.ToInt32(Request.Params["tab"]) : 1;
      var paging = Request.Params["p"] != null ? Convert.ToInt32(Request.Params["p"]) : 1;
      var shopId = Request.Params["shop"] != null ? Convert.ToInt32(Request.Params["shop"]) : 0;
      var phone = Request.Params["phone"] != null ? Request.Params["phone"] : "";
      var product = Request.Params["product"] != null ? Request.Params["product"].Replace("-", " ") : "";
      var category = Request.Params["category"] != null ? Request.Params["category"].Replace("-", " ") : "";
      var userId = Request.Params["user"] != null ? Request.Params["user"] : "";
      var statusId = Request.Params["status"] != null ? Convert.ToInt32(Request.Params["status"]) : 1;
      var processId = Request.Params["process"] != null ? Convert.ToInt32(Request.Params["process"]) : 0;
      var start = Request.Params["start"] != null ? Request.Params["start"] : DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
      var end = Request.Params["end"] != null ? Request.Params["end"] : DateTime.Now.ToString("yyyy-MM-dd");

      // Nhân viên chỉ xem của mình
      if (UserInfo.role.is_staff && statusId == 1)
        userId = UserInfo.id;
      // Admin cho xem hết
      if (UserInfo.role.is_admin && Request.Params["status"] == null)
        statusId = 0;
      // Chuyển đến shop đầu tiên
      if (shopId == 0 && listShop.Count > 0)
        return RedirectToAction("Process", "Sheet", new { shop = listShop[0].Id });

      ViewBag.TabId = tab;
      ViewBag.ShopId = shopId;
      ViewBag.ConfigInfo = ConfigInfo;
      ViewBag.Phone = phone;
      ViewBag.Product = product;
      ViewBag.DateStart = start;
      ViewBag.DateEnd = end;

      // Danh sách shop
      var slShop = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("- Chọn -"), Value = "0" } };
      foreach (var sl in listShop)
        slShop.Add(new SelectListItem
          { Text = sl.Name, Value = sl.Id.ToString(), Selected = sl.Id == shopId });
      ViewBag.DdlShop = slShop;


      // Danh sách nhân viên
      var listUser = DbUser.GetListStaff();
      var slUser = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("Tất cả"), Value = "" } };
      foreach (var item in listUser)
        slUser.Add(new SelectListItem
        { Text = item.Name, Value = item.UserId, Selected = userId == item.UserId });
      ViewBag.DdlUser = slUser;
      ViewBag.ListUser = listUser;


      // Trạng thái xử lý
      var slProcess = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("Tất cả"), Value = "0" } };
      foreach (var item in DbSheet.Process())
        slProcess.Add(new SelectListItem
        { Text = item.name, Value = item.id.ToString(), Selected = processId == item.id });
      ViewBag.DdlProcess = slProcess;


      // Danh mục
      var slCategory = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("Tất cả"), Value = "" } };
      foreach (var item in ConfigInfo.SheetCategory.Replace("\n", "|").Split('|'))
        slCategory.Add(new SelectListItem
        { Text = item, Value = item.Trim(), Selected = item.Trim() == category });
      ViewBag.DdlCategory = slCategory;


      // Tình trạng xử lý
      var slStatus = new List<SelectListItem>();
      slStatus.Add(new SelectListItem { Text = "Đang xử lý", Value = "1", Selected = statusId == 1 });
      slStatus.Add(new SelectListItem { Text = "Quá hạn", Value = "2", Selected = statusId == 2 });
      if(UserInfo.role.is_admin)
        slStatus.Add(new SelectListItem { Text = TextData.Get("Tất cả"), Value = "0", Selected = statusId == 0 });
      ViewBag.DdlStatus = slStatus;

      ViewBag.ListOrder = DbSheet.GetListProcess(phone, product, category, shopId, userId, processId, statusId, start, end, paging, PageSize, out int total);
      ViewBag.Pagination = Shared.CreateCollection(total, paging, PageSize, Request.RawUrl);

      return View();
    }

    [Authorize]
    [HttpPost]
    public ActionResult Process(int shopId, string phone, string product, string category, string userId, int processId, int statusId, string start, string end)
    {
      if (product != null)
        product = product.Replace(" ", "-");
      if (category != null)
        category = category.Replace(" ", "-");

      return RedirectToAction("Process", "Sheet",
        new { shop = shopId, phone = phone, product = product, category = category, 
            user = userId, process = processId, status = statusId, start = start, end = end });
    }


    #endregion


    #region Đơn đã tạo, lỗi

    [Authorize]
    [HttpGet]
    public ActionResult Index()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_role)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      // Danh sách shop
      var listShop = DbShop.GetList();

      int tab = Request.Params["tab"] != null ? Convert.ToInt32(Request.Params["tab"]) : 1;
      int shopId = Request.Params["shop"] != null ? Convert.ToInt32(Request.Params["shop"]) : 0;
      int paging = Request.QueryString["p"] != null ? Convert.ToInt32(Request.QueryString["p"]) : 1;
      var phone = Request.QueryString["phone"] != null ? Request.QueryString["phone"].Replace("-", " ") : "";
      var product = Request.QueryString["product"] != null ? Request.QueryString["product"].Replace("-", " ") : "";
      var category = Request.Params["category"] != null ? Request.Params["category"].Replace("-", " ") : "";
      var userId = Request.Params["user"] != null ? Request.Params["user"] : "";
      int statusId = tab == 2 ? 1 : tab == 3 ? 2 : 3;
      var start = Request.Params["start"] != null ? Request.Params["start"] : DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd");
      var end = Request.Params["end"] != null ? Request.Params["end"] : DateTime.Now.ToString("yyyy-MM-dd");

      // Chuyển đến shop đầu tiên
      if (shopId == 0 && listShop.Count > 0 && tab != 1)
        return RedirectToAction("Index", "Sheet", new { tab = tab, shop = listShop[0].Id });

      ViewBag.ShopId = shopId;
      ViewBag.TabId = tab;
      ViewBag.UserInfo = UserInfo;
      ViewBag.Phone = phone;
      ViewBag.Product = product;
      ViewBag.DateStart = start;
      ViewBag.DateEnd = end;

      var slShop = new List<SelectListItem>() 
        { new SelectListItem { Text = TextData.Get("- Chọn -"), Value = "0" } };
      foreach (var sl in listShop)
        slShop.Add(new SelectListItem 
          { Text = sl.Name, Value = sl.Id.ToString(), Selected = sl.Id == shopId });
      ViewBag.DdlShop = slShop;

      // Danh sách nhân viên
      var listUser = DbUser.GetListStaff();
      var slUser = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("Tất cả"), Value = "" } };
      foreach (var item in listUser)
        slUser.Add(new SelectListItem
        { Text = item.Name, Value = item.UserId, Selected = userId == item.UserId });
      ViewBag.DdlUser = slUser;
      ViewBag.ListUser = listUser;

      // Danh mục
      var slCategory = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("Tất cả"), Value = "" } };
      foreach (var item in ConfigInfo.SheetCategory.Replace("\n", "|").Split('|'))
        slCategory.Add(new SelectListItem
        { Text = item, Value = item.Trim(), Selected = item.Trim() == category });
      ViewBag.DdlCategory = slCategory;

      var results = DbSheet.GetList(shopId, statusId, phone, product, category, userId, start, end, paging, PageSize, out int total);
      ViewBag.Pagination = Shared.CreateCollection(total, paging, PageSize, Request.RawUrl);

      return View(results);
    }

    [Authorize]
    [HttpPost]
    public ActionResult Index(int tabId, int shopId, string phone, string product, string category, string userId, string start, string end)
    {
      return RedirectToAction("Index", "Sheet",
        new { tab = tabId, shop = shopId, phone = phone, product = product.Replace(" ", "-"), category = category.Replace(" ", "-"), user = userId, start = start, end = end });
    }


    #endregion


    #region Chuyển đổi data

    [Authorize]
    [HttpGet]
    public ActionResult Product()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_role)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      // Danh sách shop
      var listShop = DbShop.GetList();

      int shopId = Request.Params["shop"] != null ? Convert.ToInt32(Request.Params["shop"]) : 0;
      var refresh = Request.QueryString["refresh"] != null ? true : false;

      // Chuyển đến shop đầu tiên
      if (shopId == 0 && listShop.Count > 0)
        return RedirectToAction("Product", "Sheet", new { shop = listShop[0].Id });

      var slShop = new List<SelectListItem>() {
        new SelectListItem { Text = TextData.Get("- Chọn -"), Value = "0" } };
      foreach (var sl in listShop)
        slShop.Add(new SelectListItem 
          { Text = sl.Name, Value = sl.Id.ToString(), Selected = sl.Id == shopId });
      ViewBag.DdlShop = slShop;
      ViewBag.ShopId = shopId;

      if (refresh)
      {
        // Số lượng sản phẩm mới cập nhật
        int countNew = 0;
        int countUpdate = 0;
        var cookieRefresh = Request.Cookies["ProductCount"];
        if (cookieRefresh != null)
        {
          countNew = Convert.ToInt32(cookieRefresh["New"]);
          countUpdate = Convert.ToInt32(cookieRefresh["Update"]);
        }
        else
        {
          cookieRefresh = new HttpCookie("ProductCount");
        }

        var shopList = shopId == 0 ? listShop : listShop.Where(x => x.Id == shopId).ToList();
        foreach (var shop in shopList)
        {
          var getProducts = PancakeApi.ProductList(DbShop.ConvertToModel(shop));

          if (getProducts != null && getProducts.Count > 0)
          {
            foreach (var item in getProducts)
            {
              var checkProduct = DbProduct.GetByVariationId(item.id);
              if (checkProduct == null)
              {
                string size = string.Empty;
                string color = string.Empty;
                if (item.fields != null)
                {
                  foreach (var field in item.fields)
                  {
                    if (field.name.ToLower().Contains("size"))
                      size = field.value.Trim().ToLower();
                    else if (field.name.ToLower().Contains("color") || field.name.ToLower().Contains("màu"))
                      color = field.value.Trim().ToLower();
                  }
                }

                var product = new ProductsEntity()
                {
                  Id = DbConfig.GenerateId(),
                  VariationId = item.id,
                  DisplayId = item.display_id,
                  ProductId = item.product_id,
                  ProductDisplayId = item.product.display_id,
                  ProductName = item.product.name,
                  Price = item.retail_price,
                  Fields = JsonConvert.SerializeObject(item.fields),
                  Size = size,
                  Color = color,
                  SheetCode = "",
                  ShopId = shop.Id
                };
                product.Save();

                countNew++;
              }
            }
          }
          else
          {
            countNew = -1;
            countUpdate = -1;
          }
        }

        // Cập nhật cookie số lượng sản phẩm mới cập nhật
        cookieRefresh.Values.Set("New", countNew.ToString());
        cookieRefresh.Values.Set("Update", countUpdate.ToString());
        cookieRefresh.Expires = DateTime.Now.AddMinutes(1);
        Response.Cookies.Add(cookieRefresh);

        return RedirectToAction("Product", "Sheet", new { shop = shopId });
      }
      else
      {
        var cookieRefresh = Request.Cookies["ProductCount"];
        if (cookieRefresh != null)
        {
          int countNew = Convert.ToInt32(cookieRefresh["New"]);
          int countUpdate = Convert.ToInt32(cookieRefresh["Update"]);

          if (countNew >= 0 || countUpdate >= 0)
            ViewBag.Notification = Shared.RenderNotification($"Đã cập nhật sản phẩm {countNew}", true);
          else
            ViewBag.Notification = Shared.RenderNotification("Không thể làm mới do API Pancake POS lỗi, vui lòng liên hệ Pancake POS để khắc phục", false);

          cookieRefresh.Expires = DateTime.Now.AddDays(-1);
          Response.AppendCookie(cookieRefresh);
        }
      }

      ViewBag.ListProduct = DbProduct.GetList(shopId);

      return View();
    }


    #endregion


    #region Thống kê

    [Authorize]
    [HttpGet]
    public ActionResult Report()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_admin && !UserInfo.role.is_report)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      // Danh sách shop
      var listShop = DbShop.GetList();

      var shopId = Request.Params["shop"] != null ? Convert.ToInt32(Request.Params["shop"]) : 0;
      var product = Request.Params["product"] != null ? Request.Params["product"].Replace("-", " ") : "";
      var start = Request.Params["start"] != null ? Request.Params["start"] : DateTime.Now.ToString("yyyy-MM-dd");
      var end = Request.Params["end"] != null ? Request.Params["end"] : DateTime.Now.ToString("yyyy-MM-dd");

      ViewBag.DateStart = start;
      ViewBag.DateEnd = end;
      ViewBag.Product = product;

      // Danh sách shop
      var slShop = new List<SelectListItem>()
        { new SelectListItem { Text = TextData.Get("- Chọn -"), Value = "0" } };
      foreach (var sl in listShop)
        slShop.Add(new SelectListItem
        { Text = sl.Name, Value = sl.Id.ToString(), Selected = sl.Id == shopId });
      ViewBag.DdlShop = slShop;
      ViewBag.ShopId = shopId;

      var results = DbSheet.GetList(shopId, product, start, end);
      ViewBag.AdsList = DbAds.GetList(shopId, product, start, end);

      return View(results);
    }


    [Authorize]
    [HttpPost]
    public ActionResult Report(int shopId, string product, string start, string end)
    {
      return RedirectToAction("Report", "Sheet",
        new { shop = shopId, product = product.Replace(" ", "-"), start = start, end = end });
    }


    #endregion
  }
}
