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
    public class BranchData
    {
        public static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();

        #region Danh Sach va Chi Tiet Chi Nhanh

        /// <summary>
        /// Chi tiet Chi Nhanh
        /// </summary>
        /// <returns></returns>
        public static BranchsEntity RetrieveBranch(long id)
        {
            var db = new LinqMetaData();
            var query = (from b in db.Branchs
                         where !b.IsDelete && b.BranchId == id
                         select b).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Chi tiet Chi Nhanh
        /// </summary>
        /// <returns></returns>
        public static BranchsEntity RetrieveAnyBranch(long id)
        {
            var db = new LinqMetaData();
            var query = (from b in db.Branchs
                         where b.BranchId == id
                         select b).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Tim Kiem Chi Nhanh
        /// size = 0: lấy tất cả
        /// </summary>
        /// <returns></returns>
        public static DataTable SearchBranch(int paging, int size, string keyword, out int total)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();
            int start = (paging - 1) * size + 1;
            int end = paging * size;

            //Tìm theo từ khóa
            string searchKey = string.Empty;
            if (!string.IsNullOrEmpty(keyword)) 
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += " and ";
                    searchKey += "(b.PropName Like N'%" + array[i] + "%' or b.PropAddress Like N'%" + array[i] + "%')";
                }
            }

            #region SQL loc ket qua

            string filter = " where b.IsDelete = '0'";
            if (!string.IsNullOrEmpty(keyword))
                filter += " and (" + searchKey + ")";

            #endregion


            //Lấy dữ liệu chính
            string sql = " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY b.PropName) as row,";
            sql += " b.BranchId,b.PropName,b.PropAddress,b.StatusId,b.DateStart,b.DateEnd,b.Staff";
            sql += " from Branchs b";
            sql += filter;
            sql += " ) as paging";
            if(size > 0)
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

                string sqlTotal = " SELECT COUNT(BranchId) as Total FROM Branchs b";
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
        /// Danh Sách Chi Nhánh
        /// </summary>
        /// <returns></returns>
        public static List<BranchsEntity> GetListBranch()
        {
            var db = new LinqMetaData();
            var query = (from b in db.Branchs
                         where !b.IsDelete
                         select b).ToList();
            return query;
        }

        /// <summary>
        /// Danh Sách Chi Nhánh theo User
        /// </summary>
        /// <returns></returns>
        public static List<BranchsEntity> GetListBranchOfUser(long userId)
        {
            var db = new LinqMetaData();
            var query = (from b in db.Branchs
                         join r in db.BranchRole on b.BranchId equals r.BranchId
                         where !b.IsDelete
                         select b).ToList();
            return query;
        }

        /// <summary>
        /// Trang thai
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> SelectListStatus()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Đang hoạt động", color = "Green" });
            list.Add(new SelectModel { id = "2", name = "Dừng hoạt động", color = "Red" });
            list.Add(new SelectModel { id = "3", name = "Tạm dừng hoạt động", color = "Gray" });
            return list;
        }

        /// <summary>
        /// Trang thai
        /// </summary>
        /// <returns></returns>
        public static SelectModel SelectCurrentStatus(string id)
        {
            var list = SelectListStatus();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return new SelectModel();
        }

        #endregion

        #region Danh Sach Nhan Su

        /// <summary>
        /// Phan quyen nhan su vao chi nhanh
        /// </summary>
        /// <returns></returns>
        public static bool AddRoleInBranch(long branchId, long userId, int roleId)
        {
            var db = new LinqMetaData();
            var query = (from r in db.BranchRole
                         where r.BranchId == branchId && r.UserId == userId && r.RoleId == roleId
                         select r).ToList();
            if (query.Count > 0)
            {
                return true;
            }
            else
            {
                var role = new BranchRoleEntity();
                role.BranchId = branchId;
                role.UserId = userId;
                role.RoleId = roleId;

                if (role.Save())
                {
                    //Cap nhat so luong nhan su
                    UpdateBranchStaff(branchId);

                    //Set chi nhanh mặc định cho nhân viên
                    var user = UserData.RetrieveUser(userId);
                    if(user != null)
                    {
                        user.BranchId = branchId;
                        user.Save();
                    }

                    return true;
                }
                else
                    return false;
            }
        }

        /// <summary>
        /// Xoa quyen nhan su vao chi nhanh
        /// </summary>
        /// <returns></returns>
        public static bool RemoveRoleInBranch(long branchId, long userId, int roleId)
        {
            var db = new LinqMetaData();
            var query = (from r in db.BranchRole
                         where r.BranchId == branchId && r.UserId == userId && r.RoleId == roleId
                         select r).ToList();
            if (query.Count > 0)
            {
                var role = query.FirstOrDefault();

                return role.Delete();
            }
            else
                return true;
        }

        /// <summary>
        /// Kiem tra quyen cua nhan vien trong chi nhanh
        /// listRoleId: role1,role2,role3
        /// 1: Giám đốc chi nhánh
        /// 2: Kế toán
        /// 3: Trường phòng kỹ thuật
        /// 4: Nhân viện kỹ thuật
        /// 5: Nhân viên Sale
        /// 6: Nhân viên CSKM
        /// 7: Nhân viên Marketing
        /// </summary>
        /// <returns></returns>
        public static bool CheckRoleInBranch(long branchId, long userId, string listRole)
        {
            //Danh sach quyen kieu Int
            List<int> listRoleId = new List<int>();

            //Chuyen string thanh array
            string[] arrayRole = listRole.Split(',');
            for (int i = 0; i < arrayRole.Length; i++)
            {
                if (!string.IsNullOrEmpty(arrayRole[i]))
                    listRoleId.Add(Convert.ToInt32(arrayRole[i]));
            }


            var db = new LinqMetaData();
            if (listRoleId.Count > 0)
            {
                //Kiem tra dung quyen
                var query = (from r in db.BranchRole
                             where r.BranchId == branchId && r.UserId == userId
                             && listRoleId.Contains(r.RoleId)
                             select r).ToList();
                if (query.Count > 0)
                    return true;
                else
                    return false;
            }
            else
            {
                //Co quyen la duoc, khong quan trong quyen gi
                var query = (from r in db.BranchRole
                             where r.BranchId == branchId && r.UserId == userId
                             select r).ToList();
                if (query.Count > 0)
                    return true;
                else
                    return false;
            }
        }

        /// <summary>
        /// Cap nhat so luong nhan vien cua chi nhanh
        /// </summary>
        /// <returns></returns>
        public static void UpdateBranchStaff(long id)
        {
            //Danh sach nhan su
            BranchData.SearchStaff(1, 0, id, 0, "", out int total);

            var branch = RetrieveBranch(id);
            branch.Staff = total;
            branch.Save();
        }

        /// <summary>
        /// Tim Kiem Nhan Su
        /// size = 0: lấy tất cả
        /// </summary>
        /// <returns></returns>
        public static DataTable SearchStaff(int paging, int size, long brandId, int role, string keyword, out int total)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();
            int start = (paging - 1) * size + 1;
            int end = paging * size;

            //Tìm theo từ khóa
            string searchKey = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += " and ";
                    searchKey += "(u.UserName Like N'%" + array[i] + "%' or u.EmailAddress Like N'%" + array[i] + "%')";
                }
            }

            #region SQL loc ket qua

            string filter = " where u.IsLocked = 0";
            //Tim theo quyen
            filter += " and (select COUNT(r.RoleId) from StaffRole r where r.UserId = u.UserId";
            if (role != 0)
                filter += " and r.RoleId = " + role;
            filter += " ) > 0";
            //Tim theo tu khoa
            if (!string.IsNullOrEmpty(keyword))
                filter += " and (" + searchKey + ")";

            #endregion

            //Danh sach quyen
            string sql = "with StaffRole(RoleId,UserId)";
            sql += " as (select RoleId,UserId from BranchRole where BranchId = "+brandId+")";
            //Lấy dữ liệu chính
            sql += " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY u.UserName) as row,";
            sql += " u.UserId,u.UserGuid,u.UserName,u.EmailAddress,u.LastOnline,u.IsActive,";
            sql += " u.FirstName,u.LastName,u.Phone,u.Address,u.Avatar,u.Birthday,";
            sql += " stuff((select ','+convert(nvarchar(100),r.RoleId) from StaffRole r where r.UserId = u.UserId order by r.RoleId for xml path ('')),1,1,'') as RoleList";
            sql += " from [User] u";
            sql += filter;
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

                //Danh sach quyen
                string sqlTotal = "with StaffRole(RoleId,UserId)";
                sqlTotal += " as (select RoleId,UserId from BranchRole where BranchId = " + brandId + ")";
                //Lấy dữ liệu chính
                sqlTotal += "SELECT COUNT(UserId) as Total FROM [User] u";
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
        /// Danh sách nhân sự trong 1 chi nhánh
        /// size = 0: lấy tất cả
        /// </summary>
        /// <returns></returns>
        public static List<UserModel> SearchStaff(long brandId, int role, string keyword)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();

            #region SQL loc ket qua

            //Tìm theo từ khóa
            string searchKey = string.Empty;
            if (!string.IsNullOrEmpty(keyword))
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += " and ";
                    searchKey += "(u.UserName Like N'%" + array[i] + "%' or u.EmailAddress Like N'%" + array[i] + "%')";
                }
            }

            string filter = " where u.IsLocked = 0";
            //Tim theo quyen
            filter += " and (select COUNT(r.RoleId) from StaffRole r where r.UserId = u.UserId";
            if (role != 0)
                filter += " and r.RoleId = " + role;
            filter += " ) > 0";
            //Tim theo tu khoa
            if (!string.IsNullOrEmpty(keyword))
                filter += " and (" + searchKey + ")";

            #endregion

            //Danh sach quyen
            string sql = "with StaffRole(RoleId,UserId)";
            sql += " as (select RoleId,UserId from BranchRole where BranchId = " + brandId + ")";
            //Lấy dữ liệu chính
            sql += " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY u.UserName) as row,";
            sql += " u.UserId,u.UserGuid,u.UserName,u.EmailAddress,u.LastOnline,u.IsActive,";
            sql += " u.FirstName,u.LastName,u.Phone,u.Address,u.Avatar,u.Birthday";
            sql += " from [User] u";
            sql += filter;
            sql += " ) as paging";

            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            var results = new List<UserModel>();
            foreach (DataRow item in myTable.Rows)
            {
                var model = new UserModel
                {
                    id = Convert.ToInt64(item["UserId"]),
                    guid = item["UserGuid"].ToString(),
                    username = item["UserName"].ToString(),
                    avatar = item["Avatar"].ToString(),
                    link = "/user/info/" + item["UserGuid"]
                };

                results.Add(model);
            }

            return results;
        }

        /// <summary>
        /// Danh sach quyen trong chi nhanh cua mot nhan vien
        /// </summary>
        /// <returns></returns>
        public static List<RoleModel> RoleListOfStaff(long branchId, long userId)
        {
            var list = new List<RoleModel>();

            var db = new LinqMetaData();
            var query = (from r in db.BranchRole
                         where r.BranchId == branchId && r.UserId == userId
                         orderby r.RoleId
                         select r).ToList();

            foreach (var item in query)
            {
                var role = SelectCurrentRole(item.RoleId.ToString());
                if (role != null)
                {
                    var row = new RoleModel();
                    row.id = item.RoleId.ToString();
                    row.name = role.name;

                    list.Add(row);
                }   
            }

            return list;
        }

        /// <summary>
        /// Danh Sach Phan Quyen
        /// </summary>
        /// <returns></returns>
        public static string StaffRoleList(string roleListId)
        {
            string[] array = roleListId.Split(',');

            string roleList = string.Empty;
            foreach (string id in array)
            {
                var role = SelectCurrentRole(id);
                if (role != null)
                    roleList += "<span class=\"role_"+role.id+" tag" + role.color + "\">" + role.name + "</span> ";
            }

            return roleList;
        }

        #endregion

        #region Du Lieu Co Dinh

        /// <summary>
        /// Phan Quyen
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> SelectListRole()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Giám đốc chi nhánh", color = "Red" });
            list.Add(new SelectModel { id = "2", name = "Kế toán", color = "Blue" });
            list.Add(new SelectModel { id = "3", name = "Trường phòng kỹ thuật", color = "Blue" });
            list.Add(new SelectModel { id = "4", name = "Nhân viên kỹ thuật", color = "Blue" });
            list.Add(new SelectModel { id = "5", name = "Nhân viên Sale", color = "Blue" });
            list.Add(new SelectModel { id = "6", name = "Nhân viên CSKH", color = "Blue" });
            list.Add(new SelectModel { id = "7", name = "Nhân viên Marketing", color = "Blue" });
            return list;
        }

        /// <summary>
        /// Phan Quyen
        /// </summary>
        /// <returns></returns>
        public static SelectModel SelectCurrentRole(string id)
        {
            var list = SelectListRole();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        #endregion
    }
}
