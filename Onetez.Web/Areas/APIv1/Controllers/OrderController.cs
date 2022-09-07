using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Caching;
using System.Web.Mvc;
using Onetez.Core.DbContext;
using Onetez.Dal.Models;
using Newtonsoft.Json;
using System.Configuration;
using Onetez.Web.Modules;
using Onetez.Dal.EntityClasses;

namespace Onetez.Web.Areas.APIv1.Controllers
{
    public class OrderController : BaseController
    {
        [Authorize]
        [HttpPost]
        public JsonResult DeleteOrder(long id)
        {
            bool isStatus = false;
            string message = string.Empty;

            var current = DbOrder.Get(id);
            if(current != null)
            {
                message = "Đã xóa MVĐ: " + current.ShipCode;
                isStatus = true;

                current.Status = -1;
                current.ShipLogs = "";
                current.ShopLogs = "";
                current.Complain = "";
                current.Save();
            }
            else
                message = "Đơn đã bị xóa trước đó";

            return Json(new { status = isStatus, msg = message });
        }


        [Authorize]
        [HttpGet]
        public JsonResult GetList(int shop)
        {
            var results = new List<OrderInfoModel>();

            int partner = Request.Cookies["partner"] != null ? Convert.ToInt32(Request.Cookies["partner"].Value) : 0;

            // Lấy 50 đơn vừa xử lý xong
            var orderList = DbOrder.GetList(shop, false, "", "", partner, 1, 50, 0, out int total);

            foreach (var item in orderList)
            {
                results.Add(DbOrder.ConvertToModel(item));
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [HttpPost]
        public JsonResult ChangeStatus(long id, int status)
        {
            bool isRole = UserInfo.role.is_role;
            bool isStatus = false;
            string message = string.Empty;
            var result = new StaticModel();

            if (isRole)
            {
                var order = DbOrder.Get(id);

                if(order != null)
                {
                    order.Status = status;
                    order.UserHandling = UserInfo.id;
                    order.LastUpdate = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                    order.Save();

                    result = DbOrder.Status(status);

                    isStatus = order.Save();

                    if (!isStatus)
                        message = "Không thể cập nhật trạng thái đơn hàng, vui lòng thử lại";
                }
                else
                {
                    message = "Đơn hàng không tồn tại, vui lòng thử lại";
                }
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message, data = result });
        }


        [Authorize]
        [HttpPost]
        public JsonResult ShipLog(long id, string note)
        {
            bool isRole = UserInfo.role.is_role;
            bool isStatus = false;
            string message = string.Empty;
            int result = 0;

            if (isRole)
            {
                var order = DbOrder.Get(id);

                if (order != null)
                {
                    var log = new Extend_Update()
                    {
                        note = note,
                        status = "Cập nhật thủ công",
                        status_code = "000",
                        updated_at = DateTime.Now
                    };


                    var logs = JsonConvert.DeserializeObject<List<Extend_Update>>(order.ShipLogs);
                    if (logs == null)
                        logs = new List<Extend_Update>();

                    logs.Add(log);

                    logs = logs.OrderByDescending(x => x.updated_at).ToList();

                    order.ShipLogs = JsonConvert.SerializeObject(logs);
                    order.ShipUpdate = DateTime.Now;
                    order.LastUpdate = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                    order.Save();

                    isStatus = order.Save();
                    result = logs.Count;

                    if (!isStatus)
                        message = "Không thể cập nhật thông tin xử lý, vui lòng thử lại";
                }
                else
                {
                    message = "Đơn hàng không tồn tại, vui lòng thử lại";
                }
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message, data = result });
        }


