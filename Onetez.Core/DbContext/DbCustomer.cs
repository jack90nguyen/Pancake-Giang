using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Configuration;
using System.Web.Security;
using Onetez.Core.Libs;
using Onetez.Dal.CollectionClasses;
using Onetez.Dal.EntityClasses;
using Onetez.Dal.HelperClasses;
using Onetez.Dal.Linq;
using Onetez.Dal.Models;

namespace Onetez.Core.DbContext
{
    public class DbCustomer
    {
        public static CustomersEntity Get(string id)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Customers
                         where c.Id == id
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }


        public static List<CustomersEntity> GetList()
        {
            var db = new LinqMetaData();
            var query = (from c in db.Customers
                         where !c.IsDelete
                         orderby c.Name
                         select c).ToList();
            return query;
        }


        public static bool Delete(string id)
        {
            var current = Get(id);
            if (current != null)
            {
                current.IsDelete = true;
                return current.Save();
            }
            else
                return false;
        }
    }
}
