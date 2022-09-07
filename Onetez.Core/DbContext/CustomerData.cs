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
    public class CustomerData
    {
        public static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();
        public static int TimeUtc = Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]);

        #region Customer List & Detail

        /// <summary>
        /// Convert CustomersEntity to Json Model
        /// </summary>
        /// <returns></returns>
        public static CustomerModel ConvertCustomerToJson(CustomersEntity customer)
        {
            var result = new CustomerModel();

            //Thông tin cá nhân
            result.id = customer.CustomerId;
            result.name = customer.CusName;
            result.avatar = customer.CusAvatar;
            result.birthday = String.Format("{0:yyyy-MM-dd}", customer.Birthday);
            result.gender = customer.Gender;
            result.cmnd_code = customer.CmndCode;
            result.cmnd_address = customer.CmndAddress;
            result.cmnd_date = customer.CmndDate;
            //Thông tin liên hệ
            result.phone = customer.CusPhone;
            result.email = customer.CusEmail;
            result.facebook = customer.CusFacebook;
            result.address = customer.CusAddress;
            result.note = customer.CusNote;
            //Thuộc tính
            result.source = customer.SourceId;
            result.group = customer.GroupId;
            result.priority = customer.PriorityLevel;
            result.contact = customer.ContactStatusId;
            result.status = customer.StatusId;

            return result;
        }

    /// <summary>
    /// Thong tin cua khach hang theo ID
    /// </summary>
    /// <returns></returns>
    public static CustomersEntity GetOneCustomer(long customerId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Customers
                         where !c.IsDelete && c.CustomerId == customerId
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Thong tin cua khach hang theo ID
        /// Bao gom khach da xoa
        /// </summary>
        /// <returns></returns>
        public static CustomersEntity GetAnyCustomer(long customerId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Customers
                         where c.CustomerId == customerId
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Tim Kiem Khach Hang
        /// size = 0: lấy tất cả
        /// order: Sắp xếp | 0: Mới nhất | 1: Cũ nhất | 2: Tên | 3: Giới Tính | 4: Sinh Nhat | 5: Độ ưu tiên | 6: Tình trạng liên hệ
        /// </summary>
        /// <returns></returns>
        public static DataTable GetListCustomer(int paging, int size, bool isDelete, long branchId, long createUserId, 
            int gender, int sourceId, int groupId, string keyword, int order, int statusId, out int total)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();
            int start = (paging - 1) * size + 1;
            int end = paging * size;
            


            #region SQL loc ket qua

            string searchKey = string.Empty;
            bool notKey = (keyword.StartsWith("sn:")) ? true : false;
            if (!string.IsNullOrEmpty(keyword) && !notKey) //Tìm theo từ khóa
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += "  and ";

                    searchKey += "(";
                    searchKey += " c.CusName Like N'%" + array[i] + "%' or c.CusEmail Like N'%" + array[i] + "%'";
                    searchKey += " or c.CusPhone Like N'%" + array[i] + "%' or c.CusAddress Like N'%" + array[i] + "%'";
                    searchKey += " or c.CmndCode Like N'%" + array[i] + "%'or c.CusNote Like N'%" + array[i] + "%'";
                    searchKey += ")";
                }
            }

            string filter = " where c.IsDelete = " + (isDelete ? "1" : "0");
            //Tìm theo chi nhanh
            if (branchId != 0)
                filter += " and c.BranchId = " + branchId;
            //Tìm theo người tạo
            if (createUserId != 0)
                filter += " and c.CreateUserId = " + createUserId;
            //Tìm theo giới tính
            if (gender != 0)
                filter += " and c.Gender = " + gender;
            //Tìm theo nguồn khách
            if (sourceId != 0)
                filter += " and c.SourceId = " + sourceId;
            //Tìm theo đối tượng khách
            if (groupId != 0)
                filter += " and c.GroupId = " + groupId;
            //Tìm theo trang thai
            if (statusId != 0)
                filter += " and c.StatusId = " + statusId;
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(searchKey))
                filter += " and (" + searchKey + ")";
            //Tìm theo cú pháp
            if (notKey)
            {
                if (keyword.StartsWith("sn:"))
                    filter += " and MONTH (c.Birthday) = " + keyword.Replace("sn:", "");
            }


            #endregion


            string orderBy = "CreateDate desc";
            if (order == 1)
                orderBy = "CreateDate";
            else if (order == 2)
                orderBy = "CusName";
            else if (order == 3)
                orderBy = "Gender";
            else if (order == 4)
                orderBy = "MONTH (Birthday), DAY (Birthday)";
            else if (order == 5)
                orderBy = "PriorityLevel desc, ContactStatusId";
            else if (order == 6)
                orderBy = "ContactStatusId, PriorityLevel desc";

            string sql = string.Empty;
            //Lấy dữ liệu chính
            sql += " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY " + orderBy + ") as row,";
            sql += " c.CustomerId,c.CusName,c.CusEmail,c.CusPhone,c.CusAddress,c.CusAvatar,c.CusNote,c.Gender,";
            sql += " c.CreateDate,c.CreateUserId,c.Birthday,MONTH (c.Birthday) as bMonth, DAY (c.Birthday) as bDay,";
            sql += " c.BranchId,c.SourceId,c.GroupId,c.PriorityLevel,c.ContactStatusId,c.StatusId,";
            sql += " c.ContractId,c.ContractNumber";
            sql += " FROM Customers c";
            //Bo loc ket qua
            sql += filter;
            //Phan trang
            sql += " ) as paging";
            if (size > 0)
                sql += " Where row >= " + start + " AND row <=" + end;

            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            //Lấy tổng số lượng kết quả trả về
            if (size == 0)
            {
                total = myTable.Rows.Count;
            }
            else
            {
                _con.Open();
                string sqlTotal = string.Empty;
                sqlTotal += " SELECT COUNT(CustomerId) as Total FROM Customers c";
                //Bo loc ket qua
                sqlTotal += filter;

                var daTotal = new SqlDataAdapter(sqlTotal, _con);
                var tableTotal = new DataTable();
                daTotal.Fill(tableTotal);
                _con.Close();
                total = Convert.ToInt32(tableTotal.Rows[0][0]);
            }

            return myTable;
        }

        /// <summary>
        /// Thông kê số lượng Khach Hang
        /// </summary>
        /// <returns></returns>
        public static CustomerReportModel GetReportCustomer(bool isDelete, long branchId, long createUserId,
            int gender, int sourceId, int groupId, string keyword)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();

            #region SQL loc ket qua

            string searchKey = string.Empty;
            bool notKey = (keyword.StartsWith("sn:")) ? true : false;
            if (!string.IsNullOrEmpty(keyword) && !notKey) //Tìm theo từ khóa
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += "  and ";

                    searchKey += "(";
                    searchKey += " c.CusName Like N'%" + array[i] + "%' or c.CusEmail Like N'%" + array[i] + "%'";
                    searchKey += " or c.CusPhone Like N'%" + array[i] + "%' or c.CusAddress Like N'%" + array[i] + "%'";
                    searchKey += " or c.CmndCode Like N'%" + array[i] + "%'or c.CusNote Like N'%" + array[i] + "%'";
                    searchKey += ")";
                }
            }

            string filter = " where c.IsDelete = " + (isDelete ? "1" : "0");
            //Tìm theo chi nhanh
            if (branchId != 0)
                filter += " and c.BranchId = " + branchId;
            //Tìm theo người tạo
            if (createUserId != 0)
                filter += " and c.CreateUserId = " + createUserId;
            //Tìm theo giới tính
            if (gender != 0)
                filter += " and c.Gender = " + gender;
            //Tìm theo nguồn khách
            if (sourceId != 0)
                filter += " and c.SourceId = " + sourceId;
            //Tìm theo đối tượng khách
            if (groupId != 0)
                filter += " and c.GroupId = " + groupId;
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(searchKey))
                filter += " and (" + searchKey + ")";
            //Tìm theo cú pháp
            if (notKey)
            {
                if (keyword.StartsWith("sn:"))
                    filter += " and MONTH (c.Birthday) = " + keyword.Replace("sn:", "");
            }


            #endregion


            string sql = "with listAll(CustomerId,StatusId)";
            sql += " as (SELECT CustomerId,StatusId FROM Customers c";
            sql += filter;
            sql += " )";

            sql += " SELECT COUNT(CustomerId) as TotalCount,";
            sql += " (select COUNT(CustomerId) from listAll where StatusId = 1) as CountPotential,";
            sql += " (select COUNT(CustomerId) from listAll where StatusId = 2) as CountWorking,";
            sql += " (select COUNT(CustomerId) from listAll where StatusId = 3) as CountAccepted,";
            sql += " (select COUNT(CustomerId) from listAll where StatusId = 4) as CountSigned,";
            sql += " (select COUNT(CustomerId) from listAll where StatusId = 5) as CountCancel";
            sql += " FROM listAll";

            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            var report = new CustomerReportModel();
            if (myTable.Rows.Count > 0)
            {
                var row = myTable.Rows[0];
                report.TotalCount = row["TotalCount"] != DBNull.Value ? Convert.ToInt32(row["TotalCount"]) : 0;
                report.CountPotential = row["CountPotential"] != DBNull.Value ? Convert.ToInt32(row["CountPotential"]) : 0;
                report.CountWorking = row["CountWorking"] != DBNull.Value ? Convert.ToInt32(row["CountWorking"]) : 0;
                report.CountAccepted = row["CountAccepted"] != DBNull.Value ? Convert.ToInt32(row["CountAccepted"]) : 0;
                report.CountSigned = row["CountSigned"] != DBNull.Value ? Convert.ToInt32(row["CountSigned"]) : 0;
                report.CountCancel = row["CountCancel"] != DBNull.Value ? Convert.ToInt32(row["CountCancel"]) : 0;
            }

            return report;
        }

        #endregion


        #region Phiên Làm Việc với khách hàng

        /// <summary>
        /// Convert CustomersEntity to Json Model
        /// </summary>
        /// <returns></returns>
        public static SessionModel ConvertCustomerToJson(SessionsEntity current, CustomerModel customer)
        {
            var result = new SessionModel();

            result.id = current.SessionId;
            //Thông tin khách hàng
            result.customer = customer;
            //Tình trạng liên hệ
            result.status = current.StatusId;
            result.contact = current.ContactStatusId;
            result.contact_day = String.Format("{0:yyyy-MM-dd}", current.DateContact);
            result.contact_time = String.Format("{0:HH:mm}", current.DateContact);
            result.contact_note = current.ContactNote;
            //Thông tin công trình
            result.prop_type = current.PropType;
            result.prop_width = current.PropWidth;
            result.prop_height = current.PropHeight;
            result.prop_acreage = current.PropAcreage;
            result.prop_floors = current.PropFloors;
            result.prop_address = current.PropAddress;
            result.prop_note = current.PropNote;
            //Hình ảnh đính kèm
            result.images = Arrays.StringToArrayString(current.PropImages);

            return result;
        }

        /// <summary>
        /// Lấy phiên làm việc với khách hàng
        /// </summary>
        /// <returns></returns>
        public static SessionsEntity GetOneSessions(long sessionId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Sessions
                         where c.SessionId == sessionId && !c.IsDone
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Lấy phiên làm việc với khách hàng
        /// </summary>
        /// <returns></returns>
        public static SessionsEntity GetAnySessions(long sessionId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Sessions
                         where c.SessionId == sessionId
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Lấy phiên làm việc với khách hàng
        /// </summary>
        /// <returns></returns>
        public static SessionsEntity GetCustomerSessions(long customerId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Sessions
                         where c.CustomerId == customerId && !c.IsDone
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Tạo phiên làm việc mới
        /// </summary>
        /// <returns></returns>
        public static SessionsEntity CreateCustomerSessions(long branchId, long customerId, long userId)
        {
            var current = new SessionsEntity();
            current.BranchId = branchId;
            current.CustomerId = customerId;
            current.UserId = userId;
            current.DateCreate = DateTime.Now.AddHours(TimeUtc);
            current.Save();

            return current;
        }

        #endregion


        #region Dữ liệu cố định

        /// <summary>
        /// Gioi Tinh
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticListGender()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Nam" });
            list.Add(new SelectModel { id = "2", name = "Nữ" });
            return list;
        }

        /// <summary>
        /// Chi tiet gioi tinh
        /// </summary>
        /// <returns></returns>
        public static SelectModel GetDetailGender(string id)
        {
            var list = StaticListGender();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Nguon Khach Hang
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticCustomerSource()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Nguồn khác", color = "" });
            list.Add(new SelectModel { id = "2", name = "Facebook", color = "is-link" });
            list.Add(new SelectModel { id = "3", name = "Website", color = "is-info" });
            list.Add(new SelectModel { id = "4", name = "Quảng cáo", color = "is-danger" });
            list.Add(new SelectModel { id = "5", name = "Giới thiệu", color = "is-warning" });
            list.Add(new SelectModel { id = "6", name = "Tự tìm đến", color = "is-success" });
            return list;
        }

        /// <summary>
        /// Chi tiet Nguon Khach Hang
        /// </summary>
        /// <returns></returns>
        public static SelectModel GetDetailSource(string id)
        {
            var list = StaticCustomerSource();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Phân loại Nhom Khach Hang
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticCustomerGroup()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Thường", color = "is-primary" });
            list.Add(new SelectModel { id = "2", name = "Thân thiết", color = "is-success" });
            list.Add(new SelectModel { id = "3", name = "VIP", color = "is-danger" });
            list.Add(new SelectModel { id = "4", name = "Hạn chế", color = "is-warning" });
            list.Add(new SelectModel { id = "5", name = "Nhóm khác", color = "" });
            return list;
        }

        /// <summary>
        /// Chi tiet Phân loại Nhom Khach Hang
        /// </summary>
        /// <returns></returns>
        public static SelectModel GetDetailGroup(string id)
        {
            var list = StaticCustomerGroup();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Trang Thai Lien He
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticContactStatus()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Chưa liên hệ", color = "Red" });
            list.Add(new SelectModel { id = "2", name = "Không LH được", color = "Black" });
            list.Add(new SelectModel { id = "3", name = "Đã liên hệ", color = "Green" });
            return list;
        }

        /// <summary>
        /// Chi tiet Trang Thai Lien He
        /// </summary>
        /// <returns></returns>
        public static SelectModel GetDetailContactStatus(string id)
        {
            var list = StaticContactStatus();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }


        /// <summary>
        /// Tinh Trang Xu Ly
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticListStatus()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Tiền năng", color = "White" });
            list.Add(new SelectModel { id = "2", name = "Đang trao đổi", color = "Cyan" });
            list.Add(new SelectModel { id = "3", name = "Đã chốt", color = "Green" });
            list.Add(new SelectModel { id = "4", name = "Đã ký HĐ", color = "Blue" });
            list.Add(new SelectModel { id = "5", name = "Đã từ chối", color = "Black" });
            return list;
        }

        /// <summary>
        /// Chi tiet Tinh Trang Xu Ly
        /// </summary>
        /// <returns></returns>
        public static SelectModel GetDetailStatus(string id)
        {
            var list = StaticListStatus();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Mức độ ưu tiên
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticListPriority()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "-1", name = "Thấp", color = "is-dark" });
            list.Add(new SelectModel { id = "1", name = "Thường", color = "is-info" });
            list.Add(new SelectModel { id = "2", name = "Cao", color = "is-warning" });
            return list;
        }

        /// <summary>
        /// Chi tiet mức độ ưu tiên
        /// </summary>
        /// <returns></returns>
        public static SelectModel GetDetailPriority(string id)
        {
            var list = StaticListPriority();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        /// <summary>
        /// Sap xep khach hang
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticCustomerSort()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "0", name = "Mới đến cũ" });
            list.Add(new SelectModel { id = "1", name = "Cũ đến mới" });
            list.Add(new SelectModel { id = "5", name = "Độ ưu tiên cao" });
            list.Add(new SelectModel { id = "6", name = "Trạng thái liên hệ" });
            list.Add(new SelectModel { id = "2", name = "Tên khách" });
            list.Add(new SelectModel { id = "3", name = "Giới tính" });
            list.Add(new SelectModel { id = "4", name = "Ngày sinh" });
            return list;
        }

        #endregion
    }
}
