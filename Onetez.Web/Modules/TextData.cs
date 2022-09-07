using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Onetez.Dal.Models;

namespace Onetez.Web.Modules
{
  public class TextData
  {
    // @using Onetez.Web.Modules;
    // @TextData.Get("")
    public static List<TextModel> All()
    {
      var list = new List<TextModel>();

      list.Add(new TextModel { vi = "Phần mềm hỗ trợ xử lý đơn hàng", en = "Order processing software" });
      list.Add(new TextModel { vi = "Đơn chưa xử lý", en = "Order no process" });
      list.Add(new TextModel { vi = "Đơn đang xử lý", en = "Order processing" });
      list.Add(new TextModel { vi = "Đơn đã xử lý", en = "Order processed" });
      list.Add(new TextModel { vi = "Đơn đã tạo", en = "Order created" });
      list.Add(new TextModel { vi = "Đơn tạo lỗi", en = "Order error" });
      list.Add(new TextModel { vi = "Đơn hủy", en = "Order cancel" });
      list.Add(new TextModel { vi = "Sản phẩm", en = "Products" });
      list.Add(new TextModel { vi = "Chi phí Ads", en = "Ads cost" });
      list.Add(new TextModel { vi = "Thống kê", en = "Statistical" });
      list.Add(new TextModel { vi = "Đơn khiếu nại", en = "Complaint letter" });
      list.Add(new TextModel { vi = "Đơn chuyển hoàn", en = "Refund transfer form" });
      list.Add(new TextModel { vi = "XỬ LÝ ĐƠN HÀNG", en = "ORDER PROCESSING" });
      list.Add(new TextModel { vi = "NHÂN SỰ", en = "USERS" });
      list.Add(new TextModel { vi = "CẤU HÌNH", en = "CONFIG" });
      list.Add(new TextModel { vi = "Đăng Xuất", en = "Logout" });
      list.Add(new TextModel { vi = "Đăng nhập", en = "Login" });
      list.Add(new TextModel { vi = "Thông tin tài khoản", en = "Account information" });
      list.Add(new TextModel { vi = "Tên đăng nhập", en = "Username" });
      list.Add(new TextModel { vi = "Mật khẩu", en = "Password" });
      list.Add(new TextModel { vi = "Mật khẩu mới", en = "New password" });
      list.Add(new TextModel { vi = "Họ và tên", en = "Full name" });
      list.Add(new TextModel { vi = "Hình đại diện", en = "Avatar" });
      list.Add(new TextModel { vi = "Quyền hạn", en = "Role" });
      list.Add(new TextModel { vi = "Xem thống kê", en = "View Statistics" });
      list.Add(new TextModel { vi = "Chỉnh sửa", en = "Edit" });
      list.Add(new TextModel { vi = "Xóa", en = "Delete" });
      list.Add(new TextModel { vi = "Cấp lại password", en = "Reset password" });
      list.Add(new TextModel { vi = "Tạo tài khoản", en = "Create account" });
      list.Add(new TextModel { vi = "Cấu hình hệ thống", en = "System configuration" });
      list.Add(new TextModel { vi = "Hệ thống", en = "System" });
      list.Add(new TextModel { vi = "Cấu hình chung", en = "General" });
      list.Add(new TextModel { vi = "Màu từ khóa", en = "Keyword color" });
      list.Add(new TextModel { vi = "Ngày gửi hàng", en = "Delivery date" });
      list.Add(new TextModel { vi = "Tự động làm mới đơn Google Sheet", en = "Automatically refresh Google Sheet orders" });
      list.Add(new TextModel { vi = "Tạo Shop", en = "Create Shop" });
      list.Add(new TextModel { vi = "Tạo bản sao", en = "Make a copy" });
      list.Add(new TextModel { vi = "Cấu hình Shop", en = "Shop Configuration" });
      list.Add(new TextModel { vi = "Chọn nhân viên chia đơn", en = "Choose a distribution staff" });
      list.Add(new TextModel { vi = "Khách hàng", en = "Customer" });
      list.Add(new TextModel { vi = "Điện thoại", en = "Phone" });
      list.Add(new TextModel { vi = "Địa chỉ", en = "Address" });
      list.Add(new TextModel { vi = "Ngày đặt", en = "Order date" });
      list.Add(new TextModel { vi = "Xử lý", en = "Handle" });
      list.Add(new TextModel { vi = "Chờ xử lý", en = "Waiting" });
      list.Add(new TextModel { vi = "Trạng thái", en = "Status" });
      list.Add(new TextModel { vi = "Tình trạng", en = "State" });
      list.Add(new TextModel { vi = "Nhân viên", en = "Staff" });
      list.Add(new TextModel { vi = "Từ ngày", en = "From" });
      list.Add(new TextModel { vi = "Đến ngày", en = "To" });
      list.Add(new TextModel { vi = "Doanh thu", en = "Revenue" });
      list.Add(new TextModel { vi = "Ngày chia", en = "Split date" });
      list.Add(new TextModel { vi = "Danh mục", en = "Category" });
      list.Add(new TextModel { vi = "Chưa có lịch sử xử lý", en = "No processing history" });
      list.Add(new TextModel { vi = "Ghi chú", en = "Note" });
      list.Add(new TextModel { vi = "Nhấn Enter để lưu", en = "Press Enter to save" });
      list.Add(new TextModel { vi = "Xem lịch sử", en = "View history" });
      list.Add(new TextModel { vi = "Ngày hủy", en = "Cancel date" });
      list.Add(new TextModel { vi = "Ngày lên đơn", en = "Create date" });
      list.Add(new TextModel { vi = "Đã có lỗi", en = "Error" });
      list.Add(new TextModel { vi = "Tất cả", en = "All" });
      list.Add(new TextModel { vi = "- Chọn -", en = "- Select -" });
      list.Add(new TextModel { vi = "Thêm sản phẩm mới", en = "Add new product" });
      list.Add(new TextModel { vi = "Thêm sản phẩm combo", en = "Add combo product" });
      list.Add(new TextModel { vi = "Tên trong Google Sheet", en = "Name in Google Sheet" });
      list.Add(new TextModel { vi = "Tên sản phẩm", en = "Product name" });
      list.Add(new TextModel { vi = "Mã sản phẩm", en = "Product code" });
      list.Add(new TextModel { vi = "Mã mẫu mã", en = "Model code" });
      list.Add(new TextModel { vi = "Đơn giá", en = "Price" });
      list.Add(new TextModel { vi = "Giảm giá", en = "Discount" });
      list.Add(new TextModel { vi = "Số lượng", en = "Quantity" });
      list.Add(new TextModel { vi = "Trọng lượng", en = "Weight" });
      list.Add(new TextModel { vi = "Sản phẩm combo", en = "Combo product" });
      list.Add(new TextModel { vi = "Tìm sản phẩm cần thêm vào combo...", en = "Find products to add to combo..." });
      list.Add(new TextModel { vi = "Ngày", en = "Date" });
      list.Add(new TextModel { vi = "Chi phí", en = "Cost" });
      list.Add(new TextModel { vi = "Tỷ giá", en = "Rate" });
      list.Add(new TextModel { vi = "Hôm nay", en = "Today" });
      list.Add(new TextModel { vi = "Hôm qua", en = "Yesterday" });
      list.Add(new TextModel { vi = "Tuần này", en = "This week" });
      list.Add(new TextModel { vi = "Tháng này", en = "This month" });
      list.Add(new TextModel { vi = "Tháng trước", en = "Last month" });
      list.Add(new TextModel { vi = "Tất cả nhân viên", en = "All staff" });
      list.Add(new TextModel { vi = "Tất cả sản phẩm", en = "All product" });
      list.Add(new TextModel { vi = "Sản phẩm không xác định", en = "Unknown product" });
      list.Add(new TextModel { vi = "Tổng data", en = "All data" });
      list.Add(new TextModel { vi = "Đã xử lý", en = "Processed" });
      list.Add(new TextModel { vi = "Đơn chốt", en = "Accept" });
      list.Add(new TextModel { vi = "Tỉ lệ chốt", en = "Accept rate" });
      list.Add(new TextModel { vi = "Trung bình đơn chốt", en = "Average order price" });
      list.Add(new TextModel { vi = "ADS/Data", en = "ADS/Data" });
      list.Add(new TextModel { vi = "ADS/Chốt", en = "ADS/Accept" });
      list.Add(new TextModel { vi = "ADS/Doanh thu", en = "ADS/Revenue" });
      list.Add(new TextModel { vi = "Không tìm thấy dữ liệu", en = "No data found" });
      list.Add(new TextModel { vi = "Huyện xã", en = "Baragay" });
      list.Add(new TextModel { vi = "Tỉnh", en = "Province" });
      list.Add(new TextModel { vi = "Thành phố", en = "City" });

      list.Add(new TextModel { vi = "Cập nhật", en = "Update" });
      list.Add(new TextModel { vi = "Quay lại", en = "Back" });
      list.Add(new TextModel { vi = "Thêm mới", en = "Add new" });
      list.Add(new TextModel { vi = "Làm mới", en = "Refresh" });
      list.Add(new TextModel { vi = "Tìm kiếm", en = "Search" });
      list.Add(new TextModel { vi = "Lưu", en = "Save" });
      list.Add(new TextModel { vi = "Hủy", en = "Cancel" });
      list.Add(new TextModel { vi = "Hủy đơn", en = "Cancel" });
      list.Add(new TextModel { vi = "Lên đơn", en = "Create" });
      list.Add(new TextModel { vi = "Nhân bản", en = "Cloned" });
      list.Add(new TextModel { vi = "Khôi phục", en = "Restore" });
      list.Add(new TextModel { vi = "Thử lại tất cả", en = "Try them all again" });

      return list;
    }

    public static string Get(string text)
    {
      var item = All().SingleOrDefault(x => x.vi == text);
      if (item != null)
      {
        HttpCookie cookie = System.Web.HttpContext.Current.Request.Cookies["_language"];
        if(cookie != null)
        {
          if (cookie.Value == "vi")
            return item.vi;
          else if (cookie.Value == "en")
            return item.en;
          else
            return item.vi;
        }
        return item.vi;
      }
      else
        return text;
    }
  }
}
