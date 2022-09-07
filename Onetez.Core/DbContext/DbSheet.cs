using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Core.DbContext
{
  public class DbSheet
  {
    private static bool IsEnglish = Convert.ToBoolean(ConfigurationManager.AppSettings["IsEnglish"]);

    public static SheetsEntity Get(string id)
    {
      var db = new LinqMetaData();
      var query = from c in db.Sheets
                  where c.Id == id
                  select c;
      if (query.Count() > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static bool CheckExist(int shopId, string phone, string product, DateTime date)
    {
      try
      {
        var query = $"select Id from Sheets where shopId = {shopId}" +
        $" and Phone = '{phone}'" +
        $" and Product = N'{product}'";
        //$" and (select MONTH(Date) AS Month) = {date.Month}" +
        //$" and (select YEAR(Date) AS YEAR) = {date.Year}";
        var result = DbConfig.SqlGet(query);

        if (result.Rows.Count > 0)
          return true;
        else
          return false;
      }
      catch (Exception)
      {
        return false;
      }
    }


    /// <summary>
    /// Kiểm tra đã tạo đơn này chưa
    /// Nếu chưa thì trả về False
    /// </summary>
    public static bool CheckCreatedOrder(string id)
    {
      var db = new LinqMetaData();
      var query = from c in db.Sheets
                  where c.Id == id
                  && c.OrderId != ""
                  && c.StatusId == 1
                  select c;
      if (query.Count() > 0)
        return true;
      else
        return false;
    }


    public static SheetModel ConvertToModel(SheetsEntity item)
    {
      var model = new SheetModel()
      {
        Id = item.Id,
        Date = item.Date,
        DateStr = string.Format("{0:yyyy-MM-dd HH:mm:ss}", item.Date),
        Name = item.Name,
        Phone = item.Phone,
        Address = item.Address,
        Product = item.Product,
        Others = item.ProductOther,
        Link = item.Link,
        Note = item.Note,
        Size = item.Size,
        Color = item.Color,
        ShopId = item.ShopId,
        Process = item.ProcessProduct
      };

      return model;
    }


    public static SheetModel ConvertToModel(DataRow item)
    {
      var date = Convert.ToDateTime(item["Date"]);
      var model = new SheetModel()
      {
        Id = item["Id"].ToString(),
        Date = date,
        DateStr = string.Format("{0:yyyy-MM-dd HH:mm:ss}", date),
        Name = item["Name"].ToString(),
        Phone = item["Phone"].ToString(),
        Address = item["Address"].ToString(),
        Product = item["Product"].ToString(),
        Others = item["ProductOther"].ToString(),
        Link = item["Link"].ToString(),
        Note = item["Note"].ToString(),
        Size = item["Size"].ToString(),
        Color = item["color"].ToString(),
        ShopId = Convert.ToInt32(item["ShopId"])
      };

      return model;
    }


    /// <summary>
    /// Tìm kiếm đơn
    /// statusId = 0: đơn chưa xử lý | 1: đơn đã tạo | 2: đơn lỗi | 3: đơn hủy
    /// </summary>
    public static DataTable GetList(int shopId, int statusId, string phone, string product, string category,
      string userId, string dateStart, string dateEnd, int paging, int size, out int total)
    {
      int start = (paging - 1) * size + 1;
      int end = paging * size;

      // Bộ lọc tìm kiếm
      var sbFilter = new StringBuilder();
      sbFilter.Append($" where p.StatusId = {statusId}");
      if (shopId != 0)
        sbFilter.Append($" and p.ShopId = {shopId}");
      if (!string.IsNullOrEmpty(phone))
        sbFilter.Append($" and p.Phone Like N'%{phone}%'");
      if (product.ToLower() == "sản phẩm không xác định")
        sbFilter.Append($" and p.ProcessProduct = ''");
      else if(!string.IsNullOrEmpty(product))
        sbFilter.Append($" and p.ProcessProduct = N'{product}'");
      else if(!string.IsNullOrEmpty(category))
        sbFilter.Append($" and p.Category = N'{category}'");
      if (!string.IsNullOrEmpty(userId))
        sbFilter.Append($" and p.UserId = '{userId}'");
      if (!string.IsNullOrEmpty(dateStart))
        sbFilter.Append($" and Convert(Date,p.ProcessDate) >= Convert(Date,'{dateStart}')"); 
      if (!string.IsNullOrEmpty(dateEnd))
        sbFilter.Append($" and Convert(Date,p.ProcessDate) <= Convert(Date,'{dateEnd}')"); 

      // Lấy dữ liệu
      var sbSql = new StringBuilder();
      sbSql.Append(" SELECT * FROM (");
      sbSql.Append(" select ROW_NUMBER() OVER(ORDER BY p.ProcessDate desc) as row, p.*");
      sbSql.Append(" from Sheets p");
      sbSql.Append(sbFilter.ToString());
      sbSql.Append(" ) AS temp");
      sbSql.Append(" Where row >= " + start + " AND row <=" + end);

      var result = DbConfig.SqlGet(sbSql.ToString());

      if (size > 0)
      {
        // Lấy số lượng dữ liệu
        var sbTotal = new StringBuilder();
        sbTotal.Append(" SELECT COUNT(p.Id) as Total");
        sbTotal.Append(" FROM Sheets p");
        sbTotal.Append(sbFilter.ToString());

        var tableTotal = DbConfig.SqlGet(sbTotal.ToString());

        total = Convert.ToInt32(tableTotal.Rows[0][0]);
      }
      else
        total = result.Rows.Count;

      return result;
    }


    /// <summary>
    /// Tìm kiếm đơn
    /// statusId = 0: đơn chưa xử lý | 1: đơn đã tạo | 2: đơn lỗi | 3: đơn hủy
    /// </summary>
    public static List<SheetsEntity> GetList(string phone, string product, int shopId, 
      string userId, int statusId, string start, string end, int paging, int size, out int total)
    {
      if (product == null || product.ToLower() == "sản phẩm không xác định" || product.ToLower() == "unknown product")
        product = "0";

      var collection = new SheetsCollection();
      var filter = new PredicateExpression();
      if (!string.IsNullOrEmpty(userId))
        filter.AddWithAnd(SheetsFields.UserId == userId);
      if (shopId != 0)
        filter.AddWithAnd(SheetsFields.ShopId == shopId);
      if (statusId != 0)
        filter.AddWithAnd(SheetsFields.StatusId == statusId);
      else
      {
        filter.AddWithAnd(SheetsFields.StatusId == 0);
        filter.AddWithAnd(SheetsFields.UserId == ""); // → chưa gán nhân viên
      }
      if (!string.IsNullOrEmpty(start))
        filter.AddWithAnd(SheetsFields.ProcessDate >= Convert.ToDateTime(start));
      if (!string.IsNullOrEmpty(end))
        filter.AddWithAnd(SheetsFields.ProcessDate < Convert.ToDateTime(end).AddDays(1));

      collection.GetMulti(filter);

      var results = (from x in collection
                     orderby x.Date descending
                     select x).ToList();

      // Tìm theo SDT
      if (!string.IsNullOrEmpty(phone))
        results = results.Where(x => x.Phone.Contains(phone)).ToList();

      // Tìm theo sản phẩm hoặc link
      if (product == "0")
        results = results.Where(x => string.IsNullOrEmpty(x.ProcessProduct)).ToList();
      else if (!string.IsNullOrEmpty(product))
      {
        product = product.ToLower();
        results = results.Where(x => x.ProcessProduct.ToLower().Contains(product)).ToList();
      }

      total = results.Count;

      if (size > 0)
        return results.Skip(size * (paging - 1)).Take(size).ToList();
      else
        return results;
    }


    /// <summary>
    /// Tìm kiếm đơn đang xử lý → đã gán nhân viên
    /// statusId = 1: Đang xử lý | 2: Quá hạn | 0: Tất cả
    /// </summary>
    public static List<SheetsEntity> GetListProcess(string phone, string product, string category, int shopId,
      string userId, int processId, int statusId, string start, string end, int paging, int size, out int total)
    {
      if (product.ToLower() == "sản phẩm không xác định")
        product = "0";

      var collection = new SheetsCollection();
      var filter = new PredicateExpression();
      filter.AddWithAnd(SheetsFields.StatusId == 0);
      filter.AddWithAnd(SheetsFields.UserId != "");
      if (!string.IsNullOrEmpty(userId))
        filter.AddWithAnd(SheetsFields.UserId == userId);
      if (shopId != 0)
        filter.AddWithAnd(SheetsFields.ShopId == shopId);
      if (processId != 0)
        filter.AddWithAnd(SheetsFields.ProcessId == processId);
      if (statusId == 1) // Lấy đơn trong 2 ngày gần đây
        filter.AddWithAnd(SheetsFields.Date >= DateTime.Now.AddDays(-2));
      else if (statusId == 2) // Lấy đơn quá 2 ngày
        filter.AddWithAnd(SheetsFields.Date < DateTime.Now.AddDays(-2));
      if (!string.IsNullOrEmpty(start))
        filter.AddWithAnd(SheetsFields.ProcessDate >= Convert.ToDateTime(start));
      if (!string.IsNullOrEmpty(end))
        filter.AddWithAnd(SheetsFields.ProcessDate < Convert.ToDateTime(end).AddDays(1));
      if (!string.IsNullOrEmpty(category))
        filter.AddWithAnd(SheetsFields.Category == category);
      // Tìm theo sản phẩm hoặc link
      if (product == "0")
        filter.AddWithAnd(SheetsFields.ProcessProduct == string.Empty);
      else if (!string.IsNullOrEmpty(product))
        filter.AddWithAnd(SheetsFields.ProcessProduct == product);

      collection.GetMulti(filter);

      var results = (from x in collection
                     orderby x.ProcessId, x.Date descending
                     select x).ToList();

      // Tìm theo SDT
      if (!string.IsNullOrEmpty(phone))
        results = results.Where(x => x.Phone.Contains(phone)).ToList();

      total = results.Count;

      if (size > 0)
        return results.Skip(size * (paging - 1)).Take(size).ToList();
      else
        return results;
    }


    /// <summary>
    /// Danh sách đơn có người xử lý
    /// </summary>
    public static List<SheetsEntity> GetList(int shopId, string product, string start, string end)
    {
      if (product.ToLower() == "sản phẩm không xác định")
        product = "0";

      var collection = new SheetsCollection();
      var filter = new PredicateExpression();
      if (shopId != 0)
        filter.AddWithAnd(SheetsFields.ShopId == shopId);
      if (!string.IsNullOrEmpty(start))
        filter.AddWithAnd(SheetsFields.ProcessDate >= Convert.ToDateTime(start));
      if (!string.IsNullOrEmpty(end))
        filter.AddWithAnd(SheetsFields.ProcessDate < Convert.ToDateTime(end).AddDays(1));
      collection.GetMulti(filter);

      var results = (from x in collection
                     orderby x.ProcessId, x.Date descending
                     select x).ToList();

      // Tìm theo sản phẩm
      if (product == "0")
        results = results.Where(x => string.IsNullOrEmpty(x.ProcessProduct)).ToList();
      else if (!string.IsNullOrEmpty(product))
      {
        product = product.ToLower();
        results = results.Where(x => x.ProcessProduct.ToLower().Contains(product)).ToList();
      }

      return results;
    }


    public static List<SheetModel> GetListNewModel(int shopId)
    {
      var query = "select * from Sheets where StatusId = 0";
      if (shopId != 0)
        query += $" and shopId = {shopId}";
      query += "order by Date";
      var data = DbConfig.SqlGet(query);

      var results = new List<SheetModel>();
      foreach (DataRow item in data.Rows)
        results.Add(ConvertToModel(item));

      return results;
    }


    /// <summary>
    /// Danh sách tên sản phẩm của đơn trong 1 ngày
    /// </summary>
    public static List<string> GetListProductName(int shopId, string day)
    {
      var date = Convert.ToDateTime(day);
      var db = new LinqMetaData();
      var results = (from c in db.Sheets
                     where c.ShopId == shopId
                     && c.Date >= date
                     && c.Date < date.AddDays(1)
                     orderby c.ProcessProduct
                     select c.ProcessProduct).Distinct().ToList();

      return results;
    }


    public static int GetCount(int statusId)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Sheets
                   where c.StatusId == statusId
                   select c).Count();

      return query;
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


    public static List<string> ConvertLocation(string location)
    {
      var baragay = string.Empty;
      var province = string.Empty;
      var city = string.Empty;

      var data = location.Split('|').ToList();
      if (data.Count > 0)
        baragay = data[0];
      if (data.Count > 1)
        province = data[1];
      if (data.Count > 2)
        city = data[2];

      return new List<string>() { baragay, province, city };
    }


    #region Dữ liệu cố định


    // Tình trang: danh sách
    public static List<StaticModel> Process()
    {
      var list = new List<StaticModel>();

      if (IsEnglish)
      {
        list.Add(new StaticModel { id = 1, name = "New" });
        list.Add(new StaticModel { id = 2, name = "Accept" });
        list.Add(new StaticModel { id = 3, name = "Wrong phone" });
        list.Add(new StaticModel { id = 4, name = "Duplicate phone" });
        list.Add(new StaticModel { id = 5, name = "Schedule delivery" });
        list.Add(new StaticModel { id = 6, name = "Schedule send" });
        list.Add(new StaticModel { id = 14, name = "Call back later" });
        list.Add(new StaticModel { id = 7, name = "Busy" });
        list.Add(new StaticModel { id = 8, name = "Unable to contact" });
        list.Add(new StaticModel { id = 9, name = "Don't pick up the phone 1" });
        list.Add(new StaticModel { id = 10, name = "Don't pick up the phone 2" });
        list.Add(new StaticModel { id = 11, name = "Don't pick up the phone 3" });
        list.Add(new StaticModel { id = 12, name = "Don't pick up the phone 4" });
        list.Add(new StaticModel { id = 13, name = "Don't pick up the phone 5" });

        list.Add(new StaticModel { id = 15, name = "Restore" });

        list.Add(new StaticModel { id = 24, name = "Spam" });
        list.Add(new StaticModel { id = 25, name = "Test" });
        list.Add(new StaticModel { id = 26, name = "Cancel" });
      }
      else
      {
        list.Add(new StaticModel { id = 1, name = "Đơn mới" });
        list.Add(new StaticModel { id = 2, name = "Đã chốt" });
        list.Add(new StaticModel { id = 3, name = "Sai số" });
        list.Add(new StaticModel { id = 4, name = "Số trùng" });
        list.Add(new StaticModel { id = 5, name = "Hẹn giao lại" });
        list.Add(new StaticModel { id = 6, name = "Hẹn ngày gửi" });
        list.Add(new StaticModel { id = 14, name = "Hẹn gọi lại" });
        list.Add(new StaticModel { id = 7, name = "Máy bận" });
        list.Add(new StaticModel { id = 8, name = "Thuê bao" });
        list.Add(new StaticModel { id = 9, name = "KNM 1" });
        list.Add(new StaticModel { id = 10, name = "KNM 2" });
        list.Add(new StaticModel { id = 11, name = "KNM 3" });
        list.Add(new StaticModel { id = 12, name = "KNM 4" });
        list.Add(new StaticModel { id = 13, name = "KNM 5" });

        list.Add(new StaticModel { id = 15, name = "Khôi phục" });

        list.Add(new StaticModel { id = 24, name = "Số trêu" });
        list.Add(new StaticModel { id = 25, name = "Test" });
        list.Add(new StaticModel { id = 26, name = "Hủy" });
      }

      return list;
    }

    // Tình trạng: chi tiết
    public static StaticModel Process(int id)
    {
      var query = from s in Process()
                  where s.id == id
                  select s;
      if (query.Count() > 0)
        return query.FirstOrDefault();
      return new StaticModel();
    }


    #endregion
  }
}