        [Authorize]
        [HttpPost]
        public JsonResult ShopLog(long id, string note)
        {
            bool isRole = UserInfo.role.is_role;
            bool isStatus = false;
            string message = string.Empty;
            int result = 0;

            if (isRole)
            {
                var order = DbOrder.Get(id);

                if (order != null)
                {
                    var log = new ShopLogs()
                    {
                        note = note,
                        user = UserInfo.user,
                        date = DateTime.Now
                    };


                    var logs = JsonConvert.DeserializeObject<List<ShopLogs>>(order.ShopLogs);
                    if (logs == null)
                        logs = new List<ShopLogs>();

                    logs.Add(log);

                    logs = logs.OrderByDescending(x => x.date).ToList();

                    order.ShopLogs = JsonConvert.SerializeObject(logs);
                    order.ShopUpdate = DateTime.Now;
                    order.UserHandling = UserInfo.id;
                    order.LastUpdate = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                    order.Save();

                    isStatus = order.Save();
                    result = logs.Count;

                    if (!isStatus)
                        message = "Không thể cập nhật thông tin xử lý, vui lòng thử lại";
                }
                else
                {
                    message = "Đơn hàng không tồn tại, vui lòng thử lại";
                }
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message, data = result });
        }


        [Authorize]
        [HttpPost]
        public JsonResult Complain(long id, string value)
        {
            bool isRole = UserInfo.role.is_role;
            bool isStatus = false;
            string message = string.Empty;
            var result = new StaticModel();

            if (isRole)
            {
                var order = DbOrder.Get(id);

                if (order != null)
                {
                    order.Complain = value.Trim();
                    order.UserHandling = UserInfo.id;
                    order.LastUpdate = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                    order.Save();

                    isStatus = order.Save();

                    if (!isStatus)
                        message = "Không thể cập nhật thông tin khiếu nại, vui lòng thử lại";
                }
                else
                {
                    message = "Đơn hàng không tồn tại, vui lòng thử lại";
                }
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message, data = result });
        }


        [Authorize]
        [HttpPost]
        public JsonResult XoaDonTrungLap()
        {
            bool isRole = UserInfo.role.is_admin;
            string message = string.Empty;

            if (isRole)
            {
                int count = DbOrder.XoaDonTrungLap();

                if (count > 0)
                    message = "Đã xóa " + count + " đơn trùng lặp";
                else if(count < 0)
                    message = "Không có đơn hàng trùng lặp";
                else
                    message = "Không thể xóa đơn hàng trùng lặp";
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { msg = message });
        }


        [Authorize]
        [HttpPost]
        public JsonResult XoaDonThanhCong()
        {
            bool isRole = UserInfo.role.is_admin;
            bool isStatus = false;
            string message = string.Empty;

            if (isRole)
            {
                var sql = "delete Orders where Status = 5 and Complain = ''";
                isStatus = DbConfig.SqlUpdate(sql, out string error);

                if (isStatus)
                    message = "Đã xóa tất cả đơn thành công, bấm F5 để làm mới lại trang";
                else
                    message = "Không thể xóa đơn. ERROR: " + error;
            }
            else
                message = "Bạn không có quyền truy cập chức năng này";

            return Json(new { status = isStatus, msg = message });
        }


        [Authorize]
        [HttpGet]
        public JsonResult CrawlOrder(int id, int page)
        {
            ObjectCache cache = MemoryCache.Default;
            // Trạng thái đơn hàng cần lấy
            int StatusGet = Convert.ToInt32(ConfigurationManager.AppSettings["StatusOrderGet"]);

            bool result = false;
            string message = "";

            // Lấy thông tin shop
            ShopsEntity shop;
            var cache_key_shop = "shopEntity_" + id;
            if (cache[cache_key_shop] == null)
            {
                shop = DbShop.Get(id);
                SetCache(cache_key_shop, shop);
            }
            else
            {
                shop = (ShopsEntity)cache.Get(cache_key_shop);
            }

            if (shop != null)
            {
                if(page <= shop.OrderPage)
                {
                    // Lấy danh sách đơn hàng
                    var crawlOrders = PancakeApi.OrderList(DbShop.ConvertToModel(shop), page, out message);

                    if(crawlOrders != null) // != null : Quét không lỗi
                    {
                        result = true; // Quét trang này thành công
                        int create = 0;
                        int update = 0;

                        foreach (var item in crawlOrders)
                        {
                            // Chỉ lấy những đơn có mã vận chuyển
                            if(item.partner != null && !string.IsNullOrEmpty(item.partner.extend_code))
                            {
                                string lastUpdate = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);

                                string shipCode = item.partner.extend_code; // Đơn J&T Express
                                if (item.partner.partner_id == 3 && !string.IsNullOrEmpty(item.partner.order_number_vtp))
                                    shipCode = item.partner.order_number_vtp; // Đơn Viettel Post

                                var checkOrder = DbOrder.GetByShipCode(shipCode);

                                if (checkOrder == null) // Chưa có đơn này trong Database
                                {
                                    // Bỏ qua những đơn đang chưa xử lý vận chuyển hoặc đã hoàn tất
                                    if (item.status != StatusGet)
                                        continue;

                                    // Chỉ lấy những đơn đang xử lý vận chuyển
                                    var order = new OrdersEntity()
                                    {
                                        ShopId = shop.Id,
                                        OrderId = item.id,
                                        BillName = item.bill_full_name,
                                        BillPhone = item.bill_phone_number,
                                        ShipCode = shipCode,
                                        PartnerId = item.partner.partner_id,
                                        Product = "",
                                        ShipPhone = "",
                                        ShipStatus = "",
                                        ShopLogs = "",
                                        ShopUpdate = DateTime.Now.AddDays(-1),
                                        UserHandling = "",
                                        Complain = "",
                                        Status = 3,
                                        LastUpdate = lastUpdate,
                                        ShipUpdate = DateTime.Now,
                                        ShipInStock = DateTime.Now.AddYears(1)
                                    };

                                    // Lấy thông tin sản phẩm
                                    if (item.items != null && item.items.Count > 0)
                                    {
                                        string products = "";
                                        foreach (var pro in item.items)
                                            products += pro.variation_info.name + "; ";

                                        order.Product = products;
                                    }

                                    // Lấy thông tin vận chuyển
                                    if (item.partner.extend_update != null && item.partner.extend_update.Count > 0)
                                    {
                                        // chuẩn hóa dữ liệu ngày
                                        var shipLogs = item.partner.extend_update;
                                        foreach (var log in shipLogs)
                                        {
                                            var date = item.partner.updated_at;
                                            if (log.updated_at != null)
                                                date = log.updated_at.Value;
                                            else if (log.update_at != null)
                                                date = log.update_at.Value;
                                            log.updated_at = date;
                                            log.update_at = date;
                                        }

                                        // Đọc dữ liệu vận chuyển để lấy trạng thái và thời gian lưu kho
                                        ReadShippingStatus(shipLogs, out int status, out string shipStatus,
                                            out DateTime shipUpdate, out DateTime shipInStock,
                                            out Extend_Update logInStock, out Extend_Update logInShipping);

                                        // Số ngày lưu kho
                                        if (logInShipping != null) // Tính từ lúc phát kiện lần đầu tiên
                                            shipInStock = Convert.ToDateTime(logInShipping.updated_at);
                                        //else if (logInStock != null && !logInStock.note.Contains("TTKT") && shipLogs.Count > 1)
                                        else if (logInStock != null && shipLogs.Count > 1)
                                            shipInStock = Convert.ToDateTime(logInStock.updated_at); // Hoặc tính từ lúc kiện đến

                                        // sắp sếp lại lịch sự vận đơn
                                        shipLogs = shipLogs.OrderByDescending(x => x.updated_at).ToList();

                                        order.ShipLogs = JsonConvert.SerializeObject(shipLogs);
                                        order.ShipStatus = shipStatus;
                                        order.ShipUpdate = shipUpdate != null ? shipUpdate : DateTime.Now;
                                        order.ShipInStock = shipInStock;

                                        if (status != 0) order.Status = status;
                                    }

                                    order.Save();

                                    create++; // Đếm số lượng đơn tạo mới
                                }
                                else // Đã có trong Database
                                {
                                    // Cập nhật Lịch sử vận chuyển
                                    if (item.partner.extend_update != null && item.partner.extend_update.Count > 0)
                                    {
                                        // Lịch sử vận chuyển cũ
                                        var shipLogsOld = JsonConvert.DeserializeObject<List<Extend_Update>>(checkOrder.ShipLogs);

                                        // Lịch sử vận chuyển thêm thủ công
                                        var shipLogsManual = new List<Extend_Update>();
                                        if (shipLogsOld != null)
                                            shipLogsManual = shipLogsOld.Where(x => x.status_code == "000").ToList();

                                        // chuẩn hóa dữ liệu ngày
                                        var shipLogs = item.partner.extend_update;
                                        foreach (var log in shipLogs)
                                        {
                                            var date = item.partner.updated_at;
                                            if (log.updated_at != null)
                                                date = log.updated_at.Value;
                                            else if (log.update_at != null)
                                                date = log.update_at.Value;
                                            log.updated_at = date;
                                            log.update_at = date;
                                        }

                                        // Đọc dữ liệu vận chuyển để lấy trạng thái và thời gian lưu kho
                                        ReadShippingStatus(shipLogs, out int status, out string shipStatus,
                                            out DateTime shipUpdate, out DateTime shipInStock,
                                            out Extend_Update logInStock, out Extend_Update logInShipping);

                                        // Số ngày lưu kho
                                        if (logInShipping != null) // Tính từ lúc phát kiện lần đầu tiên
                                            shipInStock = Convert.ToDateTime(logInShipping.updated_at);
                                        //else if (logInStock != null && !logInStock.note.Contains("TTKT") && shipLogs.Count > 1)
                                        else if (logInStock != null && shipLogs.Count > 1)
                                            shipInStock = Convert.ToDateTime(logInStock.updated_at); // Hoặc tính từ lúc kiện đến

                                        // Thêm lịch sử vận chuyển thêm thủ công
                                        if (shipLogsManual.Count > 0)
                                            shipLogs.AddRange(shipLogsManual);

                                        // sắp sếp lại lịch sự vận đơn
                                        shipLogs = shipLogs.OrderByDescending(x => x.updated_at).ToList();

                                        checkOrder.ShipLogs = JsonConvert.SerializeObject(shipLogs);
                                        if (shipUpdate != null && checkOrder.ShipUpdate < shipUpdate)
                                            checkOrder.ShipUpdate = shipUpdate;
                                        checkOrder.ShipInStock = shipInStock;
                                        checkOrder.ShipStatus = shipStatus;

                                        if (status != 0)
                                            checkOrder.Status = status;

                                        checkOrder.LastUpdate = lastUpdate;
                                        checkOrder.Save();

                                        update++; // Đếm số lượng đơn cập nhật
                                    }
                                }
                            }
                        }

                        message = $"CREATE: {create} | UPDATE: {update}";
                    }
                    else
                    {
                        message = "Không thể lấy dữ liệu, tiếp tục thử lại ! <br/>ERROR: " + message;
                    }
                }
                else
                {
                    result = true; // Đã quét đủ số trang
                    message = "OK";
                }
            }

            return Json(new
            {
                status = result,
                message = message,
            }, JsonRequestBehavior.AllowGet);
        }


        [Authorize]
        [HttpGet]
        public JsonResult ReadShipLog()
        {
            var results = new List<OrderInfoModel>();

            var orderList = DbOrder.GetList(0, false, "", "", 0, 1, 0, 0, out int total);

            foreach (var item in orderList)
            {
                int oldStatus = item.Status;

                var shipLogs = JsonConvert.DeserializeObject<List<Extend_Update>>(item.ShipLogs);

                // Đọc dữ liệu vận chuyển để lấy trạng thái và thời gian lưu kho
                ReadShippingStatus(shipLogs, out int status, out string shipStatus,
                    out DateTime shipUpdate, out DateTime shipInStock,
                    out Extend_Update logInStock, out Extend_Update logInShipping);

                // Số ngày lưu kho
                if (logInShipping != null) // Tính từ lúc phát kiện lần đầu tiên
                    shipInStock = Convert.ToDateTime(logInShipping.updated_at);
                else if (logInStock != null && shipLogs.Count > 1)
                    shipInStock = Convert.ToDateTime(logInStock.updated_at); // Hoặc tính từ lúc kiện đến

                if (shipUpdate != null && item.ShipUpdate < shipUpdate)
                    item.ShipUpdate = shipUpdate;
                item.ShipInStock = shipInStock;
                item.ShipStatus = shipStatus;

                if(oldStatus != status && status != 0)
                {
                    item.Status = status;
                    item.LastUpdate = string.Format("{0:yyyyMMddHHmmss}", DateTime.Now);
                    item.Save();

                    results.Add(DbOrder.ConvertToModel(item));
                }
            }

            return Json(results, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Đọc dữ liệu vận chuyển để lấy trạng thái và thời gian lưu kho
        /// </summary>
        public void ReadShippingStatus(List<Extend_Update> shipLogs, out int status, out string shipStatus,
            out DateTime shipUpdate, out DateTime shipInStock, out Extend_Update logInStock, out Extend_Update logInShipping)
        {
            status = 0;
            shipStatus = "";
            shipUpdate = DateTime.Now.AddYears(1);
            shipInStock = DateTime.Now.AddYears(1);
            logInStock = null;
            logInShipping = null;

            if(shipLogs != null)
            {
                shipLogs = shipLogs.OrderBy(x => x.updated_at).ToList();
                foreach (var log in shipLogs)
                {
                    // chuẩn hóa dữ liệu ngày
                    var date = log.updated_at.Value;

                    // chuẩn hóa ghi chú vận chuyển
                    if (!string.IsNullOrEmpty(log.location))
                        log.note = log.location;
                    if (string.IsNullOrEmpty(log.note))
                        log.note = log.status;

                    //Thời gian cập nhật vận chuyển
                    if (date > shipUpdate)
                        shipUpdate = date;

                    // Ngày lưu kho: 
                    // Tính từ lúc phát kiện lần đầu tiên
                    if (log.status_code == "112" && logInShipping == null)
                        logInShipping = log;
                    else if (!string.IsNullOrEmpty(log.status) && log.status.ToLower().Contains("giao kiện") && logInShipping == null)
                        logInShipping = log;

                    // Ngày lưu kho: 
                    // Hoặc tính từ lúc kiện đến lần cuối cùng
                    if (log.status_code == "110")
                        logInStock = log;
                    else if (!string.IsNullOrEmpty(log.status) && log.status.ToLower().Contains("kiện đến")
                        && !string.IsNullOrEmpty(log.note) && !log.note.Contains("TTKT"))
                        logInStock = log;

                    // Trạng thái kiện vấn đề
                    if (!string.IsNullOrEmpty(log.note) && log.note.ToLower().Contains("kiện vấn đề"))
                        shipStatus = "Kiện vấn đề và kiện lưu kho";

                    // Trạng thái chuyển hoàn
                    if (log.status_code == "117" || log.status.ToLower().Contains("chuyển hoàn thành công") || log.status.ToLower().Contains("ký nhận chuyển hoàn") // J&T + Viettel
                        || log.status.ToLower().Contains("đã trả hàng") || log.status.ToLower().Contains("đã đối soát công nợ trả hàng") )
                    {
                        shipStatus = "Chuyển hoàn";
                        status = 4;
                    }
                    // Trạng thái thành công
                    else if (log.status_code == "113" || log.status.ToLower().Contains("giao kiện thành công") || log.status.ToLower().Contains("ký nhận") // J&T + Viettel
                        || log.status.ToLower().Contains("đã đối soát") || log.status.ToLower().Contains("đã giao hàng/chưa đối soát")) // GHTK
                    {
                        shipStatus = "Thành công";
                        status = 5;
                    }
                }
            }
        }
    }
}
