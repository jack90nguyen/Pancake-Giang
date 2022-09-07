using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Onetez.Dal.Models;

namespace Onetez.Core.Data_v1
{
    public class StaticData
    {
        #region Loại công trình

        /// <summary>
        /// Loại công trình
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> StaticListConstruction()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Nhà ở gia đình 1 mặt tiền" });
            list.Add(new SelectModel { id = "2", name = "Nhà ở gia đình 2 mặt tiền" });
            list.Add(new SelectModel { id = "3", name = "Biệt thự sân vườn" });
            list.Add(new SelectModel { id = "4", name = "Biệt thự phố" });
            list.Add(new SelectModel { id = "5", name = "Căn hộ - Chung cư" });
            list.Add(new SelectModel { id = "6", name = "Nhà cải tạo" });
            list.Add(new SelectModel { id = "7", name = "Nhà hàng" });
            list.Add(new SelectModel { id = "8", name = "Quán Cafe" });
            list.Add(new SelectModel { id = "9", name = "Cửa hàng" });
            list.Add(new SelectModel { id = "10", name = "Văn phòng" });
            list.Add(new SelectModel { id = "11", name = "Khác" });
            return list;
        }

        /// <summary>
        /// Chi tiết loại công trình
        /// </summary>
        /// <returns></returns>
        public static SelectModel GetDetailConstruction(string id)
        {
            var list = StaticListConstruction();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        #endregion

        #region Trạng thái hợp đồng

        /// <summary>
        /// Trạng thái hợp đồng
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> ListStatusContract()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Nháp", color = "White" });
            list.Add(new SelectModel { id = "2", name = "Chưa ký", color = "Cyan" });
            list.Add(new SelectModel { id = "3", name = "Đã ký", color = "Green" });
            list.Add(new SelectModel { id = "4", name = "Đã xong", color = "Blue" });
            list.Add(new SelectModel { id = "5", name = "Đã Hủy", color = "Black" });
            return list;
        }

        /// <summary>
        /// Chi tiet Trạng thái hợp đồng
        /// </summary>
        /// <returns></returns>
        public static SelectModel DetailStatusContract(string id)
        {
            var list = ListStatusContract();

            foreach (var item in list)
            {
                if (id == item.id)
                    return item;
            }

            return null;
        }

        #endregion

        #region Nguồn Thu Chi

        /// <summary>
        /// Nguồn Thu Chi
        /// </summary>
        /// <returns></returns>
        public static List<SelectModel> ListTypeIncome()
        {
            var list = new List<SelectModel>();
            list.Add(new SelectModel { id = "1", name = "Thu Chi Khác", color = "" });
            list.Add(new SelectModel { id = "2", name = "Từ Hợp Đồng", color = "" });
            list.Add(new SelectModel { id = "3", name = "Tiền Lương", color = "" });
            list.Add(new SelectModel { id = "4", name = "Tiền Thưởng", color = "" });
            list.Add(new SelectModel { id = "5", name = "Chi Phí Cố Định Khác", color = "" });
            list.Add(new SelectModel { id = "6", name = "Chi Phí Phát Sinh Khác", color = "" });
            return list;
        }

        /// <summary>
        /// Chi tiết Nguồn Thu Chi
        /// </summary>
        /// <returns></returns>
        public static SelectModel DetailTypeIncome(string id)
        {
            var list = ListTypeIncome();

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
