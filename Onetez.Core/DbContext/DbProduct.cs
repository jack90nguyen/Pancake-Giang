using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;

namespace Onetez.Core.DbContext
{
  public class DbProduct
  {
    public static ProductsEntity Get(string id)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Products
                   where c.Id == id
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static ProductsEntity GetByVariationId(string variationId)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Products
                   where c.VariationId == variationId
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }

    /// <summary>
    /// Lấy thông tin sản phẩm dựa vào Sheetcode
    /// </summary>
    /// <returns></returns>
    public static ProductsEntity GetBySheetCode(string sheetCode, int shopId)
    {
      if (string.IsNullOrEmpty(sheetCode))
        return null;

      sheetCode = sheetCode.ToLower().Trim();

      var db = new LinqMetaData();
      return (from c in db.Products
              where c.ShopId == shopId
              && c.ParentId == ""
              && c.SheetCode.ToLower().Trim() == sheetCode
              select c).FirstOrDefault();
    }

    /// <summary>
    /// Lấy thông tin sản phẩm dựa name, size, color
    /// </summary>
    public static ProductsEntity GetBySheetInfo(SheetsEntity sheet, bool findByName)
    {
      var db = new LinqMetaData();
      
      var productName = sheet.Product.ToLower().Trim();

      // Không có tên thì bỏ qua
      if (string.IsNullOrEmpty(productName))
        return null;

      // Tự động tìm theo tên/link và mã sản phẩm
      if (findByName)
      {
        // Tìm tự động theo Name/Link, Size, Color
        var listSizeColor = (from c in db.Products
                             where c.ShopId == sheet.ShopId
                             where c.ParentId == ""
                             && c.Size == sheet.Size
                             && c.Color == sheet.Color
                             && c.ProductDisplayId != ""
                             select c).ToList();

        // Lọc bớt để lấy đúng size và màu
        if(listSizeColor.Count == 0 && sheet.Color.Contains("màu "))
          listSizeColor = (from c in db.Products
                           where c.ShopId == sheet.ShopId
                           where c.ParentId == ""
                           && c.Size == sheet.Size
                           && c.Color == sheet.Color.Replace("màu ", "")
                           && c.ProductDisplayId != ""
                           select c).ToList();

        if (listSizeColor.Count > 0)
        {
          // Chuẩn hóa link
          if (sheet.Product.StartsWith("http") && sheet.Product.Contains("?"))
            productName = sheet.Product.Substring(0, sheet.Product.IndexOf("?"));

          foreach (var product in listSizeColor)
          {
            // Tìm sản phẩm có mã sản phẩm trong link
            if (productName.Contains(product.ProductDisplayId.Trim().ToLower()))
              return product;
          }
        }
      }
      // Tìm theo SheetCode đã cấu hình trong chuyển đổi
      else if (!productName.StartsWith("http"))
      {
        var listSheetCode = (from c in db.Products
                             where c.ShopId == sheet.ShopId
                             && c.ParentId == ""
                             && c.SheetCode.ToLower().Trim() == productName
                             select c).ToList();

        if (listSheetCode.Count > 0)
        {
          var listSizeColor = listSheetCode;
          if (!string.IsNullOrEmpty(sheet.Size))
            listSizeColor = listSizeColor.Where(x => x.Size == sheet.Size).ToList();
          if (!string.IsNullOrEmpty(sheet.Color))
            listSizeColor = listSizeColor.Where(x => x.Color == sheet.Color).ToList();

          if (listSizeColor.Count > 0)
            return listSizeColor.FirstOrDefault();
          else
            return listSheetCode.FirstOrDefault();
        }
      }

      return null;
    }


    public static List<ProductsEntity> GetList(int shopId)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Products
                   where c.ParentId == ""
                   orderby c.IsCombo descending, c.ProductName, c.DisplayId
                   select c).ToList();

      if (shopId != 0)
        query = query.Where(x => x.ShopId == shopId).ToList();

      return query;
    }


    public static List<ProductsEntity> GetList(int shopId, string parent)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Products
                   where c.ShopId == shopId
                   && c.ParentId == parent
                   orderby c.ShopId, c.ProductId, c.DisplayId
                   select c).ToList();

      return query;
    }


    public static List<string> GetListName(int shopId)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Products
                   where c.ParentId == ""
                   orderby c.ProductName
                   select c).ToList();

      var results = new List<string>();

      if (shopId != 0)
      {
        results = (from c in query
                   where c.ShopId == shopId
                   && c.ProductName != ""
                   select c.ProductName).Distinct().ToList();
      }
      else
      {
        results = (from c in query
                   where c.ProductName != ""
                   select c.ProductName).Distinct().ToList();
      }

      return results;
    }


    public static bool Delete(string id)
    {
      var current = Get(id);
      if (current != null)
      {
        return current.Delete();
      }
      else
        return false;
    }
  }
}
