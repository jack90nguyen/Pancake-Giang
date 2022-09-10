using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Caching;
using System.Threading.Tasks;
using System.Web.Mvc;
using Onetez.Core.Libs;
using Onetez.Core.DbContext;
using Onetez.Dal.Models;
using Onetez.Dal.EntityClasses;
using Onetez.Web.Modules;

namespace Onetez.Web.Areas.APIv1.Controllers
{
  public class SheetController : BaseController
  {
    private static int DayMax = Convert.ToInt32(ConfigurationManager.AppSettings["DayInSession"]) * -1;
    private static bool IsTesting = Convert.ToBoolean(ConfigurationManager.AppSettings["IsTesting"]);
    private static bool IsPancake = Convert.ToBoolean(ConfigurationManager.AppSettings["IsPancake"]);
    private static bool IsEnglish = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEnglish"]);
    private static bool IsSplit = Convert.ToBoolean(ConfigurationManager.AppSettings["IsSplit"]);

    [HttpGet]
    public JsonResult GetList(int? id)
    {
      var shopId = id == null ? 0 : id.Value;

      // Lấy các đơn chưa tạo
      var listSheet = DbSheet.GetListNewModel(shopId);

      // Nếu không có đơn mới thì đọc lại file Sheet
      if (listSheet.Count == 0)
      {
        string message = string.Empty;

        var shopList = new List<ShopsEntity>();
        if (shopId != 0)
        {
          var shop = DbShop.Get(shopId);
          if (shop != null)
            shopList.Add(shop);
        }
        else
          shopList = DbShop.GetList();

        foreach (var shopInfo in shopList)
        {
          var shop = DbShop.ConvertToModel(shopInfo);

          var sheetData = SheetApi.GetSheetData(shop, out string error);

          if (sheetData.Count == 0)
            message = error;
          else
          {
            // Chỉ lấy đơn trong khoản dayMax, để không lấy đơn quá cũ
            sheetData = (from x in sheetData
                         where x.Date >= DateTimeNow.AddDays(DayMax)
                         orderby x.Date
                         select x).ToList();
          }

          foreach (var item in sheetData)
          {
            // Lưu Sheet vào database
            CreateSheet(item, shop);
          }
        }

        //Lấy lại các đơn chưa tạo sau khi đã quét dữ liệu
        listSheet = DbSheet.GetListNewModel(shopId);
      }

      return Json(listSheet, JsonRequestBehavior.AllowGet);
    }


    [Authorize]
    [HttpGet]
    public JsonResult GetSheet(int id)
    {
      int total = 0;
      var error = string.Empty;
      var results = new List<SheetModel>();

      var shop = DbShop.Get(id);
      if (shop != null)
      {
        var shopModel = DbShop.ConvertToModel(shop);

        var sheetData = SheetApi.GetSheetData(shopModel, out error);

        // Chỉ lấy đơn trong khoản dayMax, để không lấy đơn quá cũ
        sheetData = (from x in sheetData
                     where x.Date >= DateTimeNow.AddDays(DayMax)
                     orderby x.Date
                     select x).ToList();

        total = sheetData.Count;

        results = sheetData;
      }

      return Json(new
      {
        total = total,
        message = error,
        data = results
      }, JsonRequestBehavior.AllowGet);
    }


