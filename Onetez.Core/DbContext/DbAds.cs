using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Core.DbContext
{
  public class DbAds
  {
    public static AdsEntity Get(long id)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Ads
                   where c.Id == id
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static List<AdsEntity> GetList(int shopId, int paging, int size, out int total)
    {
      var db = new LinqMetaData();

      var results = new List<AdsEntity>();
      
      if(shopId == 0)
      {
        results = (from c in db.Ads
                   orderby c.Day descending, c.Id descending
                   select c).ToList();
      }
      else
      {
        results = (from c in db.Ads
                   where c.ShopId == shopId
                   orderby c.Day descending, c.Id descending
                   select c).ToList();
      }

      total = results.Count;

      if (size > 0)
        return results.Skip(size * (paging - 1)).Take(size).ToList();
      else
        return results;
    }


    public static List<AdsEntity> GetList(int shopId, string product, string start, string end)
    {
      if (product.ToLower() == "sản phẩm không xác định")
        product = "0";

      var collection = new AdsCollection();
      var filter = new PredicateExpression();
      if (shopId != 0)
        filter.AddWithAnd(AdsFields.ShopId == shopId);
      if (!string.IsNullOrEmpty(start))
        filter.AddWithAnd(AdsFields.Day >= Convert.ToDateTime(start));
      if (!string.IsNullOrEmpty(end))
        filter.AddWithAnd(AdsFields.Day < Convert.ToDateTime(end).AddDays(1));
      collection.GetMulti(filter);

      var results = collection.OrderByDescending(x => x.Day).ToList();

      // Tìm theo sản phẩm
      if (!string.IsNullOrEmpty(product))
      {
        product = product.ToLower();
        results = results.Where(x => x.Product.ToLower().Contains(product)).ToList();
      }

      return results;
    }
  }
}
