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
    public class TaskData
    {
        private static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();
        private static int TimeUtc = Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]);

        #region Task List & Detail

        /// <summary>
        /// Tạo nhiệm vụ chính mới - liên kết với hợp đồng
        /// </summary>
        /// <returns></returns>
        public static TasksEntity CreateTask(ContractsEntity contract, SessionsEntity session)
        {
            string detail = "- Loại công trình: " + contract.ConstructionType + " \n";
            detail += "- Chiều cao công trình: " + contract.ConstructionFloors + " \n";
            detail += "- Diện tích thiết kế: " + contract.ConstructionAcreage + " m²" + " \n";
            detail += "- Rộng x Dài: " + session.PropWidth + "m x " + session.PropHeight + "m \n";
            if(!string.IsNullOrEmpty(session.PropNote))
                detail += "\n ---------- YÊU CẦU ---------- \n" + session.PropNote + " \n";

            var current = new TasksEntity();
            current.ContractId = contract.ContractId;
            current.UserId = null;
            current.ParentId = null;
            current.StatusId = 1;
            current.Deadline = DateTime.Now.AddDays(7).AddHours(TimeUtc);
            current.DateStart = null;
            current.DateEnd = null;
            current.TaskName = "THIẾT KẾ HỢP ĐỒNG " + contract.ContractCode;
            current.TaskDetail = detail;
            current.TaskImages = session.PropImages;

            current.Save();

            return current;
        }

        /// <summary>
        /// Lấy chi tiết nhiệm vụ 
        /// </summary>
        /// <returns></returns>
        public static TasksEntity GetOneTask(long tasksId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Tasks
                         where !c.IsDelete && c.TaskId == tasksId
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Lấy nhiệm vụ chính liên kết với hợp đồng
        /// </summary>
        /// <returns></returns>
        public static TasksEntity GetTaskContract(long contractId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Tasks
                         where !c.IsDelete && c.ContractId == contractId && c.ParentId == null
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Tìm kiếm nhiệm vụ
        /// size = 0: lấy tất cả
        /// </summary>
        /// <returns></returns>
        public static DataTable GetListTask(int paging, int size, bool isDelete, long branchId, long? parentId,
            string dataStart, string dateEnd, string keyword, int statusId, long staffId, out int total)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();
            int start = (paging - 1) * size + 1;
            int end = paging * size;


            #region SQL loc ket qua

            string searchKey = string.Empty;
            if (!string.IsNullOrEmpty(keyword)) //Tìm theo từ khóa
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += " and ";

                    searchKey += "(";
                    searchKey += " c.TaskName Like N'%" + array[i] + "%' or c.TaskDetail Like N'%" + array[i] + "%'";
                    searchKey += ")";
                }
            }

            string filter = " where c.IsDelete = " + (isDelete ? "1" : "0");
            //Tìm theo chi nhanh
            if (branchId != 0)
                filter += " and c.BranchId = " + branchId;
            //Tìm theo người thực hiện
            if (staffId != 0)
                filter += " and c.UserId = " + staffId;
            //Tìm theo nhiệm vụ chính
            if (parentId != null)
                filter += " and c.ParentId = " + parentId;
            else
                filter += " and c.ParentId Is Null";
            //Tìm theo trang thai
            if (statusId != 0)
                filter += " and c.StatusId = " + statusId;
            //Tìm từ ngày
            if (!string.IsNullOrEmpty(dataStart))
                filter += " and Convert(Date,'" + dataStart + "')<=Convert(Date,c.Deadline)";
            //Tìm đến ngày
            if (!string.IsNullOrEmpty(dateEnd))
                filter += " and Convert(Date,'" + dateEnd + "')>=Convert(Date,c.Deadline)";
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(searchKey))
                filter += " and (" + searchKey + ")";

            #endregion

            string sql = string.Empty;
            //Lấy danh sách nhiệm vụ con
            sql += " with listTask(TaskId,StatusId,ContractId)";
            sql += " as (SELECT TaskId,StatusId,ContractId FROM Tasks";
            sql += " where IsDelete = 0 and ParentId Is Not Null)";
            //Lấy dữ liệu chính
            sql += " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY c.Deadline desc) as row,";
            sql += " c.TaskId,c.ContractId,c.UserId,c.ParentId,c.StatusId,";
            sql += " c.Deadline,c.DateStart,c.DateEnd,c.TaskName,";
            sql += " (select COUNT(TaskId) from listTask t where c.ContractId = t.ContractId) as TotalTask,";
            sql += " (select COUNT(TaskId) from listTask t where c.ContractId = t.ContractId and t.StatusId = 3) as DoneTask";
            sql += " FROM Tasks c";
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
                sqlTotal += " SELECT COUNT(TaskId) as Total FROM Tasks c";
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
        /// Lấy chi tiết nhiệm vụ 
        /// </summary>
        /// <returns></returns>
        public static List<TasksEntity> GetListTask(long parentId)
        {
            var db = new LinqMetaData();
            var list = (from c in db.Tasks
                         where !c.IsDelete && c.ParentId == parentId
                         orderby c.Deadline
                         select c).ToList();
            return list;
        }

        /// <summary>
        /// Xóa nhiệm vụ con
        /// </summary>
        /// <returns></returns>
        public static bool DeleteTaskSub(long tasksId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Tasks
                         where !c.IsDelete && c.ParentId == tasksId
                         select c).ToList();
            if (query.Count > 0)
            {
                foreach (var item in query)
                {
                    item.IsDelete = true;
                    item.Save();

                    //Xóa nhiệm vụ con
                    DeleteTaskSub(item.TaskId);
                }

                return true;
            }
            else
                return true;
        }

        /// <summary>
        /// Convert TasksEntity to Json Model
        /// </summary>
        /// <returns></returns>
        public static TaskModel ConvertTaskToJson(TasksEntity current)
        {
            var result = new TaskModel();

            result.id = current.TaskId;
            result.name = current.TaskName;
            result.detail = current.TaskDetail;
            result.deadline = ConvertString.ConvertDate(current.Deadline);
            result.deadline_day = String.Format("{0:yyyy-MM-dd}", current.Deadline);
            result.deadline_time = String.Format("{0:HH:mm}", current.Deadline);
            result.parent = current.ParentId;
            result.status_id = current.StatusId;
            result.status_icon = StaticData.DetailStatusTask(current.StatusId.ToString()).icon;
            //Hình ảnh đính kèm
            result.images = Arrays.StringToArrayString(current.TaskImages);

            //Nhân viên xử lý
            if(current.UserId != null)
            {
                result.staff_id = current.UserId;
                result.staff_name = current.User.UserName;
            }

            //Danh sách nhiệm vụ con
            result.childs = GetListJsonTask(current.TaskId);

            return result;
        }

        /// <summary>
        /// Danh sách nhiệm vụ con
        /// </summary>
        /// <returns></returns>
        public static List<TaskModel> GetListJsonTask(long parentId)
        {
            var results = new List<TaskModel>();

            var listItem = GetListTask(parentId);
            foreach (var row in listItem)
            {
                var item = new TaskModel();
                item.id = row.TaskId;
                item.name = row.TaskName;
                item.detail = row.TaskDetail;
                item.deadline = ConvertString.ConvertDate(row.Deadline);
                item.deadline_day = String.Format("{0:yyyy-MM-dd}", row.Deadline);
                item.deadline_time = String.Format("{0:HH:mm}", row.Deadline);
                item.parent = row.ParentId;
                item.status_id = row.StatusId;
                item.status_icon = StaticData.DetailStatusTask(row.StatusId.ToString()).icon;

                //Nhân viên xử lý
                if (row.UserId != null)
                {
                    item.staff_id = row.UserId;
                    item.staff_name = row.User.UserName;
                }

                results.Add(item);
            }

            return results;
        }

        #endregion



    }
}
