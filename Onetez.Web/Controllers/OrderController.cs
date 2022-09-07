using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.Mvc;
using Onetez.Core.Libs;
using Onetez.Core.DbContext;
using Excel;

namespace Onetez.Web.Controllers
{
  public class OrderController : BaseController
  {
    [Authorize]
    [HttpGet]
    public ActionResult Index()
    {
      // USER: kiểm tra quyền
      if (!UserInfo.role.is_role)
        return RedirectToAction("PageRole", "Home", new { url = Request.RawUrl });

      ViewBag.IsAdmin = UserInfo.role.is_admin;
      ViewBag.IsPartner = UserInfo.role.is_partner;
      ViewBag.DateTimeNow = DateTimeNow;

      var tab = Request.QueryString["tab"] != null ? Convert.ToInt32(Request.QueryString["tab"]) : 1;
      int shopId = Request.QueryString["shop"] != null ? Convert.ToInt32(Request.QueryString["shop"]) : 0;
      var paging = Request.QueryString["p"] != null ? Convert.ToInt32(Request.QueryString["p"]) : 1;
      var key = Request.QueryString["key"] != null ? Request.QueryString["key"] : "";
      var sale = Request.QueryString["sale"] != null ? Request.QueryString["sale"] : "";
      var other = Request.QueryString["other"] != null ? Convert.ToInt32(Request.QueryString["other"]) : 0;
      var sort = Request.QueryString["sort"] != null ? Convert.ToInt32(Request.QueryString["sort"]) : 0;
      var partner = Request.QueryString["partner"] != null ? Convert.ToInt32(Request.QueryString["partner"]) : 0;

      var listShop = DbShop.GetList();
      if (shopId == 0 && listShop.Count > 0) shopId = listShop[0].Id;
      var slShop = new List<SelectListItem>();
      foreach (var sl in listShop)
        slShop.Add(new SelectListItem { Text = sl.Name, Value = sl.Id.ToString(), Selected = sl.Id == shopId });
      ViewBag.DdlShop = slShop;
      ViewBag.ShopId = shopId;

      if (UserInfo.role.is_partner)
      {
        if (UserInfo.user.StartsWith("jt"))
          partner = 15;
        else if (UserInfo.user.StartsWith("vt"))
          partner = 3;
        else if (UserInfo.user.StartsWith("ghtk"))
          partner = 1;
      }
      var cookiePartner = new HttpCookie("partner");
      cookiePartner.Value = partner.ToString();
      cookiePartner.Expires = DateTime.Now.AddYears(1);
      Response.Cookies.Add(cookiePartner);

      ViewBag.Keyword = key;


      // Danh sách nhân viên sale
      var listSale = DbOrder.GetListSale();
      var slSale = new List<SelectListItem>();
      slSale.Add(new SelectListItem { Text = "- nhân viên -", Value = "" });
      slSale.Add(new SelectListItem { Text = "Chưa có nhân viên", Value = "0", Selected = sale == "0" });
      foreach (string item in listSale)
        slSale.Add(new SelectListItem { Text = item, Value = item, Selected = sale == item });
      ViewBag.DdlSale = slSale;

      // Đối tác vận chuyển
      var slPartner = new List<SelectListItem>();
      slPartner.Add(new SelectListItem { Text = "- Đối tác vận chuyển -", Value = "0", Selected = sale == "0" });
      foreach (var sl in DbOrder.Partner())
        slPartner.Add(new SelectListItem { Text = sl.name, Value = sl.id.ToString(), Selected = partner == sl.id });
      ViewBag.DdlPartner = slPartner;

      // Bộ lọc
      var slOther = new List<SelectListItem>();
      slOther.Add(new SelectListItem { Text = "- lọc theo -", Value = "0", Selected = other == 0 });
      slOther.Add(new SelectListItem { Text = "Nhân viên không xử lý", Value = "1", Selected = other == 1 });
      slOther.Add(new SelectListItem { Text = "Shipper không xử lý", Value = "2", Selected = other == 2 });
      ViewBag.DdlOther = slOther;

      // Sắp xếp
      var slSort = new List<SelectListItem>();
      slSort.Add(new SelectListItem { Text = "- sắp xếp -", Value = "0", Selected = sort == 0 });
      slSort.Add(new SelectListItem { Text = "Xử lý gấp", Value = "1", Selected = sort == 1 });
      slSort.Add(new SelectListItem { Text = "Kiện vấn đề", Value = "2", Selected = sort == 2 });
      ViewBag.DdlSort = slSort;


      // Màu từ khóa
      ViewBag.ColorKeyword = DbColors.GetList("keyword");
      ViewBag.ColorShip = DbColors.GetList("ship");


      if (tab == 1)
      {
        int total = 0;

        var orderList = DbOrder.GetList(shopId, false, key, sale, partner, paging, total, sort, out total);
        ViewBag.Pagination = Shared.CreateCollection(total, 1, total, Request.RawUrl);

        return View(orderList);
      }
      else if (tab == 2)
      {
        int total = 0;

        var orderList = DbOrder.GetList(shopId, true, key, sale, partner, paging, PageSize, sort, out total);
        ViewBag.Pagination = Shared.CreateCollection(total, paging, PageSize, Request.RawUrl);

        return View(orderList);
      }
      else if (tab == 3)
      {
        int total = 0;

        var orderList = DbOrder.GetListComplain(shopId, key, sale, partner, paging, PageSize, out total);
        ViewBag.Pagination = Shared.CreateCollection(total, paging, PageSize, Request.RawUrl);

        return View(orderList);
      }
      else
      {
        int total = 0;

        var orderList = DbOrder.GetListRefund(shopId, key, other, partner, paging, PageSize, out total);
        ViewBag.Pagination = Shared.CreateCollection(total, paging, PageSize, Request.RawUrl);

        return View(orderList);
      }
    }


