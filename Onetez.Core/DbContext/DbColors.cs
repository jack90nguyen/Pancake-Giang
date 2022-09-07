using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.Linq;

namespace Onetez.Core.DbContext
{
  public class DbColors
  {
    public static ColorsEntity Get(int id)
    {
      var db = new LinqMetaData();
      var query = (from c in db.Colors
                   where c.Id == id
                   select c).ToList();
      if (query.Count > 0)
        return query.FirstOrDefault();
      else
        return null;
    }


    public static List<ColorsEntity> GetList(string type)
    {
      var db = new LinqMetaData();

      var query = (from c in db.Colors
                   where c.Type == type
                   orderby c.Name
                   select c).ToList();

      return query;
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
  }
}
