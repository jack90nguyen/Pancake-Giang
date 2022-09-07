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
    public class ContractData
    {
        public static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();
        public static int TimeUtc = Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]);

        #region Contract List & Detail

        /// <summary>
        /// Tạo hợp đồng mới
        /// </summary>
        /// <returns></returns>
        public static ContractsEntity CreateContract(BranchsEntity branch, CustomersEntity customer, SessionsEntity session, long userId)
        {
            var current = new ContractsEntity();
            //Thông tin hợp đồng
            current.DateCreate = DateTime.Now.AddHours(TimeUtc);
            current.DateSign = DateTime.Now.AddHours(TimeUtc);
            current.StatusId = 1;
            current.UserId = userId;
            //Thông tin khách hàng
            current.CustomerId = customer.CustomerId;
            current.CusName = customer.CusName;
            current.Gender = customer.Gender;
            current.CusBirthday = customer.Birthday;
            current.CmndCode = customer.CmndCode;
            current.CusPhone = customer.CusPhone;
            current.CusEmail = customer.CusEmail;
            current.CusAddress = customer.CmndAddress;
            //Thông tin công ty
            current.BranchId = branch.BranchId;
            current.CompanyName = branch.CompanyName;
            current.CompanyAddress = branch.PropAddress;
            current.CompanyPhone = branch.PropPhone;
            current.CompanyBank = branch.PropBank;
            //Thông tin công trình
            current.SessionId = session.SessionId;
            current.ConstructionType = StaticData.GetDetailConstruction(session.PropType.ToString()).name;
            current.ConstructionFloors = session.PropFloors;
            current.ConstructionAcreage = session.PropAcreage;
            current.ConstructionAddress = session.PropAddress;

            current.Save();

            //Gán hợp đồng hiện tại cho khách hàng
            customer.ContractId = current.ContractId;
            customer.Save();

            //Gán hợp đồng cho phiên làm việc
            session.ContractId = current.ContractId;
            session.Save();

            return current;
        }

        /// <summary>
        /// Lấy chi tiết hợp đồng
        /// </summary>
        /// <returns></returns>
        public static ContractsEntity GetOneContract(long contractId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Contracts
                         where c.ContractId == contractId && !c.IsDelete
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Lấy chi tiết hợp đồng
        /// </summary>
        /// <returns></returns>
        public static ContractsEntity GetOneContract(string code)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Contracts
                         where c.ContractCode == code && !c.IsDelete
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Convert ContractsEntity to Json Model
        /// </summary>
        /// <returns></returns>
        public static ContractModel ConvertContractToJson(ContractsEntity current)
        {
            var result = new ContractModel();

            //Thông tin hợp đồng
            result.id = current.ContractId;
            result.code = current.ContractCode;
            result.status = current.StatusId;
            result.datesign = String.Format("{0:yyyy-MM-dd}", current.DateSign);
            result.unitprice = current.DesignUnitPrice > 0 ? String.Format("{0:0,0}", current.DesignUnitPrice) : "";
            result.totalmoney = current.TotalDesignMoney > 0 ? String.Format("{0:0,0}", current.TotalDesignMoney) : "";
            result.textmoney = current.TotalTextMoney;
            //Thông tin khách hàng
            result.customer_name = current.CusName;
            result.customer_gender = current.Gender;
            result.customer_birthday = String.Format("{0:yyyy-MM-dd}", current.CusBirthday);
            result.customer_cmnd_code = current.CmndCode;
            result.customer_phone = current.CusPhone;
            result.customer_email = current.CusEmail;
            result.customer_address = current.CusAddress;
            //Thông tin công ty
            result.compary_name = current.CompanyName;
            result.compary_address = current.CompanyAddress;
            result.compary_phone = current.CompanyPhone;
            result.compary_bank = current.CompanyBank;
            result.signer_name = current.SignerName;
            result.signer_position = current.SignerPosition;
            //Thông tin công trình
            result.construction_type = current.ConstructionType;
            result.construction_floors = current.ConstructionFloors;
            result.construction_acreage = current.ConstructionAcreage;
            result.construction_address = current.ConstructionAddress;
            //Đợt thanh toán
            result.deposits = GetJsonDeposit(current.ContractId);

            //Nhân viên xử lý cuối cùng
            result.staff_name = current.User.UserName;

            //Trạng thái hợp đồng
            var listStep = new List<SelectModel>();
            var listStatus = StaticData.ListStatusContract();
            if(current.StatusId != 5)
            {
                foreach (var item in listStatus)
                {
                    int statusId = Convert.ToInt32(item.id);

                    if (statusId == 5)
                        break;

                    var step = new SelectModel();
                    step.id = item.id;
                    step.name = item.name;
                    step.color = statusId <= current.StatusId ? "active" : "";

                    listStep.Add(step);
                }
            }
            else
            {
                foreach (var item in listStatus)
                {
                    int statusId = Convert.ToInt32(item.id);

                    if(statusId == 1)
                    {
                        var step = new SelectModel();
                        step.id = item.id;
                        step.name = item.name;
                        step.color = "active";

                        listStep.Add(step);
                    }
                    else if (statusId == 5)
                    {
                        var step = new SelectModel();
                        step.id = "X";
                        step.name = item.name;
                        step.color = "active--red";

                        listStep.Add(step);
                    }
                }
            }
            result.status_step = listStep;

            return result;
        }

        /// <summary>
        /// Tim Kiem Hợp Đồng
        /// size = 0: lấy tất cả
        /// </summary>
        /// <returns></returns>
        public static DataTable GetListContract(int paging, int size, bool isDelete, long branchId, long createUserId,
            string dataStart, string dateEnd, string keyword, int statusId, out int total)
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
                        searchKey += "  and ";

                    searchKey += "(";
                    searchKey += " c.ContractCode Like N'%" + array[i] + "%' or c.ConstructionType Like N'%" + array[i] + "%'";
                    searchKey += " or c.CusName Like N'%" + array[i] + "%' or c.CusEmail Like N'%" + array[i] + "%'";
                    searchKey += " or c.CusPhone Like N'%" + array[i] + "%'or c.CmndCode Like N'%" + array[i] + "%'";
                    searchKey += ")";
                }
            }

            string filter = " where c.IsDelete = " + (isDelete ? "1" : "0");
            //Tìm theo chi nhanh
            if (branchId != 0)
                filter += " and c.BranchId = " + branchId;
            //Tìm theo người tạo
            if (createUserId != 0)
                filter += " and c.UserId = " + createUserId;
            //Tìm theo trang thai
            if (statusId != 0)
                filter += " and c.StatusId = " + statusId;
            //Tìm từ ngày
            if (!string.IsNullOrEmpty(dataStart))
                filter += " and Convert(Date,'" + dataStart + "')<=Convert(Date,c.DateSign)";
            //Tìm đến ngày
            if (!string.IsNullOrEmpty(dateEnd))
                filter += " and Convert(Date,'" + dateEnd + "')>=Convert(Date,c.DateSign)";
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(searchKey))
                filter += " and (" + searchKey + ")";

            #endregion

            string sql = string.Empty;


            //Lấy dữ liệu chính
            sql += " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY c.DateSign desc, c.DateCreate desc) as row,";
            sql += " c.ContractId,c.ContractCode,c.DateSign,c.StatusId,c.UserId,";
            sql += " c.ConstructionType,c.ConstructionAcreage,c.ConstructionFloors,c.DesignUnitPrice,c.TotalDesignMoney,";
            sql += " c.CustomerId,c.CusName,c.CusEmail,c.CusPhone,c.Gender";
            sql += " FROM Contracts c";
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
                sqlTotal += " SELECT COUNT(ContractId) as Total FROM Contracts c";
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
        /// Thông kê số lượng hợp đồng
        /// </summary>
        /// <returns></returns>
        public static ContractReportModel GetReportContract(bool isDelete, long branchId, long createUserId, string dataStart, string dateEnd, string keyword)
        {
            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();

            #region SQL loc ket qua

            string searchKey = string.Empty;
            if (!string.IsNullOrEmpty(keyword)) //Tìm theo từ khóa
            {
                string[] array = keyword.Split(' ');
                for (int i = 0; i < array.Length; i++)
                {
                    if (i > 0)
                        searchKey += "  and ";

                    searchKey += "(";
                    searchKey += " c.ContractCode Like N'%" + array[i] + "%' or c.ConstructionType Like N'%" + array[i] + "%'";
                    searchKey += " or c.CusName Like N'%" + array[i] + "%' or c.CusEmail Like N'%" + array[i] + "%'";
                    searchKey += " or c.CusPhone Like N'%" + array[i] + "%'or c.CmndCode Like N'%" + array[i] + "%'";
                    searchKey += ")";
                }
            }

            string filter = " where c.IsDelete = " + (isDelete ? "1" : "0");
            //Tìm theo chi nhanh
            if (branchId != 0)
                filter += " and c.BranchId = " + branchId;
            //Tìm theo người tạo
            if (createUserId != 0)
                filter += " and c.UserId = " + createUserId;
            //Tìm từ ngày
            if (!string.IsNullOrEmpty(dataStart))
                filter += " and Convert(Date,'" + dataStart + "')<=Convert(Date,c.DateSign)";
            //Tìm đến ngày
            if (!string.IsNullOrEmpty(dateEnd))
                filter += " and Convert(Date,'" + dateEnd + "')>=Convert(Date,c.DateSign)";
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(searchKey))
                filter += " and (" + searchKey + ")";

            #endregion


            string sql = "with listAll(ContractId,StatusId)";
            sql += " as (SELECT ContractId,StatusId FROM Contracts c";
            sql += filter;
            sql += " )";

            sql += " SELECT COUNT(ContractId) as TotalCount,";
            sql += " (select COUNT(ContractId) from listAll where StatusId = 1) as CountDraft,";
            sql += " (select COUNT(ContractId) from listAll where StatusId = 2) as CountNew,";
            sql += " (select COUNT(ContractId) from listAll where StatusId = 3) as CountSigned,";
            sql += " (select COUNT(ContractId) from listAll where StatusId = 4) as CountDone,";
            sql += " (select COUNT(ContractId) from listAll where StatusId = 5) as CountCancel";
            sql += " FROM listAll";

            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            var report = new ContractReportModel();
            if (myTable.Rows.Count > 0)
            {
                var row = myTable.Rows[0];
                report.TotalCount = row["TotalCount"] != DBNull.Value ? Convert.ToInt32(row["TotalCount"]) : 0;
                report.CountDraft = row["CountDraft"] != DBNull.Value ? Convert.ToInt32(row["CountDraft"]) : 0;
                report.CountNew = row["CountNew"] != DBNull.Value ? Convert.ToInt32(row["CountNew"]) : 0;
                report.CountSigned = row["CountSigned"] != DBNull.Value ? Convert.ToInt32(row["CountSigned"]) : 0;
                report.CountDone = row["CountDone"] != DBNull.Value ? Convert.ToInt32(row["CountDone"]) : 0;
                report.CountCancel = row["CountCancel"] != DBNull.Value ? Convert.ToInt32(row["CountCancel"]) : 0;
            }

            return report;
        }

        #endregion

        #region Mã hợp đồng

        /// <summary>
        /// Tạo mã hợp đồng mới theo ngày
        /// Định dạng: yyyyMMddSTT
        /// </summary>
        /// <returns></returns>
        public static string CreateContractCode(DateTime datesign, out string code_stt)
        {
            string code_end = ConfigurationManager.AppSettings["ContractCode"];
            string date = String.Format("{0:yyyy-MM-dd}", datesign);

            SqlConnection _con = new SqlConnection(SqlConnect);
            _con.Open();

            string sql = "SELECT top 1 c.ContractStt FROM Contracts c";
            sql += " where c.IsDelete = 0";
            sql += " and c.ContractStt != ''";
            sql += " and Convert(Date,'"+ date + "') = Convert(Date,c.DateSign)";
            sql += " order by c.ContractStt desc";

            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            if (myTable.Rows.Count > 0)
            {
                int stt = Convert.ToInt32(myTable.Rows[0]["ContractStt"]) + 1;

                code_stt = stt < 10 ? "0" + stt : stt.ToString();
            }
            else
            {
                code_stt = "01";
            }

            string code_date = String.Format("{0:yyyyMMdd}", datesign);

            return code_date + code_stt + code_end;
        }

        #endregion

        #region Các đợt thanh toán

        /// <summary>
        /// Chi tiết đơt thanh toán của hợp đồng
        /// </summary>
        /// <returns></returns>
        public static DepositEntity GetOneDeposit(long depositId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Deposit
                         where c.DepositId == depositId
                         orderby c.DepositDate
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Danh sách đơt thanh toán của hợp đồng
        /// </summary>
        /// <returns></returns>
        public static List<DepositEntity> GetListDeposit(long contractId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Deposit
                         where c.ContractId == contractId
                         orderby c.DepositDate
                         select c).ToList();
            return query;
        }

        /// <summary>
        /// Danh sách đơt thanh toán của hợp đồng
        /// </summary>
        /// <returns></returns>
        public static List<DepositModel> GetJsonDeposit(long contractId)
        {
            var results = new List<DepositModel>();

            int index = 1;
            var listItem = GetListDeposit(contractId);
            foreach (var row in listItem)
            {
                var item = new DepositModel();
                item.id = row.DepositId;
                item.name = "ĐỢT " + index;
                item.money = String.Format("{0:0,0}", row.DepositMoney);
                item.date = String.Format("{0:yyyy-MM-dd}", row.DepositDate);
                item.note = row.DepositNote;
                item.is_paid = row.IsPaid;

                results.Add(item);

                index++;

                //Cập nhật tên cho đợt
                row.DepositName = item.name;
                row.Save();
            }

            return results;
        }

        #endregion
    }
}
