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
    public class IncomeData
    {
        private static string SqlConnect = WebConfigurationManager.ConnectionStrings["Main.ConnectionString"].ToString();
        private static int TimeUtc = Convert.ToInt32(ConfigurationManager.AppSettings["TimeUtc"]);

        #region Income List & Detail

        /// <summary>
        /// Lấy chi tiết thu chi
        /// </summary>
        /// <returns></returns>
        public static IncomesEntity GetOneIncome(long incomeId)
        {
            var db = new LinqMetaData();
            var query = (from c in db.Incomes
                         where !c.IsDelete && c.IncomeId == incomeId
                         select c).ToList();
            if (query.Count > 0)
                return query.FirstOrDefault();
            else
                return null;
        }

        /// <summary>
        /// Lấy chi tiết thu chi
        /// Theo Type, KeyId, Key2Id
        /// Type: 2 - Từ Hợp Đồng
        /// </summary>
        /// <returns></returns>
        public static List<IncomesEntity> GetIncomes(int type, long keyId, long? key2Id)
        {
            var db = new LinqMetaData();

            var list = new List<IncomesEntity>();

            if(key2Id == null)
            {
                list = (from c in db.Incomes
                        where !c.IsDelete && c.KeyId == keyId
                        select c).ToList();
            }
            else
            {
                list = (from c in db.Incomes
                        where !c.IsDelete && c.KeyId == keyId && c.Key2Id == key2Id
                        select c).ToList();
            }

            return list;
        }

        /// <summary>
        /// Xóa thu chi liên quan
        /// Theo Type, KeyId, Key2Id
        /// Type: 2 - Từ Hợp Đồng
        /// </summary>
        /// <returns></returns>
        public static bool DeleteIncomes(int type, long keyId, long? key2Id, long userId)
        {
            try
            {
                var list = GetIncomes(type, keyId, key2Id);

                foreach (var item in list)
                {
                    item.IsDelete = true;
                    item.UserId = userId;
                    item.Save();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }

            
        }

        /// <summary>
        /// Tìm kiếm Thu Chi
        /// size = 0: lấy tất cả
        /// </summary>
        /// <returns></returns>
        public static DataTable GetListIncome(int paging, int size, bool isDelete, long branchId, int isreceive, int ispaid, 
            int type, int payType, long staffId, string keyword, string dataStart, string dateEnd, out int total)
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
                    searchKey += " c.IncomeName Like N'%" + array[i] + "%'";
                    searchKey += ")";
                }
            }

            string filter = " where c.IsDelete = " + (isDelete ? "1" : "0");
            //Tìm theo chi nhanh
            if (branchId != 0)
                filter += " and c.BranchId = " + branchId;
            //Tìm theo loại phiếu
            if (isreceive !=  0)
                filter += " and c.IncomeMoney " + (isreceive == 1 ? " >= 0" : " < 0");
            //Tìm theo tình trạng thanh toán
            if (ispaid != 0)
                filter += " and c.IsPaid = " + (ispaid == 1 ? "1" : "0");
            //Tìm theo nguồn thu chi
            if (type != 0)
                filter += " and c.IncomeType = " + type;
            //Tìm theo hình thức thanh toán
            if (payType != 0)
                filter += " and c.PayType = " + payType;
            //Tìm theo người tạo
            if (staffId != 0)
                filter += " and c.UserId = " + staffId;
            //Tìm từ ngày
            if (!string.IsNullOrEmpty(dataStart))
                filter += " and Convert(Date,'" + dataStart + "')<=Convert(Date,c.DatePay)";
            //Tìm đến ngày
            if (!string.IsNullOrEmpty(dateEnd))
                filter += " and Convert(Date,'" + dateEnd + "')>=Convert(Date,c.DatePay)";
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(searchKey))
                filter += " and (" + searchKey + ")";

            #endregion

            string sql = string.Empty;
            //Lấy dữ liệu chính
            sql += " SELECT * FROM (";
            sql += " SELECT ROW_NUMBER() OVER(ORDER BY c.DateCreate desc) as row,";
            sql += " c.IncomeId,c.IncomeName,c.IncomeMoney,c.IncomeType,c.PayType,";
            sql += " c.IsPaid,c.KeyId,c.UserId,c.DateCreate,c.DatePay,c.IncomeNote";
            sql += " FROM Incomes c";
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
                sqlTotal += " SELECT COUNT(IncomeId) as Total FROM Incomes c";
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
        /// Thông kê thu chi
        /// </summary>
        /// <returns></returns>
        public static IncomeReportModel GetReportIncome(bool isDelete, long branchId, int type, int payType, long staffId, string keyword, string dataStart, string dateEnd)
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
                        searchKey += " and ";

                    searchKey += "(";
                    searchKey += " c.IncomeName Like N'%" + array[i] + "%'";
                    searchKey += ")";
                }
            }

            string filter = " where c.IsDelete = " + (isDelete ? "1" : "0");
            //Tìm theo chi nhanh
            if (branchId != 0)
                filter += " and c.BranchId = " + branchId;
            //Tìm theo nguồn thu chi
            if (type != 0)
                filter += " and c.IncomeType = " + type;
            //Tìm theo hình thức thanh toán
            if (payType != 0)
                filter += " and c.PayType = " + payType;
            //Tìm theo người tạo
            if (staffId != 0)
                filter += " and c.UserId = " + staffId;
            //Tìm từ ngày
            if (!string.IsNullOrEmpty(dataStart))
                filter += " and Convert(Date,'" + dataStart + "')<=Convert(Date,c.DatePay)";
            //Tìm đến ngày
            if (!string.IsNullOrEmpty(dateEnd))
                filter += " and Convert(Date,'" + dateEnd + "')>=Convert(Date,c.DatePay)";
            //Tìm theo từ khóa
            if (!string.IsNullOrEmpty(searchKey))
                filter += " and (" + searchKey + ")";

            #endregion


            string sql = "with listAll(IncomeMoney,IsPaid)";
            sql += " as (SELECT IncomeMoney,IsPaid FROM Incomes c";
            sql += filter;
            sql += " )";

            sql += " SELECT SUM(IncomeMoney) as TotalProfit,";
            sql += " (select SUM(IncomeMoney) from listAll a where a.IncomeMoney >= 0) as TotalReceive,";
            sql += " (select SUM(IncomeMoney) from listAll a where a.IncomeMoney >= 0 and a.IsPaid = 0) as ReceiveDebt,";
            sql += " (select SUM(IncomeMoney) from listAll a where a.IncomeMoney < 0) as TotalPay,";
            sql += " (select SUM(IncomeMoney) from listAll a where a.IncomeMoney < 0 and a.IsPaid = 0) as PayDebt";
            sql += " FROM listAll";

            var da = new SqlDataAdapter(sql, _con);
            var myTable = new DataTable();
            da.Fill(myTable);
            _con.Close();

            var report = new IncomeReportModel();
            if (myTable.Rows.Count > 0)
            {
                var row = myTable.Rows[0];
                report.TotalProfit = row["TotalProfit"] != DBNull.Value ? Convert.ToDouble(row["TotalProfit"]) : 0;
                report.TotalReceive = row["TotalReceive"] != DBNull.Value ? Convert.ToDouble(row["TotalReceive"]) : 0;
                report.ReceiveDebt = row["ReceiveDebt"] != DBNull.Value ? Convert.ToDouble(row["ReceiveDebt"]) : 0;
                report.TotalPay = row["TotalPay"] != DBNull.Value ? -Convert.ToDouble(row["TotalPay"]) : 0;
                report.PayDebt = row["PayDebt"] != DBNull.Value ? -Convert.ToDouble(row["PayDebt"]) : 0;
            }

            return report;
        }

        /// <summary>
        /// Convert IncomesEntity to Json Model
        /// </summary>
        /// <returns></returns>
        public static IncomeModel ConvertIncomeToJson(IncomesEntity current)
        {
            var result = new IncomeModel();

            bool isreceive = current.IncomeMoney >= 0 ? true : false;
            double money = isreceive ? current.IncomeMoney : -current.IncomeMoney;

            result.id = current.IncomeId;
            result.type = current.IncomeType;
            result.name = current.IncomeName;
            result.money = String.Format("{0:0,0}", money);
            result.note = current.IncomeNote;
            result.date_pay = String.Format("{0:yyyy-MM-dd}", current.DatePay);
            result.pay_type = current.PayType;
            result.is_paid = current.IsPaid;
            result.is_receive = isreceive;
            result.key_id = current.KeyId;
            result.key2_id = current.Key2Id;

            //Lấy mã hợp đồng
            if (current.KeyId != null && current.IncomeType == 2)
            {
                var contract = ContractData.GetOneContract(current.KeyId.Value);
                if(contract != null)
                {
                    result.key_name = contract.ContractCode;
                }
            }

            return result;
        }

        #endregion
    }
}