    [HttpGet]
    public JsonResult RefreshData(int? id)
    {
      var total = 0;
      var message = string.Empty;

      var shopId = id == null ? 0 : id.Value;
      var shopList = new List<ShopsEntity>();
      if (shopId != 0)
      {
        var shop = DbShop.Get(shopId);
        if (shop != null)
          shopList.Add(shop);
      }
      else
        shopList = DbShop.GetList();

      foreach (var shopInfo in shopList)
      {
        var shop = DbShop.ConvertToModel(shopInfo);

        var sheets = SheetApi.GetSheetData(shop, out string error);

        if (sheets.Count == 0)
          message = error;
        else
        {
          // Chỉ lấy đơn trong khoản dayMax, để không lấy đơn quá cũ
          sheets = (from x in sheets
                    where x.Date >= DateTimeNow.AddDays(DayMax)
                    orderby x.Date
                    select x).ToList();
        }

        foreach (var item in sheets)
        {
          // Lưu Sheet vào database
          if (CreateSheet(item, shop))
            total++;

          if(total == 100)
            break;
        }
      }

      // Tự động chia đơn cho nhân viên
      if (IsSplit)
        AutoSplitOrder();

      return Json(new
      {
        status = total > 0,
        message = total > 0 ? $"Tìm thấy {total} đơn mới, tải lại trang để xem" : message
      }, JsonRequestBehavior.AllowGet);
    }

 
    // Lưu Sheet vào database
    private bool CreateSheet(SheetModel item, ShopModel shop)
    {
      // Kiểm tra có đơn này chưa
      if (DbSheet.CheckExist(item.ShopId, item.Phone, item.Product, item.Date) == false)
      {
        try
        {
          var sheet = new SheetsEntity()
          {
            Id = item.Id,
            Date = item.Date,
            Name = item.Name,
            Phone = item.Phone,
            Location = item.Address,
            Product = item.Product,
            ProductOther = item.Others,
            Link = item.Link,
            Note = item.Note,
            Size = item.Size,
            Color = item.Color,
            ShopId = item.ShopId,
            StatusId = 0,
            ProcessId = 1,
            ProcessDate = DateTime.Now
          };
          sheet.Save();

          new Task(() =>
          {
            // Lấy thông tin sản phẩm
            var product = DbProduct.GetBySheetInfo(sheet, shop.product_find_by_name);
            if (product != null)
            {
              sheet.ProcessCode = product.ProductDisplayId;
              sheet.ProcessProduct = product.ProductName.Trim();
              sheet.Revenue = product.Quantity * product.Price;
              sheet.Save();
            }
          }).Start();

          return true;
        }
        catch (Exception)
        {
          return false;
        }
      }

      return false;
    }


    private void AutoSplitOrder()
    {
      var userList = (from x in DbUser.GetListStaff() 
                      where x.RoleId == 2 
                      orderby Guid.NewGuid()
                      select x.UserId).ToList();
      var orderList = DbSheet.GetList(null, null, 0, "", 0, "", "", 1, 0, out int total);

      int u = 0;
      foreach (var sheet in orderList)
      {
        var user = userList[u];
        sheet.ProcessDate = DateTime.Now;
        sheet.UserId = user;
        sheet.Save();

        u++;
        if (u == userList.Count)
          u = 0;
      }
    }


    [Authorize]
    [HttpGet]
    public JsonResult GetCount(int status)
    {
      int count = DbSheet.GetCount(status);

      return Json(count, JsonRequestBehavior.AllowGet);
    }


