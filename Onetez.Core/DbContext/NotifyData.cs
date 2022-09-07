using System;
using System.Collections.Generic;
using System.Configuration;
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
    public class NotifyData
    {
        public static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();
        public static int TimeUtc = Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]);

        #region Notify List & Detail

        /// <summary>
        /// Thong tin Notify theo ID
        /// </summary>
        /// <returns></returns>
        public static NotifysEntity RetrieveNotify(long id)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Notifys
                         where c.NotifyId == id
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Thong tin Notify theo ID
        /// </summary>
        /// <returns></returns>
        public static NotifysEntity RetrieveNotify(long id, long customerId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Notifys
                         where c.NotifyId == id && c.CustomerId == customerId
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }
        
        /// <summary>
        /// Lấy danh sách thông báo
        /// </summary>
        /// <returns></returns>
        public static DataTable RetrieveListNotify(int paging, int size, long customerId, out int total, bool getTotal)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();
            int start = (paging - 1) * size + 1;
            int end = paging * size;

            //Lấy dữ liệu chính
            string sql = "SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY n.CreateDate desc) as row,";
            sql += " n.NotifyId,n.PropName,n.PropLink,n.CreateDate,n.IsRead,n.PropType";
            sql += " FROM Notifys n";
            sql += " WHERE n.CustomerId = " + customerId;
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
                sqlTotal += " SELECT COUNT(n.NotifyId) as Total FROM Notifys n";
                sqlTotal += " WHERE n.CustomerId = " + customerId;
                var daTotal = new SqlDataAdapter(sqlTotal, _con);
                var tableTotal = new DataTable();
                daTotal.Fill(tableTotal);
                _con.Close();
                total = Convert.ToInt32(tableTotal.Rows[0][0]);
            }

            return myTable;
        }

        /// <summary>
        /// Lấy số lượng thông báo mới
        /// </summary>
        /// <returns></returns>
        public static int TotalNotify(long customerId)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();

            //Lấy dữ liệu chính
            string sql = "SELECT COUNT(NotifyId) as Total from Notifys where IsRead = '0' and CustomerId = " + customerId;

            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            return Convert.ToInt32(myTable.Rows[0][0]); ;
        }

        /// <summary>
        /// Lấy danh sách thông báo quản trị viên gửi
        /// </summary>
        /// <returns></returns>
        public static DataTable SearchNotify(int paging, int size, int type, string send, string keyword, out int total, bool getTotal)
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
                        searchKey += "  and ";
                    searchKey += "(n.PropName Like N'%" + array[i] + "%' or n.SendToList Like N'%" + array[i] + "%')";
                }
            }

            //Lấy dữ liệu chính
            string sql = " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY n.CreateDate desc) as row,";
            sql += " n.NotifyId,n.PropName,n.CreateDate,n.CreateUserId,n.SendToList,n.PropType,";
            sql += " u.UserGuid as StaffGuid,(u.LastName + ' ' + u.FirstName) as StaffName,u.Avatar as StaffAvatar";
            sql += " from Notifys n";
            sql += " join [User] u On u.UserId = n.CreateUserId";
            sql += " where n.CustomerId = 0 and n.ParentId Is Null";
            //Loc theo loai
            if (type != 0)
                sql += " and n.PropType = " + type;
            //Loc theo loai
            if (!string.IsNullOrEmpty(send))
                sql += " and n.SendToList Like '" + send + "%'";
            //Loc theo tu khoa
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
                sqlTotal += " SELECT COUNT(n.NotifyId) as Total FROM Notifys n";
                sqlTotal += " where n.CustomerId = 0 and n.ParentId Is Null";
                //Loc theo loai
                if (type != 0)
                    sqlTotal += " and n.PropType = " + type;
                //Loc theo loai
                if (!string.IsNullOrEmpty(send))
                    sqlTotal += " and n.SendToList Like '" + send + "%'";
                //Loc theo tu khoa
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

        /// <summary>
        /// Tạo Notify theo ID
        /// </summary>
        /// <returns></returns>
        public static bool CreateNotify(string name, string link, string note, long customerId, long createUserId, DateTime date, long? parentId, int type)
        {
            var item = new NotifysEntity();
            item.PropName = name;
            item.PropLink = link;
            item.PropNote = note;
            item.CustomerId = customerId;
            item.CreateUserId = createUserId;
            item.CreateDate = date;
            item.IsRead = false;
            item.ParentId = parentId;
            item.PropType = type;
            return item.Save();
        }

        /// <summary>
        /// Xóa thông báo con của 1 thông báo chính
        /// </summary>
        /// <returns></returns>
        public static bool DeleteNotify(long parentId)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();

            string sql = "Delete from Notifys where ParentId = " + parentId;
            SqlCommand command = new SqlCommand(sql, _con);
            int result = command.ExecuteNonQuery();
            _con.Close();
            if (result >= 1)
                return true;
            else
                return false;
        }

        #endregion

        #region Mẫu thông báo

        /// <summary>
        /// Gửi thông báo đến khách hàng
        /// </summary>
        /// <returns></returns>
        public static int SendNotifyToCustomer(NotifysEntity parent, string SendTo)
        {
            int count = 0;
            List<long> listId = new List<long>();

            //if (SendTo == "all")
            //{
            //    listId = CustomerData.ListCustomerId("", "");
            //}
            //else if (SendTo == "house")
            //{
            //    string toList = parent.SendToList.Replace("house:", "");
            //    listId = CustomerData.ListCustomerId(toList, "");
            //}
            //else if (SendTo == "customer")
            //{
            //    string toList = parent.SendToList.Replace("customer:", "");
            //    listId = CustomerData.ListCustomerId("", toList);
            //}

            //foreach (var id in listId)
            //{
            //    if (CreateNotify(parent.PropName, parent.PropLink, parent.PropNote, id, parent.CreateUserId, parent.CreateDate, parent.NotifyId, parent.PropType))
            //        count++;
            //}

            return count;
        }

        #endregion
    }
}
