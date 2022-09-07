using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Configuration;
using System.Web.Management;
using System.Web.Security;
using Newtonsoft.Json;
using Onetez.Core.Libs;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;

namespace Onetez.Core.DbContext
{
  public class DbShop
  {
    public static ShopsEntity Get(int id)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Shops
                   where c.Id == id
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static List<ShopsEntity> GetList()
    {
      var db = new LinqMetaData();
      var query = (from c in db.Shops
                   orderby c.Name
                   select c).ToList();

      return query;
    }


    public static ShopModel ConvertToModel(ShopsEntity item)
    {
      if (item == null)
        return null;

      var model = new ShopModel()
      {
        id = item.Id,
        //shop_id = Convert.ToInt32(item.ShopId),
        name = item.Name,
        api_key = item.ApiKey,
        warehouse_id = item.WarehouseId,
        warehouse_info = JsonConvert.DeserializeObject<WarehouseInfo>(item.WarehouseInfo),
        spreadsheet_id = item.SpreadsheetId,
        spreadsheet_tab = item.SpreadsheetTab,
        product_other_in_note = item.ProductOtherInNote,
        product_error_to_note = item.ProductErrorToNote,
        product_find_by_name = item.ProductFindByName,
        product_to_order_empty = item.ProductToOrderEmpty,
        sheet_columns = GetSheetColumns(item.SheetColumns)
      };

      return model;
    }

    /// <summary>
    /// Thứ tự cột trong Sheet
    /// </summary>
    public static ShopModel.SheetColumns GetSheetColumns(string sheetColumns)
    {
      var result = JsonConvert.DeserializeObject<ShopModel.SheetColumns>(sheetColumns);
      if (result == null)
        result = new ShopModel.SheetColumns()
        {
          date = 0,
          name = 1,
          phone = 2,
          address = 3,
          product = 4,
          link = 5,
          note = 6,
          size = 0,
          color = 0,
          other = 0,
          save = 9
        };

      return result;
    }


    /// <summary>
    /// Kiểm có phải sản phẩm tạo đơn trống
    /// </summary>
    public static bool CheckProductToOrderEmpty(string product, string productToOrderEmpty)
    {
      product = product.ToLower();
      var list = productToOrderEmpty.Split(';');
      foreach (var item in list)
      {
        if(!string.IsNullOrEmpty(item.Trim()) && product.Contains(item.Trim()))
          return true;
      }
      return false;
    }


    public static bool Delete(int id)
    {
      var current = Get(id);
      if (current != null)
      {
        return current.Delete();
      }
      else
        return false;
    }


    public static int MaxOrderPage()
    {
      var db = new LinqMetaData();
      var query = (from c in db.Shops
                   orderby c.OrderPage descending
                   select c.OrderPage);
      if (query.Count() > 0)
        return query.FirstOrDefault();
      else
        return 1;
    }
  }
}