    [HttpPost]
    public JsonResult CreateOrder(string id, bool? createOrderEmpty)
    {
      ObjectCache cache = MemoryCache.Default;

      var isStatus = false;
      var message = string.Empty;
      var error = string.Empty;
      OrderCreateResult.DataOrder result;
      ShopModel shop;

      var sheet = DbSheet.Get(id);

      if (sheet != null)
      {
        // Lấy thông tin shop
        var cache_key_shop = "shop_" + sheet.ShopId;
        if (cache[cache_key_shop] == null)
        {
          var shopInfo = DbShop.Get(sheet.ShopId);
          if (shopInfo != null)
          {
            shop = DbShop.ConvertToModel(shopInfo);
            SetCache(cache_key_shop, shop);
          }
          else
            shop = null;
        }
        else
          shop = (ShopModel)cache.Get(cache_key_shop);

        // Tạo đơn lên Google Sheet
        if(!IsPancake)
        {
          result = new OrderCreateResult.DataOrder();
          result.id = SheetApi.CreateRow(shop, sheet, out error);

          if (!string.IsNullOrEmpty(result.id))
          {
            if(UserInfo != null)
              sheet.ProcessLog = string.Format("{0:dd/MM, HH:mm}: Create order", DateTime.Now) + $" ({UserInfo.name})<br>" + sheet.ProcessLog;
            else
              sheet.ProcessLog = string.Format("{0:dd/MM, HH:mm}: Create order", DateTime.Now) + sheet.ProcessLog;

            sheet.Error = "";
            sheet.StatusId = 1;
            sheet.OrderId = result.id;
            sheet.AppleDate = DateTimeNow;
            sheet.Save();

            isStatus = true;
          }
          else
          {
            sheet.Error = error;
            sheet.StatusId = 2;
            sheet.Save();
          }

          return Json(new
          {
            status = isStatus,
            message = message,
            error = error,
            data = result
          });
        }

        if (string.IsNullOrEmpty(sheet.OrderId) && sheet.StatusId != 1)
        {
          if (shop != null)
          {
            // Tên sản phẩm
            var sheetProduct = sheet.Product.IndexOf("?") == -1 ? sheet.Product : sheet.Product.Substring(0, sheet.Product.IndexOf("?"));
            // Link sản phẩm
            var sheetLink = sheet.Link.IndexOf("?") == -1 ? sheet.Link : sheet.Link.Substring(0, sheet.Link.IndexOf("?"));
            // Ghi chú nội bộ
            var orderNote = string.Empty;

            // Kiểm tra có phải sản phẩm tạo đơn trống
            var productToOrderEmpty = DbShop.CheckProductToOrderEmpty(sheetProduct + sheetLink, shop.product_to_order_empty);

            // Yêu cầu tạo đơn trống
            if (createOrderEmpty == true)
              productToOrderEmpty = true;

            // Lấy thông tin sản phẩm chính
            ProductsEntity product = null;
            if (productToOrderEmpty == false)
              product = DbProduct.GetBySheetInfo(sheet, shop.product_find_by_name);

            // product_error_to_note = true → Lưu đơn không thể chuyển đổi vào ghi chú
            if (product != null || shop.product_error_to_note || productToOrderEmpty)
            {
              // Sản phẩm vào đơn
              var orderItems = new List<OrderItem>();

              // Tìm thấy sản phẩm
              if (product != null)
              {
                // Sản phẩm đầu tiên
                if (!product.IsCombo)
                {
                  orderItems.Add(DbOrder.ProductToOrder(product));
                }
                // Sản phẩm theo kiểu combo
                else
                {
                  var childs = DbProduct.GetList(product.ShopId, product.Id);
                  foreach (var item in childs)
                    orderItems.Add(DbOrder.ProductToOrder(item));
                }
              }

              // Các sản phẩm khác trong đơn
              var hasUpsale = false;
              if (!string.IsNullOrEmpty(sheet.ProductOther))
              {
                // Lưu sản phẩm phụ vào ghi chú nội bộ
                if (shop.product_other_in_note)
                {
                  orderNote += $"[Upsale] {sheet.ProductOther}\n";
                }
                // Lưu sản phẩm phụ vào đơn hàng
                else
                {
                  // Sửa lại chỉ lấy 1 sản phẩm thôi
                  var other = DbProduct.GetBySheetCode(sheet.ProductOther, sheet.ShopId);
                  if (other != null)
                  {
                    orderItems.Add(DbOrder.ProductToOrder(other));
                    hasUpsale = true;
                  }
                  //var productOthers = JsonConvert.DeserializeObject<List<string>>(sheet.ProductOther);
                  //if (productOthers != null)
                  //{
                  //  foreach (var sheetCode in productOthers)
                  //  {
                  //    var other = DbProduct.GetBySheetCode(sheetCode, sheet.ShopId);
                  //    if (other != null)
                  //      orderItems.Add(DbOrder.ProductToOrder(other));
                  //  }
                  //}
                }
              }

              // Sản phẩm chính
              orderNote += $"[Product] {sheetProduct}\n";
              // Size
              if (!string.IsNullOrEmpty(sheet.Size) && (sheet.Size != sheet.ProductOther || !hasUpsale))
                orderNote += $"[Size] {sheet.Size}\n";
              // Color
              if (!string.IsNullOrEmpty(sheet.Color) && (sheet.Color != sheet.ProductOther || !hasUpsale))
                orderNote += $"[Color] {sheet.Color}\n";
              // Ghi chú
              if (!string.IsNullOrEmpty(sheet.Note))
                orderNote += $"[Note] {sheet.Note}\n";
              // Giảm giá
              if (product != null && product.Discount > 0)
                orderNote += $"[Discount] {string.Format("{0:0,0}", product.Discount)}";
              if (hasUpsale)
                orderNote += $"[Upsale] {sheet.ProductOther}\n";

              // Thông tin địa chỉ
              var shippingAddress = new ShippingAddress()
              {
                address = sheet.Address,
                commune_id = null,
                district_id = null,
                province_id = null,
                post_code = null,
                country_code = "",
                full_address = sheet.Address,
                full_name = sheet.Name,
                phone_number = Shared.StandardizedPhone(sheet.Phone)
              };

              // Dữ liệu đơn hàng
              var order = new OrderModel()
              {
                bill_full_name = shippingAddress.full_name,
                bill_phone_number = shippingAddress.phone_number,
                is_free_shipping = true,
                received_at_shop = false,
                items = orderItems,
                note = orderNote,
                note_print = sheet.ProcessNote,
                warehouse_id = shop.warehouse_id,
                shipping_address = sheet.Address,
                shipping_fee = 0,
                shop_id = shop.id,
                total_discount = product != null ? Convert.ToInt32(product.Discount) : 0,
                warehouse_info = shop.warehouse_info,
                custom_id = null
              };

              if (IsTesting)
              {
                result = new OrderCreateResult.DataOrder { id = DateTime.Now.ToString("MMddHHmmss") };

                sheet.ProcessLog = string.Format("{0:dd/MM, HH:mm}: Create order", DateTime.Now) + $" ({UserInfo.name})<br>" + sheet.ProcessLog;

                sheet.Error = "";
                sheet.StatusId = 1;
                sheet.OrderId = result.id;
                sheet.ShopId = shop.id;
                sheet.AppleDate = DateTimeNow;
                sheet.Save();

                isStatus = true;

                if (product != null)
                  error = $"Code: {product.SheetCode} | Name: {product.ProductName} | Sie: {product.Size} | Color: {product.Color}";
              }
              else
              {
                // API push order to Pancake
                result = PancakeApi.CreateOrder(shop, order, out message, out error);

                if (result != null)
                {
                  if (UserInfo != null)
                    sheet.ProcessLog = string.Format("{0:dd/MM, HH:mm}: ", DateTime.Now) + (IsEnglish ? "Create Order" : "Create order") + $" ({UserInfo.name})<br>" + sheet.ProcessLog;
                  else
                    sheet.ProcessLog = string.Format("{0:dd/MM, HH:mm}: Create order", DateTime.Now) + sheet.ProcessLog;

                  sheet.Error = "";
                  sheet.StatusId = 1;
                  sheet.OrderId = result.id;
                  sheet.ShopId = shop.id;
                  sheet.AppleDate = DateTimeNow;
                  sheet.Save();

                  isStatus = true;

                  if (product != null)
                    error = $"Code: {product.SheetCode} | Name: {product.ProductName} | Sie: {product.Size} | Color: {product.Color}";
                }
                else
                {
                  sheet.Error = message;
                  sheet.StatusId = 2;
                  sheet.Save();
                }
              }
            }
            else
            {
              message = "Không thể chuyển đổi Sheet";
              result = null;

              sheet.Error = message;
              sheet.StatusId = 2;
              sheet.Save();
            }
          }
          else
          {
            message = "Không thể lấy thông tin Shop. Kiểm tra lại cấu hình trong phần [Chuyển đổi Data] hoặc [Cấu hình Shop]";
            result = null;

            sheet.Error = message;
            sheet.StatusId = 2;
            sheet.Save();
          }
        }
        else // Đơn hàng đã tạo rồi
        {
          sheet.Error = "";
          sheet.StatusId = 1;
          sheet.Save();

          result = new OrderCreateResult.DataOrder();
          result.id = sheet.OrderId;

          isStatus = true;
        }
      }
      else
      {
        message = "Đơn hàng không tồn tại !";
        result = null;
      }

      return Json(new
      {
        status = isStatus,
        message = message,
        error = error,
        data = result
      });
    }
    

