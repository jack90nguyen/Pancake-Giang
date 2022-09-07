using System.Web.Mvc;
using Onetez.Dal.EntityClasses;
using Onetez.Core.DbContext;
using System.Collections.Generic;
using Onetez.Dal.Models;
using System;

namespace Onetez.Web.Areas.APIv1.Controllers
{
  public class ProductController : BaseController
  {

    [Authorize]
    [HttpGet]
    public JsonResult GetCombo(string id)
    {
      var product = DbProduct.Get(id);
      var childs = new List<ProductConvert>();
      if (product != null)
      {
        foreach (var item in DbProduct.GetList(product.ShopId, product.Id))
        {
          childs.Add(new ProductConvert
          {
            id = item.Id,
            name = item.ProductName,
            msp = item.ProductDisplayId,
            mmm = item.DisplayId,
            price = item.Price.ToString(),
            quantity = item.Quantity
          });
        }

        return Json(new { id = product.Id, name = product.SheetCode, price = product.Discount, childs = childs }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        return Json(new { id = DbConfig.GenerateId(), name = "", price = 0, childs = childs }, JsonRequestBehavior.AllowGet);
      }
    }


    [Authorize]
    [HttpPost]
    public JsonResult Delete(string id)
    {
      bool isRole = UserInfo.role.is_role;
      bool isStatus = false;
      string message = string.Empty;

      if (isRole)
      {
        isStatus = DbProduct.Delete(id);

        if (!isStatus)
          message = "Không thể xóa sản phẩm này, vui lòng thử lại";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message });
    }

    [Authorize]
    [HttpPost]
    public JsonResult Duplicate(string id)
    {
      bool isRole = UserInfo.role.is_role;
      bool isStatus = false;
      string message = string.Empty;

      if (isRole)
      {
        var product = DbProduct.Get(id);

        var duplicate = new ProductsEntity()
        {
          Id = DbConfig.GenerateId(),
          VariationId = product.VariationId,
          DisplayId = product.DisplayId,
          ProductId = product.ProductId,
          ProductDisplayId = product.ProductDisplayId,
          ProductName = product.ProductName,
          Price = product.Price,
          Fields = product.Fields,
          Quantity = product.Quantity,
          ShopId = product.ShopId,
          SheetCode = ""
        };

        isStatus = duplicate.Save();

        if (!isStatus)
          message = "Không thể tạo bản sao cho sản phẩm này, vui lòng thử lại";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message });
    }


    [Authorize]
    [HttpPost]
    public JsonResult AddToCombo(string parent_id, string product_id)
    {
      bool isRole = UserInfo.role.is_role;
      bool isStatus = false;
      string message = string.Empty;
      var result = new ProductConvert();

      if (isRole)
      {
        var product = DbProduct.Get(product_id);
        if (product != null)
        {
          var duplicate = new ProductsEntity()
          {
            Id = DbConfig.GenerateId(),
            VariationId = product.VariationId,
            DisplayId = product.DisplayId,
            ProductId = product.ProductId,
            ProductDisplayId = product.ProductDisplayId,
            ProductName = product.ProductName,
            Price = product.Price,
            Fields = product.Fields,
            Quantity = product.Quantity,
            ShopId = product.ShopId,
            SheetCode = "",
            ParentId = parent_id
          };

          isStatus = duplicate.Save();

          result = new ProductConvert
          {
            id = duplicate.Id,
            name = duplicate.ProductName,
            msp = duplicate.ProductDisplayId,
            mmm = duplicate.DisplayId,
            price = duplicate.Price.ToString(),
            quantity = duplicate.Quantity
          };
        }
        else
          message = "Không thể thêm sản phẩm, vui lòng thử lại";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message, data = result });
    }

    [Authorize]
    [HttpPost]
    public JsonResult UpdateItemCombo(string id, string price, int quantity)
    {
      bool isRole = UserInfo.role.is_role;
      bool isStatus = false;
      string message = string.Empty;

      if (isRole)
      {
        var product = DbProduct.Get(id);
        if (product != null)
        {
          product.Price = Convert.ToInt32(price);
          product.Quantity = quantity;

          isStatus = product.Save();

          if (isStatus)
            message = $"Đã lưu sản phẩm '{product.ProductName}'";
          else
            message = "Không thể cập nhật, vui lòng thử lại";
        }
        else
          message = "Không thể cập nhật, vui lòng thử lại";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message });
    }

    [Authorize]
    [HttpPost]
    public JsonResult UpdateCombo(string id, string sheetcode, string discount, int shop_id)
    {
      bool isRole = UserInfo.role.is_role;
      bool isStatus = false;
      string message = string.Empty;

      if (isRole)
      {
        var product = DbProduct.Get(id);
        if (product == null)
          product = new ProductsEntity { Id = id };

        product.SheetCode = sheetcode;
        product.Discount = Convert.ToInt32(discount);
        product.IsCombo = true;
        product.Quantity = 1;
        product.ShopId = shop_id;

        isStatus = product.Save();

        if (isStatus)
          message = "Đã cật nhật thông tin sản phẩm";
        else
          message = "Không thể cập nhật, vui lòng thử lại";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message });
    }


    [Authorize]
    [HttpGet]
    public JsonResult GetProduct(string id)
    {
      var product = DbProduct.Get(id);
      if (product != null)
      {
        return Json(new
        {
          id = product.Id,
          code = product.SheetCode,
          name = product.ProductName,
          product = product.ProductDisplayId,
          display = product.DisplayId,
          size = product.Size,
          color = product.Color,
          price = product.Price,
          discount = product.Discount,
          quantity = product.Quantity,
          weight = product.Weight,
        }, JsonRequestBehavior.AllowGet);
      }
      else
      {
        return null;
      }
    }

    [Authorize]
    [HttpPost]
    public JsonResult UpdateProduct(string id, string code, string name, string product, string display, string size, string color, double price, double discount, int quantity, int weight, int shop)
    {
      bool isRole = UserInfo.role.is_role;
      bool isStatus = false;
      string message = string.Empty;

      if (isRole)
      {
        var item = DbProduct.Get(id);
        if (item == null)
        {
          id = DbConfig.GenerateId();
          item = new ProductsEntity();
          item.Id = id;
          item.ShopId = shop;
        }

        item.SheetCode = code != null ? code.Trim().Replace("  ", " ") : "";
        item.ProductName = name != null ? name.Trim() : "";
        item.ProductDisplayId = product != null ? product.Trim() : "";
        item.DisplayId = display != null ? display.Trim() : "";
        item.Color = color != null ? color.Trim() : "";
        item.Size = size != null ? size.Trim() : "";
        item.Price = price;
        item.Discount = discount;
        item.Quantity = quantity;
        item.Weight = weight;
        item.Save();

        isStatus = true;
        message = "Đã cật nhật thông tin sản phẩm";
      }
      else
        message = "Bạn không có quyền truy cập chức năng này";

      return Json(new { status = isStatus, msg = message, id = id });
    }
  }
}