    [Authorize]
    [HttpPost]
    public ActionResult Index(int FilterTab, string FilterKeyword, string FilterSale, int FilterOther, int FilterSort, int FilterPartner)
    {
      int shopId = Request.QueryString["shop"] != null ? Convert.ToInt32(Request.QueryString["shop"]) : 0;

      return RedirectToAction("Index", "Order", new
      {
        tab = FilterTab,
        key = FilterKeyword,
        sale = FilterSale,
        other = FilterOther,
        sort = FilterSort,
        partner = FilterPartner,
        shop = shopId
      });
    }


    [Authorize]
    [HttpGet]
    public ActionResult Crawl()
    {


      return View();
    }


    [Authorize]
    [HttpGet]
    public ActionResult Import()
    {


      return View();
    }


    [Authorize]
    [HttpPost]
    public ActionResult Import(string date)
    {
      if (Request.Files.Count > 0)
      {
        string fileUpload = string.Empty;

        var file = Request.Files[0];
        if (file != null && file.ContentLength > 0)
        {
          string folder = "Upload/Excel/" + date + "/";
          string path = AppDomain.CurrentDomain.BaseDirectory + folder;
          if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

          var fileName = Path.GetFileName(ConvertString.RenameFile(file.FileName));
          file.SaveAs(path + fileName);

          fileUpload = path + fileName;
        }

        if (!string.IsNullOrEmpty(fileUpload))
        {
          var listOrder = ReadExcel(fileUpload, out string msg);

          if (listOrder.Count > 0)
          {
            int count = 0;
            foreach (var item in listOrder)
            {
              var order = DbOrder.GetByShipCode(item.code);
              if (order != null)
              {
                order.ShipPhone = item.sale;
                order.Save();

                count++;
              }
            }

            ViewBag.Notification = Shared.RenderNotification($"Đã cập nhật {count} đơn hàng!", true);
          }
          else
          {
            ViewBag.Notification = Shared.RenderNotification("Đã xãy ra lỗi! " + msg, false);
          }
        }
        else
        {
          ViewBag.Notification = Shared.RenderNotification("Không thể upload file, vui lòng thử lại", false);
        }
      }
      else
      {
        ViewBag.Notification = Shared.RenderNotification("Bạn chưa chọn file", false);
      }

      return View();
    }


    public List<ExcelOrderModel> ReadExcel(string file, out string msg)
    {
      var list = new List<ExcelOrderModel>();

      msg = string.Empty;

      try
      {
        string str = string.Empty;
        // Đọc file Excel
        var excelData = Workbook.Worksheets(file);
        if (excelData != null && excelData.Count() > 0)
        {
          var worksheet = excelData.FirstOrDefault();

          if (worksheet.NumberOfColumns >= 2)
          {
            foreach (var row in worksheet.Rows)
            {
              var model = new ExcelOrderModel();
              model.code = row.Cells[0].Text.Trim();
              model.sale = row.Cells[1].Text.Trim();

              if (!string.IsNullOrEmpty(model.code) && !string.IsNullOrEmpty(model.sale))
                list.Add(model);

              str += $"{model.code} \t {model.sale} <br/>";
            }
          }
        }
      }
      catch (Exception ex)
      {
        msg = ex.Message;
      }


      return list;
    }
  }


  public class ExcelOrderModel
  {
    public string code { get; set; }

    public string sale { get; set; }
  }
}