    [HttpPost]
    public JsonResult UpdateOrder(string id, bool? createOrderEmpty)
    {
      ObjectCache cache = MemoryCache.Default;

      var isStatus = false;
      var message = string.Empty;
      var error = string.Empty;
      OrderCreateResult.DataOrder result = null;
      ShopModel shop;

      var sheet = DbSheet.Get(id);

      if (sheet != null)
      {
        // Lấy thông tin shop
        var cache_key_shop = "shop_" + sheet.ShopId;
        if (cache[cache_key_shop] == null)
        {
          var shopInfo = DbShop.Get(sheet.ShopId);
          if (shopInfo != null)
          {
            shop = DbShop.ConvertToModel(shopInfo);
            SetCache(cache_key_shop, shop);
          }
          else
            shop = null;
        }
        else
          shop = (ShopModel)cache.Get(cache_key_shop);

        // Tạo đơn lên Google Sheet
        if(!IsPancake)
        {
          result = new OrderCreateResult.DataOrder();
          result.id = SheetApi.UpdateRow(shop, sheet, out error);

          if (!string.IsNullOrEmpty(result.id))
            isStatus = true;

          return Json(new
          {
            status = isStatus,
            message = message,
            error = error,
            data = result
          });
        }

        // Viết hàm update cho POS
      }
      else
      {
        message = "Đơn hàng không tồn tại !";
        result = null;
      }

      return Json(new
      {
        status = isStatus,
        message = message,
        error = error,
        data = result
      });
    }


