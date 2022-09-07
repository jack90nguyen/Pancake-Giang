using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Onetez.Core.Libs;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;

namespace Onetez.Core.DbContext
{
  public class DbOrder
  {
    public static OrdersEntity Get(long id)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Orders
                   where c.Id == id
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static OrdersEntity GetByOrderId(string orderId, int shopId)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Orders
                   where c.OrderId == orderId
                   && c.ShopId == shopId
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static OrdersEntity GetByShipCode(string shipCode)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Orders
                   where c.ShipCode == shipCode
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static List<OrdersEntity> GetList(int shopId, bool isDone, string keyword, string sale, int partner, int paging, int size, int sort, out int total)
    {
      var db = new LinqMetaData();

      var listAll = new List<OrdersEntity>();

      if (isDone)
      {
        listAll = (from c in db.Orders
                   where c.Status == 5
                   orderby c.LastUpdate descending
                   select c).ToList();
      }
      else
      {
        listAll = (from c in db.Orders
                   where c.Status != 4 && c.Status != 5
                   && c.Status > 0
                   orderby c.Status, c.ShipInStock, c.ShipUpdate descending
                   select c).ToList();
      }

      if (shopId != 0)
        listAll = listAll.Where(x => x.ShopId == shopId).ToList();

      // Sắp xếp theo
      if (sort == 2)
      {
        listAll = (from c in listAll
                   orderby c.ShipStatus descending, c.ShipInStock, c.ShipUpdate descending
                   select c).ToList();
      }

      // Loc theo nhân viên xử lý
      if (!string.IsNullOrEmpty(sale))
      {
        if (sale == "0")
          listAll = listAll.Where(x => x.ShipPhone == "").ToList();
        else
          listAll = listAll.Where(x => x.ShipPhone == sale).ToList();
      }

      // Loc theo đối tác vận chuyển
      if (partner != 0)
      {
        listAll = listAll.Where(x => x.PartnerId == partner).ToList();
      }

      // Tìm theo từ khóa
      keyword = keyword.Trim().ToLower();
      if (!string.IsNullOrEmpty(keyword))
      {
        var listFilter = new List<OrdersEntity>();

        foreach (var item in listAll)
        {
          var content = item.ShipCode + item.BillPhone;
          if (content.ToLower().Contains(keyword))
            listFilter.Add(item);
        }

        listAll = listFilter;
      }

      total = listAll.Count;
      if (size > 0)
        return listAll.Skip(size * (paging - 1)).Take(size).ToList();
      else
        return listAll;
    }


    public static List<OrdersEntity> GetListComplain(int shopId, string keyword, string sale, int partner, int paging, int size, out int total)
    {
      var db = new LinqMetaData();

      var listAll = (from c in db.Orders
                     where c.Complain != ""
                     && c.ShipStatus != "Quét mã ký nhận"
                     orderby c.Status, c.ShipInStock, c.LastUpdate descending
                     select c).ToList();

      if (shopId != 0)
        listAll = listAll.Where(x => x.ShopId == shopId).ToList();

      // Loc theo nhân viên xử lý
      if (!string.IsNullOrEmpty(sale))
        listAll = listAll.Where(x => x.ShipPhone == sale).ToList();

      // Loc theo đối tác vận chuyển
      if (partner != 0)
        listAll = listAll.Where(x => x.PartnerId == partner).ToList();

      // Tìm theo từ khóa
      keyword = keyword.Trim();
      if (!string.IsNullOrEmpty(keyword))
      {
        var listFilter = new List<OrdersEntity>();

        foreach (var item in listAll)
        {
          if (item.ShipCode.Contains(keyword) || item.BillPhone.Contains(keyword))
            listFilter.Add(item);
        }

        listAll = listFilter;
      }

      total = listAll.Count;
      if (size > 0)
        return listAll.Skip(size * (paging - 1)).Take(size).ToList();
      else
        return listAll;
    }


    public static List<OrdersEntity> GetListRefund(int shopId, string keyword, int filter, int partner, int paging, int size, out int total)
    {
      var db = new LinqMetaData();

      var listAll = new List<OrdersEntity>();

      listAll = (from c in db.Orders
                 where c.Status == 4
                 orderby c.LastUpdate descending
                 select c).ToList();

      if (shopId != 0)
        listAll = listAll.Where(x => x.ShopId == shopId).ToList();

      if (filter == 1)
        listAll = listAll.Where(x => x.ShopLogs == "").ToList();
      else if (filter == 2)
      {
        var listFilter = new List<OrdersEntity>();

        foreach (var item in listAll)
        {
          var shipLogs = JsonConvert.DeserializeObject<List<Extend_Update>>(item.ShipLogs);
          if (shipLogs != null)
          {
            var shipLogsManual = shipLogs.Where(x => x.status_code == "000").ToList();
            if (shipLogsManual.Count == 0)
              listFilter.Add(item);
          }
        }

        listAll = listFilter;
      }

      keyword = keyword.Trim();

      if (!string.IsNullOrEmpty(keyword))
      {
        var listFilter = new List<OrdersEntity>();

        foreach (var item in listAll)
        {
          if (item.ShipCode.Contains(keyword) || item.BillPhone.Contains(keyword))
            listFilter.Add(item);
        }

        listAll = listFilter;
      }

      total = listAll.Count;
      if (size > 0)
        return listAll.Skip(size * (paging - 1)).Take(size).ToList();
      else
        return listAll;
    }


    public static List<string> GetListSale()
    {
      var db = new LinqMetaData();

      var listAll = (from c in db.Orders
                     where c.ShipPhone != ""
                     orderby c.ShipPhone
                     select c.ShipPhone).ToList().Distinct();

      return listAll.ToList();
    }


