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
using SD.LLBLGen.Pro.ORMSupportClasses;

namespace Onetez.Core.Data_v1
{
    public class AreaData
    {
        public static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();

        #region Area List & Detail

        /// <summary>
        /// Thong tin cua Area theo ID
        /// </summary>
        /// <returns></returns>
        public static AreasEntity RetrieveArea(long areaId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Areas
                         where !c.IsDelete && c.AreaId == areaId
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Danh sach Area con cua mot dia diem
        /// </summary>
        /// <returns></returns>
        public static List<AreasEntity> RetrieveListArea(long? parentId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Areas
                         where !c.IsDelete && c.ParentId == parentId
                         select c).ToList();
            return query;
        }

        /// <summary>
        /// Tim Kiem Area
        /// Tiêu chí: parentId -Keyword
        /// parentId: 0 : không xét
        /// keyword: Từ khóa
        /// </summary>
        /// <returns></returns>
        public static DataTable SearchArea(int paging, int size, long? parentId, string keyword, out int total, bool getTotal)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();
            int start = (paging - 1) * size + 1;
            int end = paging * size;
            string searchKey = string.Empty;
            if (!string.IsNullOrEmpty(keyword)) //Tìm theo từ khóa
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += " and ";
                    searchKey += "(c.AreaName Like N'%" + array[i] + "%')";
                }
            }

            //Lấy số lượng quận huyện
            string sql = "with filter(AreaId, ParentId)";
            sql += " as (select AreaId, ParentId from Areas";
            sql += " where IsDelete = '0' and ParentId is not Null)";
            //Lấy dữ liệu chính
            sql += " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY IsPin desc, AreaName) as row,";
            sql += " c.AreaId,c.AreaName,";
            sql += " (select COUNT(AreaId) from filter f where f.ParentId = c.AreaId) as TotalItem";
            sql += " from Areas c";
            sql += " where c.IsDelete = '0'";
            //Tìm theo mục cha
            if (parentId == null)
                sql += " and c.ParentId is Null";
            else if (parentId != null && parentId != 0)
                sql += " and c.ParentId = " + parentId;
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(keyword))
                sql += " and (" + searchKey + ")";
            sql += " ) as paging Where row >= " + start + " AND row <=" + end;
            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            //Lấy tổng số lượng kết quả trả về
            if (!getTotal)
            {
                total = myTable.Rows.Count;
            }
            else
            {
                _con.Open();
                string sqlTotal = string.Empty;
                sqlTotal += " SELECT COUNT(AreaId) as Total FROM Areas c";
                sqlTotal += " where c.IsDelete = '0'";
                //Tìm theo mục cha
                if (parentId == null)
                    sqlTotal += " and c.ParentId is Null";
                else if (parentId != null && parentId != 0)
                    sqlTotal += " and c.ParentId = " + parentId;
                //Tìm theo từ khóa
                if (!string.IsNullOrEmpty(keyword))
                    sqlTotal += " and (" + searchKey + ")";
                var daTotal = new SqlDataAdapter(sqlTotal, _con);
                var tableTotal = new DataTable();
                daTotal.Fill(tableTotal);
                _con.Close();
                total = Convert.ToInt32(tableTotal.Rows[0][0]);
            }

            return myTable;
        }

        // <summary>
        /// Area Theo JsResult
        /// </summary>
        /// <returns></returns>
        public static List<JsResultModel> TreeViewArea(long? parentId)
        {
            var list = new List<JsResultModel>();
            var childList = SearchArea(1, int.MaxValue, parentId, "", out int total, false);
            foreach (DataRow r in childList.Rows)
            {
                list.Add(new JsResultModel { ItemId = r["AreaId"].ToString(), ItemName = r["AreaName"].ToString() });
            }
            return list;
        }

        /// <summary>
        /// Lấy Tỉnh/Thành > Quận/Huyện > Phường/Xã của một địa điểm
        /// </summary>
        /// <returns></returns>
        public static string RetrieveAreaName(long areaId)
        {
            string location = string.Empty;
            var current = RetrieveArea(areaId);
            if(current != null)
            {
                location = current.AreaName;
                if (current.ParentId != null)
                    location = RetrieveAreaName(current.ParentId.Value) + " > " + location;
            }
            return location;
        }

        /// <summary>
        /// Xóa một địa điểm, xóa luôn những địa điểm con
        /// </summary>
        /// <returns></returns>
        public static bool DeleteArea(long areaId)
        {
            try
            {
                var current = AreaData.RetrieveArea(areaId);
                if (current != null)
                {
                    current.IsDelete = true;
                    current.Save();

                    var listChild = RetrieveListArea(areaId);
                    if (listChild.Count > 0)
                    {
                        foreach (var item in listChild)
                        {
                            DeleteArea(item.AreaId);
                        }
                    }
                }

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        #endregion
    }
}
;