    [Authorize]
    [HttpPost]
    public JsonResult Delete(string id)
    {
      bool isStatus = false;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        current.ProcessLog = string.Format("{0:dd/MM, HH:mm}: ", DateTime.Now)
          + $"Hủy đơn ({UserInfo.name})<br>" + current.ProcessLog;
        current.CancelDate = DateTime.Now;

        current.StatusId = 3;
        isStatus = current.Save();
      }

      string message = isStatus ? "Đã hủy đơn " + id : "Không thể hủy đơn " + id;

      return Json(new { status = isStatus, msg = message });
    }


    [Authorize]
    [HttpPost]
    public JsonResult Remove(string id)
    {
      var isStatus = false;
      var message = string.Empty;

      if (UserInfo.role.is_admin)
      {
        var current = DbSheet.Get(id);
        if (current != null)
          isStatus = current.Delete();

        message = isStatus ? "Đã xóa đơn " + id : "Không thể xóa đơn " + id;
      }
      else
        message = "Bạn không đủ quyền hạn để xóa đơn";


      return Json(new { status = isStatus, msg = message });
    }


    [Authorize]
    [HttpPost]
    public JsonResult Restore(string id)
    {
      bool isStatus = false;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        var processInfo = DbSheet.Process(15);
        current.ProcessId = processInfo.id;
        current.ProcessLog = string.Format("{0:dd/MM, HH:mm}: ", DateTime.Now)
          + processInfo.name + $" ({UserInfo.name})<br>" + current.ProcessLog;

        current.StatusId = 0;
        current.OrderId = "";
        current.Error = "";
        isStatus = current.Save();
      }

      string message = isStatus ? "Đã khôi phục đơn " + id : "Không thể khôi phục đơn " + id;