    public static OrderInfoModel ConvertToModel(OrdersEntity item)
    {
      var model = new OrderInfoModel()
      {
        Id = item.Id,
        ShopId = item.ShopId,
        BillName = item.BillName,
        BillPhone = item.BillPhone,
        Product = item.Product,
        ShipCode = item.ShipCode,
        ShipLogs = JsonConvert.DeserializeObject<List<Extend_Update>>(item.ShipLogs),
        ShipUpdate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", item.ShipUpdate),
        ShipPhone = item.ShipPhone,
        ShipInStock = item.ShipInStock < DateTime.Now ?
              ConvertString.ConvertDate(null, item.ShipInStock).Replace("trước", "") : "",
        ShipStatus = item.ShipStatus,
        ShopLogs = JsonConvert.DeserializeObject<List<ShopLogs>>(item.ShopLogs),
        ShopUpdate = string.Format("{0:yyyy-MM-dd HH:mm:ss}", item.ShopUpdate),
        Complain = item.Complain,
        Status = DbOrder.Status(item.Status),
        UserHandling = item.UserHandling,
        LastUpdate = item.LastUpdate
      };

      if (model.ShipLogs == null)
        model.ShipLogs = new List<Extend_Update>();
      if (model.ShopLogs == null)
        model.ShopLogs = new List<ShopLogs>();

      return model;
    }


    public static bool Delete(long id)
    {
      var current = Get(id);
      if (current != null)
      {
        return current.Delete();
      }
      else
        return false;
    }


    public static OrderItem ProductToOrder(ProductsEntity product)
    {
      var fields = JsonConvert.DeserializeObject<VariationField[]>(product.Fields);

      var detail = string.Empty;
      for (int i = 0; i < fields.Length; i++)
      {
        if (i > 0) detail += ", ";
        detail += $"{fields[i].name}: {fields[i].value}";
      }

      var orderItem = new OrderItem()
      {
        discount_each_product = 0,
        is_bonus_product = false,
        is_discount_percent = false,
        is_wholesale = false,
        one_time_product = false,
        product_id = product.ProductId,
        quantity = product.Quantity,
        variation_id = product.VariationId,
        variation_info = new VariationInfo()
        {
          detail = detail,
          display_id = product.DisplayId,
          name = product.ProductName,
          product_display_id = product.ProductDisplayId,
          fields = fields,
          retail_price = Convert.ToInt32(product.Price),
          weight = product.Weight
        }
      };

      return orderItem;
    }


    #region Dữ liệu cố định

    // Quyền: danh sách
    public static List<StaticModel> Status()
    {
      var list = new List<StaticModel>();

      list.Add(new StaticModel
      {
        id = 1,
        name = "Shipper xử lý gấp",
        color = "is-danger",
      });

      list.Add(new StaticModel
      {
        id = 2,
        name = "Shop xử lý gấp",
        color = "is-danger",
      });

      list.Add(new StaticModel
      {
        id = 3,
        name = "Đang xử lý",
        color = "is-info",
      });

      list.Add(new StaticModel
      {
        id = 4,
        name = "Chuyển hoàn",
        color = "is-warning",
      });

      list.Add(new StaticModel
      {
        id = 5,
        name = "Thành công",
        color = "is-success",
      });

      return list;
    }

    // Quyền: chi tiết
    public static StaticModel Status(int id)
    {
      var query = from s in Status()
                  where s.id == id
                  select s;
      if (query.Count() > 0)
        return query.FirstOrDefault();
      return null;
    }


    // Đối tác vận chuyển: danh sách
    public static List<StaticModel> Partner()
    {
      var list = new List<StaticModel>();

      list.Add(new StaticModel
      {
        id = 15,
        name = "J&T Express",
        color = "is-danger",
      });

      list.Add(new StaticModel
      {
        id = 3,
        name = "Viettel Post",
        color = "is-danger",
      });

      list.Add(new StaticModel
      {
        id = 1,
        name = "GHTK",
        color = "is-danger",
      });

      return list;
    }

    // Đối tác vận chuyển: chi tiết
    public static StaticModel Partner(int id)
    {
      var query = from s in Status()
                  where s.id == id
                  select s;
      if (query.Count() > 0)
        return query.FirstOrDefault();
      return null;
    }

    #endregion


    #region Xóa đơn trùng


    public static int XoaDonTrungLap()
    {
      // Lấy các mã vận đon trùng
      var sbSql = new StringBuilder();
      sbSql.Append(" select distinct * from (");
      sbSql.Append(" select o.ShipCode,");
      sbSql.Append(" (select COUNT(c.OrderId) from Orders c where c.ShipCode = o.ShipCode) as Qty");
      sbSql.Append(" from Orders o where o.ShipCode != ''");
      sbSql.Append(" ) as temp");
      sbSql.Append(" where Qty > 1");

      var con = new SqlConnection(DbConfig.SqlConnect);
      con.Open();
      var da = new SqlDataAdapter(sbSql.ToString(), con);
      var myTable = new DataTable();
      da.Fill(myTable);
      con.Close();

      if (myTable.Rows.Count > 0)
      {
        int countDelete = 0;

        foreach (DataRow row in myTable.Rows)
        {
          string mvd = row["ShipCode"].ToString();

          var db = new LinqMetaData();
          var query = (from c in db.Orders
                       where c.ShipCode == mvd
                       orderby c.Id descending
                       select c).ToList();

          if (query.Count > 1)
          {
            // Xóa đơn trùng, xóa đơn cũ hơn, để lại 1 đơn mới nhất
            for (int i = query.Count - 1; i > 0; i--)
            {
              var item = query[i];

              if (item.Delete())
                countDelete++;
            }
          }
        }

        return countDelete;
      }
      else
        return -1;
    }


    #endregion
  }
}