      return Json(new { status = isStatus, msg = message });
    }


    [Authorize]
    [HttpPost]
    public JsonResult UserProcess(string[] users, string[] sheets)
    {
      var status = false;
      var message = string.Empty;

      if (users.Length > 0 && sheets.Length > 0)
      {
        users = users.OrderBy(x => Guid.NewGuid()).ToArray();

        int u = 0;
        for (int i = 0; i < sheets.Length; i++)
        {
          var user = users[u];
          var sheet = DbSheet.Get(sheets[i]);
          if(sheets != null)
          {
            sheet.ProcessDate = DateTime.Now;
            sheet.UserId = user;
            sheet.Save();

            u++;
            if (u == users.Length)
              u = 0;
          }
        }

        status = true;
        message = $"Divided {sheets.Length} orders among {users.Length} employees.";
      }
      else
        message = "Select the order and the employee to divide the order";

      return Json(new { status = status, msg = message });
    }


    [Authorize]
    [HttpPost]
    public JsonResult Unprocessed(string[] sheets)
    {
      var status = false;
      var message = string.Empty;

      if (sheets.Length > 0)
      {
        for (int i = 0; i < sheets.Length; i++)
        {
          var sheet = DbSheet.Get(sheets[i]);
          if(sheets != null)
          {
            sheet.ProcessDate = DateTime.Now;
            sheet.UserId = string.Empty;
            sheet.Save();
          }
        }

        status = true;
        message = "Moved to unprocessed.";
      }
      else
        message = "Select the order and the employee to divide the order";

      return Json(new { status = status, msg = message });
    }


    [Authorize]
    [HttpPost]
    public JsonResult ChangeProcess(string id, int process)
    {
      var status = false;
      var message = string.Empty;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        var processInfo = DbSheet.Process(process);
        current.ProcessId = process;
        current.ProcessLog = string.Format("{0:dd/MM, HH:mm}: ", DateTime.Now) 
          + processInfo.name + $" ({UserInfo.name})<br>" + current.ProcessLog;
        // Nếu chốt đơn mới tính cho người đó
        if (process == 2) 
          current.UserId = UserInfo.id;
        // Lưu thời gian gọi
        if (process >= 2 && process <= 14)
          current.ProcessCall = DateTime.Now;
        current.Save();

        status = true;
        message = current.ProcessLog;
      }
      else
        message = "Đơn này không tồn tại hoặc đã xóa";

      return Json(new { status = status, msg = message });
    }

    [Authorize]
    [HttpPost]
    public JsonResult ChangeCategory(string id, string category, int index)
    {
      var status = false;
      var message = string.Empty;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        if(index == 1)
          current.Category = category != null ? category : "";
        else if(index == 2)
          current.Category2 = category != null ? category : "";
        else if(index == 3)
          current.Category3 = category != null ? category : "";
        current.Save();

        status = true;
        message = "Đã cập nhật ghi chú cho đơn";
      }
      else
        message = "Đơn này không tồn tại hoặc đã xóa";

      return Json(new { status = status, msg = message });
    }

    [Authorize]
    [HttpPost]
    public JsonResult ChangeAddress(string id, string address)
    {
      var status = false;
      var message = string.Empty;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        current.Address = address != null ? address.Trim().Replace("\n", "") : "";
        current.Save();

        status = true;
        message = "Đã cập nhật địa chỉ cho đơn";
      }
      else
        message = "Đơn này không tồn tại hoặc đã xóa";

      return Json(new { status = status, msg = message });
    }

    [Authorize]
    [HttpPost]
    public JsonResult ChangeNote(string id, string note)
    {
      var status = false;
      var message = string.Empty;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        current.ProcessNote = note != null ? note.Trim() : "";
        current.Save();

        status = true;
        message = "Đã cập nhật ghi chú cho đơn";
      }
      else
        message = "Đơn này không tồn tại hoặc đã xóa";

      return Json(new { status = status, msg = message });
    }

    [Authorize]
    [HttpPost]
    public JsonResult ChangeRevenue(string id, string revenue)
    {
      var status = false;
      var message = string.Empty;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        current.Revenue = revenue != null ? Convert.ToDouble(revenue) : 0;
        current.Save();

        status = true;
        message = "Đã cập nhật doanh thu";
      }
      else
        message = "Đơn này không tồn tại hoặc đã xóa";

      return Json(new { status = status, msg = message });
    }

    [Authorize]
    [HttpPost]
    public JsonResult ChangeLocation(string id, string state, string city, string locality, string address, string pin)
    {
      var status = false;
      var message = string.Empty;

      var current = DbSheet.Get(id);
      if (current != null)
      {
        current.Location = $"{state}|{city}|{locality}|{address}|{pin}";
        current.Save();

        status = true;
        message = "Đã cập nhật địa chỉ";
      }
      else
        message = "Đơn này không tồn tại hoặc đã xóa";

      return Json(new { status = status, msg = message });
    }


    // Gán dữ liệu sản phẩm
    [HttpGet]
    public JsonResult ProcessProduct(int id)
    {
      var results = new List<SheetModel>();

      var shop = DbShop.Get(id);

      bool findByName = shop != null ? shop.ProductFindByName : true;

      // Đơn chưa gán sản phẩm
      var list = DbSheet.GetList(0, "0", null, null);
      foreach (var item in list)
      {
        // Lấy thông tin sản phẩm
        var product = DbProduct.GetBySheetInfo(item, findByName);
        if (product != null && item.Revenue == 0)
        {
          item.ProcessProduct = product.ProductName.Trim();
          item.Revenue = product.Quantity * product.Price;
          item.Save();
          results.Add(DbSheet.ConvertToModel(item));
        }
      }

      return Json(new
      { 
        total = list.Count,
        known = results.Count,
        unknown = list.Count - results.Count,
        data = results
      }, JsonRequestBehavior.AllowGet);
    }
  }
}
